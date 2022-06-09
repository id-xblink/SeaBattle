using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfSeaTest
{
	public partial class SoloW : Window
	{
		internal bool IsAdvanced = false;

		List<CellData> dataHits = new List<CellData>();
		int memoryLength = 0;
		bool axis = false;

		internal List<CellData> LeftCells { get; set; } = new List<CellData>(); //Данные игрока
		internal List<CellData> RightCells { get; set; } = new List<CellData>(); //Данные бота

		List<Button> buttons = new List<Button>(); //Клетки проверок бота
		List<Button> offbuttons = new List<Button>(); //Клетки для закросок от проверок бота

		static Random random = new Random();

		string[] symbol = new string[10] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
		string[] number = new string[10] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

		GameStat Game = new GameStat();

		int LeftMiss = 0;
		List<int> LeftHit = new List<int>();
		List<int> LeftKill = new List<int>();

		int RightMiss = 0;
		List<int> RightHit = new List<int>();
		List<int> RightKill = new List<int>();

		bool IsLeftMoveFirst = false;

		public SoloW()
		{
			InitializeComponent();
		}

		private async void Window_LoadedAsync(object sender, RoutedEventArgs e)
		{
			Game.Start = DateTime.Now;
			Fill(GLeft, true);
			Fill(GRight, false);
			if (random.Next(0, 2) == 1)
			{
				//Первым ходит бот
				GRight.IsEnabled = false;
				await Test();
			}
			else
			{
				//Первым ходит игрок
				IsLeftMoveFirst = true;
			}
		}

		private void Fill(Grid grid, bool side)
		{
			int k = 0;

			for (int i = 0; i < 11; i++)
			{
				grid.ColumnDefinitions.Add(new ColumnDefinition());
				grid.RowDefinitions.Add(new RowDefinition());
			}

			for (int i = 0; i <= 10; i++)
			{
				for (int j = 0; j <= 10; j++)
				{
					if (i == 0 && j != i)
					{
						TextBlock tb = new TextBlock
						{
							Text = symbol[j - 1],
							FontSize = 14,
							FontWeight = FontWeights.Bold,
							HorizontalAlignment = HorizontalAlignment.Center,
							VerticalAlignment = VerticalAlignment.Center,
						};
						Grid.SetRow(tb, i);
						Grid.SetColumn(tb, j);
						grid.Children.Add(tb);
					}
					else
					if (j == 0 && i != j)
					{
						TextBlock tb = new TextBlock
						{
							Text = number[i - 1],
							FontSize = 14,
							FontWeight = FontWeights.Bold,
							HorizontalAlignment = HorizontalAlignment.Center,
							VerticalAlignment = VerticalAlignment.Center,
						};
						Grid.SetRow(tb, i);
						Grid.SetColumn(tb, j);
						grid.Children.Add(tb);
					}
					else
					if (j != 0 && i != 0)
					{
						Button button = new Button
						{
							Content = k,
							Background = Brushes.LightBlue,
						};

						if (side)
						{
							button.Tag = LeftCells[k];

							Button offbutton = new Button
							{
								Tag = LeftCells[k],
								Background = Brushes.LightBlue,
							};

							if (LeftCells[k].busy)
							{
								button.Background = Brushes.Blue;
								offbutton.Background = Brushes.Blue;
							}

							Grid.SetRow(offbutton, i);
							Grid.SetColumn(offbutton, j);
							grid.Children.Add(offbutton);

							offbuttons.Add(offbutton);
						}
						else
						{
							button.Tag = RightCells[k];
						}

						if (side)
						{
							//Левая сторона
							buttons.Add(button);
							button.Click += Button_Bot_ClickAsync;
						}
						else
						{
							//Правая сторона
							button.Click += Button_Player_ClickAsync;
						}

						if (!side)
						{
							//Для бота атакующего игрока
							Grid.SetRow(button, i);
							Grid.SetColumn(button, j);
							grid.Children.Add(button);
						}
						
						k++;
					}
				}
			}
		}

		private async void Button_Player_ClickAsync(object sender, RoutedEventArgs e)
		{
			if (!Button_Hit_Click(sender, e))
			{
				//Промах
				GRight.IsEnabled = false; //Выключить на период атаки
				await Test();
			}
			else
			{
				//Попадание по правой части
				int points = Convert.ToInt32(GRight.Tag) - 1;
				GRight.Tag = points;
				if (points == 0)
				{
					//Победа левой части
					MessageBox.Show("Игрок победил", "SeaBattle", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
					SaveGame();
				}
			}
		}

		private async Task Test()
		{
			await Task.Delay(random.Next(250, 751));
			if (memoryLength == 0)
			{
				//Выбираем если нет раненных кораблей (ищем новые)
				int id = random.Next(0, buttons.Count); //Индекс на удаление
				Button choose = buttons[id]; //Длина (значение) корабля
				buttons.RemoveAt(id); //Удаление из листа
				choose.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
			}
			else
			{
				//Нужно ударить по краям
				//Лист возможных атак
				List<Button> vs = new List<Button>();

				//Просматриваем координаты попаданий
				foreach (CellData location in dataHits)
				{
					//Проверка клеток вокруг
					int x1 = location.x - 1; //Начальная координата x
					int y1 = location.y - 1; //Начальная координата y
					int x2 = x1 + 2; //Конечная координата x
					int y2 = y1 + 2; //Конечная координата y
					//Просмотр всех кнопок вокруг начальной точки
					foreach (Button round in buttons)
					{
						CellData rdata = (CellData)round.Tag;
						if (rdata.x >= x1 && rdata.x <= x2 && rdata.y >= y1 && rdata.y <= y2)
						{
							if (dataHits.Count == 1)
							{
								//Если только одно попадание, то берём клетки вокруг
								if (rdata.x == location.x || rdata.y == location.y)
								{
									Button button = offbuttons.FirstOrDefault(c => ((CellData)c.Tag).x == rdata.x && ((CellData)c.Tag).y == rdata.y);
									vs.Add(round);
								}
							}
							else
							{
								//Если попали повторно, выбираем ориентацию для следующей атаки
								if (axis)
								{
									//Ориентация корабля горизонтальная
									if (rdata.x == location.x)
									{
										Button button = offbuttons.FirstOrDefault(c => ((CellData)c.Tag).x == rdata.x && ((CellData)c.Tag).y == rdata.y);
										vs.Add(round);
									}
								}
								else
								{
									//Ориентация корабля вертикальная
									if (rdata.y == location.y)
									{
										Button button = offbuttons.FirstOrDefault(c => ((CellData)c.Tag).x == rdata.x && ((CellData)c.Tag).y == rdata.y);
										vs.Add(round);
									}
								}
							}
						}
					}
				}

				int id = random.Next(0, vs.Count); //Индекс на удаление
				Button choose = vs[id]; //Длина (значение) корабля
				buttons.Remove(choose); //Удаление из листа
				choose.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
			}
		}

		private void ClearRound(int ox, int oy)
		{
			int x1 = ox - 1; //Начальная координата x
			int y1 = oy - 1; //Начальная координата y
			int x2 = x1 + 2; //Конечная координата x
			int y2 = y1 + 2; //Конечная координата y

			List<Button> vs = new List<Button>();

			foreach (Button round in buttons)
			{
				CellData rdata = (CellData)round.Tag;
				if (rdata.x >= x1 && rdata.x <= x2 && rdata.y >= y1 && rdata.y <= y2)
				{
					Button button = offbuttons.FirstOrDefault(c => ((CellData)c.Tag).x == rdata.x && ((CellData)c.Tag).y == rdata.y);
					vs.Add(round);
				}
			}
			foreach (Button remove in vs)
			{
				buttons.Remove(remove);
			}
		}

		private void RefreshData(Button button)
		{
			CellData cell = (CellData)button.Tag;
			RightKill.Add(cell.length);
			button.Background = Brushes.Orange;
			foreach (CellData data in dataHits)
			{
				ClearRound(data.x, data.y);
			}
			dataHits.Clear();
			memoryLength = 0;
		}

		private async void Button_Bot_ClickAsync(object sender, RoutedEventArgs e)
		{
			if (!Button_Hit_Click(sender, e))
			{
				GRight.IsEnabled = true; //Выключить на период атаки
			}
			else
			{
				//Попадание по левой части
				int points = Convert.ToInt32(GLeft.Tag) - 1;
				GLeft.Tag = points;
				if (points == 0)
				{
					//Победа правой части
					MessageBox.Show("Компьютер победил", "SeaBattle", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
					SaveGame();
				}
				else
				{
					//Если бот продолжает атаку (по игроку)
					await Test();
				}
			}
		}

		private bool Button_Hit_Click(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			CellData data = (CellData)button.Tag;
			if (data.busy)
			{
				//Попадание
				if (!GRight.IsEnabled)
				{
					//Атаковал бот
					RightHit.Add(data.length);
					Button mybutton = offbuttons.FirstOrDefault(c => ((CellData)c.Tag).x == data.x && ((CellData)c.Tag).y == data.y);
					mybutton.Background = Brushes.Red;
					dataHits.Add(new CellData(data.x, data.y, data.busy, data.length, data.number, data.health));
					switch (memoryLength)
					{
						case 0:
							{
								if (data.length == 1)
								{
									RefreshData(mybutton);
								}
								else
								{
									memoryLength = data.length; //Установка длины корабля по которому попали
								}
								break;
							}
						case 2:
							{
								RefreshData(mybutton);
								break;
							}
						case 3:
						case 4:
							{
								//Узнаём ориентацию корабля
								if (dataHits.Count < memoryLength)
								{
									if (data.x == dataHits[0].x)
									{
										//По оси x (горизонтально)
										axis = true;
									}
									else
									{
										//По оси y (вертикально)
										axis = false;
									}
								}
								else
								{
									RefreshData(mybutton);
								}
								break;
							}
					}
				}
				else
				{
					//Атаковал игрок
					button.Click -= Button_Player_ClickAsync;
					LeftHit.Add(data.length);
					if (data.health == 1)
					{
						button.Background = Brushes.Orange;
						LeftKill.Add(data.length);
					}
					else
					{
						button.Background = Brushes.Red;
						foreach (CellData check in RightCells)
						{
							if (data.length == check.length && data.number == check.number)
							{
								//Если совпадает длина и номер (это один корабль)
								check.health -= 1;
							}
						}
					}
				}
				return true;
			}
			else
			{
				//Промах
				button.Background = Brushes.Gray;
				if (!GRight.IsEnabled)
				{
					//Атаковал бот
					Button mybutton = offbuttons.FirstOrDefault(c => ((CellData)c.Tag).x == data.x && ((CellData)c.Tag).y == data.y);
					mybutton.Background = Brushes.Gray;
					RightMiss++;
				}
				else
				{
					//Атаковал игрок
					button.Click -= Button_Player_ClickAsync;
					LeftMiss++;
				}
				return false;
			}
		}

		private void SaveGame()
		{
			//Игра закончена
			Game.End = DateTime.Now;
			
			//Убито левым
			int k4 = LeftKill.Where(x => x == 4).Count();
			int k3 = LeftKill.Where(x => x == 3).Count();
			int k2 = LeftKill.Where(x => x == 2).Count();
			int k1 = LeftKill.Where(x => x == 1).Count();
			//Нанесено урона левым
			int d4 = LeftHit.Where(x => x == 4).Count();
			int d3 = LeftHit.Where(x => x == 3).Count();
			int d2 = LeftHit.Where(x => x == 2).Count();
			int d1 = LeftHit.Where(x => x == 1).Count();
			//Потеряно левым
			int l4 = RightKill.Where(x => x == 4).Count();
			int l3 = RightKill.Where(x => x == 3).Count();
			int l2 = RightKill.Where(x => x == 2).Count();
			int l1 = RightKill.Where(x => x == 1).Count();
			//Получено урона левым
			int r4 = RightHit.Where(x => x == 4).Count();
			int r3 = RightHit.Where(x => x == 3).Count();
			int r2 = RightHit.Where(x => x == 2).Count();
			int r1 = RightHit.Where(x => x == 1).Count();

			int[] k = new int[4] { k4, k3, k2, k1 }; //Убито левым
			int[] d = new int[4] { d4, d3, d2, d1 }; //Нанесено урона левым
			int[] l = new int[4] { l4, l3, l2, l1 }; //Потеряно левым
			int[] r = new int[4] { r4, r3, r2, r1 }; //Получено урона левым

			Game.My_Hit = LeftHit.Count;
			Game.My_Miss = LeftMiss;

			Game.Enemy_Hit = RightHit.Count;
			Game.Enemy_Miss = RightMiss;

			if (Convert.ToInt32(GRight.Tag) == 0)
				Game.Victory = 1; //Победа левого игрока
			else
				Game.Victory = 0; //Поражение левого игрока

			//Первый ход для левого
			if (IsLeftMoveFirst)
				Game.First_Move = 1;
			else
				Game.First_Move = 0;

			try
			{
				DBConnector.MainConnection.Open();

				string idDestroy = new MySqlCommand($"INSERT INTO `destroy` (`destroy_4`, `destroy_3`, `destroy_2`, `destroy_1`) VALUES ({k[0]}, {k[1]}, {k[2]}, {k[3]}); SELECT LAST_INSERT_ID();", DBConnector.MainConnection).ExecuteScalar().ToString();

				string idDeal = new MySqlCommand($"INSERT INTO `deal` (`deal_4`, `deal_3`, `deal_2`, `deal_1`) VALUES ({d[0]}, {d[1]}, {d[2]}, {d[3]}); SELECT LAST_INSERT_ID();", DBConnector.MainConnection).ExecuteScalar().ToString();

				string idLost = new MySqlCommand($"INSERT INTO `lost` (`lost_4`, `lost_3`, `lost_2`, `lost_1`) VALUES ({l[0]}, {l[1]}, {l[2]}, {l[3]}); SELECT LAST_INSERT_ID();", DBConnector.MainConnection).ExecuteScalar().ToString();

				string idReceive = new MySqlCommand($"INSERT INTO `receive` (`receive_4`, `receive_3`, `receive_2`, `receive_1`) VALUES ({r[0]}, {r[1]}, {r[2]}, {r[3]}); SELECT LAST_INSERT_ID();", DBConnector.MainConnection).ExecuteScalar().ToString();

				string idStat = new MySqlCommand($"INSERT INTO `stat` (`destroy_id`, `deal_id`, `lost_id`, `receive_id`, `my_hit`, `my_miss`, `enemy_hit`, `enemy_miss`, `first_move`) VALUES ({idDestroy}, {idDeal}, {idLost}, {idReceive}, {Game.My_Hit}, {Game.My_Miss}, {Game.Enemy_Hit}, {Game.Enemy_Miss}, {Game.First_Move}); SELECT LAST_INSERT_ID();", DBConnector.MainConnection).ExecuteScalar().ToString();

				string idGame = new MySqlCommand($"INSERT INTO `game` (`datetime_start`, `datetime_end`, `type_id`, `user_id`, `stat_id`, `victory`) VALUES ('{Game.Start.ToString("yyyy-MM-dd HH:mm:ss")}', '{Game.End.ToString("yyyy-MM-dd HH:mm:ss")}', {1}, {DBConnector.IdUser}, {idStat}, {Game.Victory}); SELECT LAST_INSERT_ID();", DBConnector.MainConnection).ExecuteScalar().ToString();

				DBConnector.MainConnection.Close();
			}
			catch (Exception)
			{
				MessageBox.Show("Не удалось сохранить данные игры. Возможно нет подключения к интернету. Проверьте подключение и запустите приложение заного", "SeaBattle", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
				Environment.Exit(0);
			}

			MenuW mw = new MenuW();
			Close();
			mw.Show();
		}
	}
}