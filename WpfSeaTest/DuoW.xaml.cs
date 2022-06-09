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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfSeaTest
{
	public partial class DuoW : Window
	{
		internal List<CellData> LeftCells { get; set; } = new List<CellData>();
		internal List<CellData> RightCells { get; set; } = new List<CellData>();

		string[] symbol = new string[10] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
		string[] number = new string[10] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
		static Random random = new Random();

		GameStat Game = new GameStat();
		
		int LeftMiss = 0;
		List<int> LeftHit = new List<int>();
		List<int> LeftKill = new List<int>();
		
		int RightMiss = 0;
		List<int> RightHit = new List<int>();
		List<int> RightKill = new List<int>();

		bool IsLeftMoveFirst = false;
		
		public DuoW()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Game.Start = DateTime.Now;
			Fill(GLeft, true);
			Fill(GRight, false);

			int result = random.Next(0, 2);
			if (result == 0)
			{
				GLeft.IsEnabled = false;
				IsLeftMoveFirst = true; //Ходит первым левый
			}
			else
			{
				GRight.IsEnabled = false;
				IsLeftMoveFirst = false; //Левый не ходит первым
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
						}
						else
						{
							button.Tag = RightCells[k];
						}

						button.Click += Button_Swap_Click;

						Grid.SetRow(button, i);
						Grid.SetColumn(button, j);
						grid.Children.Add(button);
						k++;
					}
				}
			}
		}

		private bool Button_Hit_Click(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			button.Click -= Button_Swap_Click;
			CellData data = (CellData)button.Tag;
			if (data.busy)
			{
				if (GLeft.IsEnabled)
				{
					//Атака по левой стороне
					RightHit.Add(data.length);
				}
				else
				{
					//Атака по правой стороне
					LeftHit.Add(data.length);
				}
				//Описание попадания
				if (data.health == 1)
				{
					if (GLeft.IsEnabled)
					{
						//Атака по левой стороне
						RightKill.Add(data.length);
					}
					else
					{
						//Атака по правой стороне
						LeftKill.Add(data.length);
					}
					button.Background = Brushes.Orange;
				}
				else
				{
					button.Background = Brushes.Red;
					List<CellData> attacked = null;
					if (GLeft.IsEnabled)
					{
						//Атака по левой стороне
						attacked = LeftCells;
					}
					else
					{
						//Атака по правой стороне
						attacked = RightCells;
					}

					foreach (CellData check in attacked)
					{
						if (data.length == check.length && data.number == check.number)
						{
							//Если совпадает длина и номер (это один корабль)
							check.health -= 1;
						}
					}
				}
				return true;
			}
			else
			{
				if (GLeft.IsEnabled)
				{
					//Атака по левой стороне
					RightMiss++;
				}
				else
				{
					//Атака по правой стороне
					LeftMiss++;
				}
				button.Background = Brushes.Gray;
				return false;
			}
		}

		private void Button_Swap_Click(object sender, RoutedEventArgs e)
		{
			if (!Button_Hit_Click(sender, e))
			{
				//Промах
				GLeft.IsEnabled = !GLeft.IsEnabled;
				GRight.IsEnabled = !GRight.IsEnabled;
			}
			else
			{
				//Попадание
				bool IsGameEnd = false;

				if (GLeft.IsEnabled)
				{
					//Попадание по левой части
					int points = Convert.ToInt32(GLeft.Tag) - 1;
					GLeft.Tag = points;
					if (points == 0)
					{
						//Победа правой части
						MessageBox.Show("Правый игрок победил", "SeaBattle", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
						IsGameEnd = true;
					}
				}
				else
				{
					//Попадание по правой части
					int points = Convert.ToInt32(GRight.Tag) - 1;
					GRight.Tag = points;
					if (points == 0)
					{
						//Победа левой части
						MessageBox.Show("Левый игрок победил", "SeaBattle", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
						IsGameEnd = true;
					}
				}

				if (IsGameEnd)
				{
					Game.End = DateTime.Now;
					//Игра закончена

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

					int[] k = new int[4] { k4, k3, k2, k1}; //Убито левым
					int[] d = new int[4] { d4, d3, d2, d1}; //Нанесено урона левым
					int[] l = new int[4] { l4, l3, l2, l1}; //Потеряно левым
					int[] r = new int[4] { r4, r3, r2, r1}; //Получено урона левым

					Game.My_Hit = LeftHit.Count;
					Game.My_Miss = LeftMiss;

					Game.Enemy_Hit = RightHit.Count;
					Game.Enemy_Miss = RightMiss;
					
					MessageBoxResult result = MessageBox.Show("Записать статистику левого игрока? Иначе будет записана статистика правого игрока", "SeaBattle", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
					if (result == MessageBoxResult.Yes)
					{
						//Запись статистики левого игрока
						if (Convert.ToInt32(GRight.Tag) == 0)
							Game.Victory = 1; //Победа левого игрока
						else
							Game.Victory = 0; //Поражение левого игрока

						//Первый ход для левого
						if (IsLeftMoveFirst)
							Game.First_Move = 1;
						else
							Game.First_Move = 0;

						SaveGame(k, d, l, r, Game.My_Hit, Game.My_Miss, Game.Enemy_Hit, Game.Enemy_Miss);
					}
					else
					{
						//Запись статистики правого игрока
						if (Convert.ToInt32(GLeft.Tag) == 0)
							Game.Victory = 1; //Победа правого игрока
						else
							Game.Victory = 0; //Поражение правого игрока

						//Первый ход для правого
						if (IsLeftMoveFirst)
							Game.First_Move = 0;
						else
							Game.First_Move = 1;

						SaveGame(l, r, k, d, Game.Enemy_Hit, Game.Enemy_Miss, Game.My_Hit, Game.My_Miss);
					}

					MenuW mw = new MenuW();
					Close();
					mw.Show();
				}
			}
		}

		private void SaveGame(int[] k, int[] d, int[] l, int[] r, int MH, int MM, int EH, int EM)
		{
			try
			{
				DBConnector.MainConnection.Open();

				string idDestroy = new MySqlCommand($"INSERT INTO `destroy` (`destroy_4`, `destroy_3`, `destroy_2`, `destroy_1`) VALUES ({k[0]}, {k[1]}, {k[2]}, {k[3]}); SELECT LAST_INSERT_ID();", DBConnector.MainConnection).ExecuteScalar().ToString();

				string idDeal = new MySqlCommand($"INSERT INTO `deal` (`deal_4`, `deal_3`, `deal_2`, `deal_1`) VALUES ({d[0]}, {d[1]}, {d[2]}, {d[3]}); SELECT LAST_INSERT_ID();", DBConnector.MainConnection).ExecuteScalar().ToString();

				string idLost = new MySqlCommand($"INSERT INTO `lost` (`lost_4`, `lost_3`, `lost_2`, `lost_1`) VALUES ({l[0]}, {l[1]}, {l[2]}, {l[3]}); SELECT LAST_INSERT_ID();", DBConnector.MainConnection).ExecuteScalar().ToString();

				string idReceive = new MySqlCommand($"INSERT INTO `receive` (`receive_4`, `receive_3`, `receive_2`, `receive_1`) VALUES ({r[0]}, {r[1]}, {r[2]}, {r[3]}); SELECT LAST_INSERT_ID();", DBConnector.MainConnection).ExecuteScalar().ToString();

				string idStat = new MySqlCommand($"INSERT INTO `stat` (`destroy_id`, `deal_id`, `lost_id`, `receive_id`, `my_hit`, `my_miss`, `enemy_hit`, `enemy_miss`, `first_move`) VALUES ({idDestroy}, {idDeal}, {idLost}, {idReceive}, {MH}, {MM}, {EH}, {EM}, {Game.First_Move}); SELECT LAST_INSERT_ID();", DBConnector.MainConnection).ExecuteScalar().ToString();

				string idGame = new MySqlCommand($"INSERT INTO `game` (`datetime_start`, `datetime_end`, `type_id`, `user_id`, `stat_id`, `victory`) VALUES ('{Game.Start.ToString("yyyy-MM-dd HH:mm:ss")}', '{Game.End.ToString("yyyy-MM-dd HH:mm:ss")}', {2}, {DBConnector.IdUser}, {idStat}, {Game.Victory}); SELECT LAST_INSERT_ID();", DBConnector.MainConnection).ExecuteScalar().ToString();

				DBConnector.MainConnection.Close();
			}
			catch (Exception)
			{
				MessageBox.Show("Не удалось сохранить данные игры. Возможно нет подключения к интернету. Проверьте подключение и запустите приложение заного", "SeaBattle", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
				Environment.Exit(0);
			}
		}
	}
}