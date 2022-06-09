using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
	public partial class OnlineW : Window
	{
		static ServerObject server; // сервер
		static Thread listenThread; // потока для прослушивания

		static string userName;
		private const string host = "127.0.0.1";
		private const int port = 8888;
		static TcpClient client;
		static NetworkStream stream;
		
		public OnlineW()
		{
			InitializeComponent();
		}

		List<CellData> MyCells = new List<CellData>();
		List<CellData> EnemyCells = new List<CellData>();
		List<Button> MyButtons = new List<Button>();
		List<Button> EnemyButtons = new List<Button>();
		List<Button> buttons = new List<Button>();
		List<Button> offbuttons = new List<Button>(); //Клетки для закросок от проверок другого игрока

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

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Fill(GArea);
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
							button.Tag = MyCells[k];
							if (MyCells[k].busy)
								button.Background = Brushes.Blue;
							button.Click += Button_Local_Swap_Click;
							MyButtons.Add(button);

							Button offbutton = new Button
							{
								Tag = MyCells[k],
								Background = Brushes.LightBlue,
							};

							if (MyCells[k].busy)
								offbutton.Background = Brushes.Blue;

							Grid.SetRow(offbutton, i);
							Grid.SetColumn(offbutton, j);
							grid.Children.Add(offbutton);

							offbuttons.Add(offbutton);
						}
						else
						{
							button.Tag = EnemyCells[k];
							button.Click += Button_Swap_Click;
							EnemyButtons.Add(button);
						}
						
						if (!side)
						{
							Grid.SetRow(button, i);
							Grid.SetColumn(button, j);
							grid.Children.Add(button);
						}
						k++;
					}
				}
			}
		}

		private void Fill(Grid grid)
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
							FontSize = 15,
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
							FontSize = 15,
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
							Width = 40,
							Height = 40,
							AllowDrop = true,
							Background = Brushes.LightBlue,
							Tag = new CellData(j, i, false, 0, 0, 0),
							BorderThickness = new Thickness(1),
							Padding = new Thickness(0),
						};

						button.Drop += Button_Drop;

						Grid.SetRow(button, i);
						Grid.SetColumn(button, j);
						grid.Children.Add(button);

						buttons.Add(button);

						k++;
					}
				}
			}
		}

		private void Button_Drop(object sender, DragEventArgs e)
		{
			Button button = (Button)sender;
			CellData cell = (CellData)button.Tag;
			int length = Convert.ToInt32(e.Data.GetData(DataFormats.Text));
			SetInArea(length, cell, true, true);
		}

		private bool SetInArea(int length, CellData cell, bool transform, bool warning) //Длина корабля, данные клетки куда ставится корабль
		{
			if (transform)
			{
				//Ориентация определяется по гриду
				switch (length)
				{
					case 4:
						{
							transform = Convert.ToBoolean(G4.Tag);
							break;
						}
					case 3:
						{
							transform = Convert.ToBoolean(G3.Tag);
							break;
						}
					case 2:
						{
							transform = Convert.ToBoolean(G2.Tag);
							break;
						}
					case 1:
						{
							transform = Convert.ToBoolean(G1.Tag);
							break;
						}
				}
			}
			else
			{
				//Ориентация выбирается рандомно
				int result = random.Next(0, 2);
				if (result == 0)
					transform = false;
				else
					transform = true;
			}

			List<Button> allow = new List<Button>();

			if (transform)
			{
				if (cell.y + length - 1 > 10)
				{
					if (warning)
						MessageBox.Show("Недостаточно места для установки корабля", "SeaBattle", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
					return false;
				}
			}
			else
			{
				if (cell.x + length - 1 > 10)
				{
					if (warning)
						MessageBox.Show("Недостаточно места для установки корабля", "SeaBattle", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
					return false;
				}
			}

			foreach (Button check in buttons)
			{
				CellData data = (CellData)check.Tag;
				if (transform)
				{
					//Проверка клеток по линии
					if (data.x == cell.x && data.y >= cell.y && data.y <= cell.y + length - 1 && !data.busy) //Строка равна и столбцев в пределах
					{
						allow.Add(check); //Добавление клетки
					}
				}
				else
				{
					if (data.y == cell.y && data.x >= cell.x && data.x <= cell.x + length - 1 && !data.busy) //Столбец равен и строки в пределах
					{
						allow.Add(check); //Добавление клетки
					}
				}
			}

			foreach (Button check in allow) //Проверка клеток вокруг
			{
				CellData data = (CellData)check.Tag;
				//Проверка клеток вокруг
				int x1 = data.x - 1; //Начальная координата x
				int y1 = data.y - 1; //Начальная координата y
				int x2 = x1 + 2; //Конечная координата x
				int y2 = y1 + 2; //Конечная координата y
				foreach (Button round in buttons)
				{
					CellData rdata = (CellData)round.Tag;
					if (rdata.x >= x1 && rdata.x <= x2 && rdata.y >= y1 && rdata.y <= y2)
					{
						if (rdata.x != data.x || rdata.y != data.y)
						{
							if (rdata.busy)
							{
								if (warning)
									MessageBox.Show("Пространство занято другим кораблём", "SeaBattle", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
								return false;
							}
						}
					}
				}
			}

			if (allow.Count == length)
			{
				//Если необходимые клетки свободны/доступны
				foreach (Button check in allow)
				{
					CellData data = (CellData)check.Tag;
					check.Background = Brushes.Blue;
					data.busy = true;
					data.length = length;
					switch (length)
					{
						case 4:
							{
								data.number = Convert.ToInt32(TBCount4.Tag);
								break;
							}
						case 3:
							{
								data.number = Convert.ToInt32(TBCount3.Tag);
								break;
							}
						case 2:
							{
								data.number = Convert.ToInt32(TBCount2.Tag);
								break;
							}
						case 1:
							{
								data.number = Convert.ToInt32(TBCount1.Tag);
								break;
							}
					}
					data.health = length;
				}

				switch (length)
				{
					case 4:
						{
							TBCount4.Tag = Convert.ToInt32(TBCount4.Tag) - 1;
							TBCount4.Text = $"Осталось: {TBCount4.Tag.ToString()}";
							if (Convert.ToInt32(TBCount4.Tag) == 0)
								G4.IsEnabled = false;
							break;
						}
					case 3:
						{
							TBCount3.Tag = Convert.ToInt32(TBCount3.Tag) - 1;
							TBCount3.Text = $"Осталось: {TBCount3.Tag.ToString()}";
							if (Convert.ToInt32(TBCount3.Tag) == 0)
								G3.IsEnabled = false;
							break;
						}
					case 2:
						{
							TBCount2.Tag = Convert.ToInt32(TBCount2.Tag) - 1;
							TBCount2.Text = $"Осталось: {TBCount2.Tag.ToString()}";
							if (Convert.ToInt32(TBCount2.Tag) == 0)
								G2.IsEnabled = false;
							break;
						}
					case 1:
						{
							TBCount1.Tag = Convert.ToInt32(TBCount1.Tag) - 1;
							TBCount1.Text = $"Осталось: {TBCount1.Tag.ToString()}";
							if (Convert.ToInt32(TBCount1.Tag) == 0)
								G1.IsEnabled = false;
							break;
						}
				}
				return true;
			}
			else
			{
				//Клетки заняты
				if (warning)
					MessageBox.Show("Нельзя ставить корабли друг на друга", "SeaBattle", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
				return false;
			}
		}

		private void BClear_Click(object sender, RoutedEventArgs e)
		{
			foreach (Button check in buttons)
			{
				check.Background = Brushes.LightBlue;
				CellData data = (CellData)check.Tag;
				data.busy = false;
			}

			G4.IsEnabled = true;
			G3.IsEnabled = true;
			G2.IsEnabled = true;
			G1.IsEnabled = true;

			G4.Tag = false;
			G3.Tag = false;
			G2.Tag = false;
			G1.Tag = false;

			RotateTransform rotateTransform = G4.RenderTransform as RotateTransform;
			rotateTransform.Angle = 0;
			rotateTransform = G3.RenderTransform as RotateTransform;
			rotateTransform.Angle = 0;
			rotateTransform = G2.RenderTransform as RotateTransform;
			rotateTransform.Angle = 0;
			rotateTransform = G1.RenderTransform as RotateTransform;
			rotateTransform.Angle = 0;

			TBCount4.Text = $"Осталось: 1";
			TBCount3.Text = $"Осталось: 2";
			TBCount2.Text = $"Осталось: 3";
			TBCount1.Text = $"Осталось: 4";

			TBCount4.Tag = 1;
			TBCount3.Tag = 2;
			TBCount2.Tag = 3;
			TBCount1.Tag = 4;
		}

		private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			Rectangle rect = (Rectangle)sender;
			DragDrop.DoDragDrop(rect, rect.Tag.ToString(), DragDropEffects.Copy);
		}

		private void Rectangle_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			RotateTransform rotateTransform = new RotateTransform();
			Rectangle rectangle = (Rectangle)sender;
			switch (Convert.ToInt32(rectangle.Tag))
			{
				case 4:
					{
						G4.Tag = !Convert.ToBoolean(G4.Tag);
						rotateTransform = G4.RenderTransform as RotateTransform;
						break;
					}
				case 3:
					{
						G3.Tag = !Convert.ToBoolean(G3.Tag);
						rotateTransform = G3.RenderTransform as RotateTransform;
						break;
					}
				case 2:
					{
						G2.Tag = !Convert.ToBoolean(G2.Tag);
						rotateTransform = G2.RenderTransform as RotateTransform;
						break;
					}
				case 1:
					{
						G1.Tag = !Convert.ToBoolean(G1.Tag);
						rotateTransform = G1.RenderTransform as RotateTransform;
						break;
					}
			}
			if (rotateTransform.Angle + 90 == 180)
				rotateTransform.Angle = 0;
			else
				rotateTransform.Angle += 90;
		}

		private void BLeave_Click(object sender, RoutedEventArgs e)
		{
			BExit_Click(sender, e);
		}

		private void BGo_Click(object sender, RoutedEventArgs e)
		{
			if (Convert.ToInt32(TBCount4.Tag) == 0 && Convert.ToInt32(TBCount3.Tag) == 0 && Convert.ToInt32(TBCount2.Tag) == 0 && Convert.ToInt32(TBCount1.Tag) == 0)
			{
				string info = "Ready";
				foreach (Button check in buttons)
				{
					CellData cell = (CellData)check.Tag;
					MyCells.Add(new CellData(cell.x, cell.y, cell.busy, cell.length, cell.number, cell.health));
					info += $"|{cell.x}:{cell.y}:{cell.busy}:{cell.length}:{cell.number}:{cell.health}";
				}

				string message = info;
				BGo.IsEnabled = false;

				byte[] data = Encoding.Unicode.GetBytes(message);
				stream.Write(data, 0, data.Length);
			}
			else
			{
				MessageBox.Show("Должны быть расставлены все корабли", "SeaBattle", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
			}
		}

		private void BRandom_Click(object sender, RoutedEventArgs e)
		{
			BClear_Click(sender, e);

			List<int> vs = new List<int>() { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };

			while (vs.Count > 0)
			{
				int id = random.Next(0, vs.Count); //Индекс на удаление
				int number = vs[id]; //Длина (значение) корабля
				vs.RemoveAt(id); //Удаление из листа
				bool placed = false;

				do
				{
					int x = random.Next(1, 11);
					int y = random.Next(1, 11);
					foreach (Button check in buttons)
					{
						CellData data = (CellData)check.Tag;
						if (data.x == x && data.y == y)
						{
							placed = SetInArea(number, data, false, false); //Ориентация фигуры определяется в первом false
							break;
						}
					}
				} while (!placed);
			}
		}

		private void BConnect_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			if (client == null)
			{
				userName = DBConnector.IdUser.ToString();
				client = new TcpClient();
				try
				{
					if (server != null)
					{
						client.Connect("", port); //подключаем себя
						TBStatus.Text = "Статус: хост; ожидание игрока";
					}
					else
					{
						client.Connect(TBIP.Text, port); //подключение клиента
						TBStatus.Text = "Статус: клиент; ожидание игрока";
					}
						
					stream = client.GetStream(); // получаем поток

					string message = userName;
					byte[] data = Encoding.Unicode.GetBytes(message);
					stream.Write(data, 0, data.Length);

					// запускаем новый поток для получения данных
					Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
					receiveThread.Start(); //старт потока
				}
				catch (Exception)
				{
					client = null;
				}
			}
		}

		private void ReceiveMessage()
		{
			while (stream != null)
			{
				try
				{
					byte[] data = new byte[64]; // буфер для получаемых данных
					StringBuilder builder = new StringBuilder();
					int bytes = 0;
					do
					{
						bytes = stream.Read(data, 0, data.Length);
						builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
					}
					while (stream.DataAvailable);

					string message = builder.ToString();
					if (message != "")
					{
						if (message.Contains("Connected"))
						{
							if (server != null)
							{
								if (server.clients.Count == 2)
								{
									if (stream != null)
									{
										string message1 = "Start";
										byte[] data1 = Encoding.Unicode.GetBytes(message1);
										stream.Write(data1, 0, data1.Length);
										PrepareGame();
									}
								}
							}
							else
							{
								if (stream != null)
								{
									string message1 = "Start";
									byte[] data1 = Encoding.Unicode.GetBytes(message1);
									stream.Write(data1, 0, data1.Length);
									PrepareGame();
								}
							}
						}
						if (message.Contains("Start"))
						{
							//Игра началась + выбор кораблей
							PrepareGame();
						}
						if (message.Contains("Ready"))
						{
							string info = message.Replace("Ready|", "");

							string[] vs = info.Split('|');
							foreach (string str in vs)
							{
								string[] dataCell = str.Split(':');
								EnemyCells.Add(new CellData(Convert.ToInt32(dataCell[0]), Convert.ToInt32(dataCell[1]), Convert.ToBoolean(dataCell[2]), Convert.ToInt32(dataCell[3]), Convert.ToInt32(dataCell[4]), Convert.ToInt32(dataCell[5])));//тут вспомнить
							}
							if (MyCells.Count != 0 && EnemyCells.Count != 0)
							{
								string message1 = "Go";
								byte[] data1 = Encoding.Unicode.GetBytes(message1);
								stream.Write(data1, 0, data1.Length);
								StartGame();
							}
						}
						if (message.Contains("Go"))
						{
							StartGame();
						}
						if (message.Contains("Attack"))
						{
							//Атакует
							Dispatcher.BeginInvoke(new ThreadStart(delegate
							{
								string result = message.Replace("Attack|", "");
								string[] vs = result.Split(':');

								foreach (Button check in MyButtons)
								{
									CellData cell = (CellData)check.Tag;
									if (cell.x == Convert.ToInt32(vs[0]) && cell.y == Convert.ToInt32(vs[1]))
										check.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
								}
							}));
						}
						if (message.Contains("End"))
						{
							Dispatcher.BeginInvoke(new ThreadStart(delegate
							{
								SaveGame();
								string[] result = message.Split('|');
								MessageBox.Show(result[1], "SeaBattle", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
								BExit_Click(null, null);
							}));
						}
					}
					else
					{
						BExit_Click(null, null);
					}
				}
				catch (Exception)
				{
					if (stream != null)
					Dispatcher.BeginInvoke(new ThreadStart(delegate
					{
						BExit_Click(null, null);
					}));
				}
			}
		}

		private void PrepareGame()
		{
			Dispatcher.BeginInvoke(new ThreadStart(delegate
			{
				GConnection.IsEnabled = false;
				GConnection.Visibility = Visibility.Collapsed;
				GChoose.Visibility = Visibility.Visible;
				Width = 780;
				Height = 500;
			}));
		}

		private void BHost_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (server == null)
				{
					server = new ServerObject();
					listenThread = new Thread(new ThreadStart(server.Listen));
					listenThread.Start(); //старт потока
					
					TBStatus.Text = "Статус: хост";
				}
				else
				{
					if (!server.GetListenState())
					{
						BDisconnect_Click(sender, e);
						BHost_Click(sender, e);
					}
				}
			}
			catch (Exception)
			{
				server.Disconnect();
			}
		}

		private void BDisconnect_Click(object sender, RoutedEventArgs e)
		{
			if (stream != null)
			{
				stream.Close();
				stream = null;
			}

			if (client != null)
			{
				client.Close();
				client = null;
			}

			if (server != null)
			{
				server.Disconnect();
				server = null;
			}
		}

		private void StartGame()
		{
			Game.Start = DateTime.Now;
			Dispatcher.BeginInvoke(new ThreadStart(delegate
			{
				Fill(GLeft, true);
				Fill(GRight, false);
				if (server == null)
					GRight.IsEnabled = false;
				else
					IsLeftMoveFirst = true;
				//Старт игры
				GChoose.Visibility = Visibility.Collapsed;
				GGame.Visibility = Visibility.Visible;
			}));
		}

		private void Leave()
		{
			MenuW mw = new MenuW();
			Close();
			mw.Show();
		}

		private void Button_Swap_Click(object sender, RoutedEventArgs e)
		{
			if (!Button_Hit_Click(sender, e))
			{
				//Промах
				GRight.IsEnabled = !GRight.IsEnabled;
			}
		}

		private bool Button_Hit_Click(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			button.Click -= Button_Swap_Click;
			CellData cell = (CellData)button.Tag;
			if (stream != null)
			{
				string message = $"Attack|{cell.x}:{cell.y}";
				byte[] data = Encoding.Unicode.GetBytes(message);
				stream.Write(data, 0, data.Length);
			}
			if (cell.busy)
			{
				LeftHit.Add(cell.length);
				if (cell.health == 1)
				{
					button.Background = Brushes.Orange;
					LeftKill.Add(cell.length);
				}
				else
				{
					button.Background = Brushes.Red;
					foreach (CellData check in EnemyCells)
					{
						if (cell.length == check.length && cell.number == check.number)
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
				button.Background = Brushes.Gray;
				LeftMiss++;
				return false;
			}
		}

		private void Button_Local_Swap_Click(object sender, RoutedEventArgs e)
		{
			if (!Button_Local_Hit_Click(sender, e))
			{
				//Промах
				GRight.IsEnabled = !GRight.IsEnabled;
			}
			else
			{
				//Попадание по левой части
				int points = Convert.ToInt32(GLeft.Tag) - 1;
				GLeft.Tag = points;
				if (points == 0)
				{
					//Победа правой части
					if (stream != null)
					{
						string message = $"End|Вы победили";
						byte[] data = Encoding.Unicode.GetBytes(message);
						stream.Write(data, 0, data.Length);
					}
					
					Dispatcher.BeginInvoke(new ThreadStart(delegate
					{
						SaveGame();
						MessageBox.Show("Вы проиграли", "SeaBattle", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
						BExit_Click(sender, e);
					}));
				}
			}
		}

		private bool Button_Local_Hit_Click(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			CellData data = (CellData)button.Tag;
			if (data.busy)
			{
				Button mybutton = offbuttons.FirstOrDefault(c => ((CellData)c.Tag).x == data.x && ((CellData)c.Tag).y == data.y);
				mybutton.Background = Brushes.Red;
				
				RightHit.Add(data.length);
				if (data.health == 1)
				{
					mybutton.Background = Brushes.Orange;
					RightKill.Add(data.length);
				}
				else
				{
					mybutton.Background = Brushes.Red;
					foreach (CellData check in MyCells)
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
				Button mybutton = offbuttons.FirstOrDefault(c => ((CellData)c.Tag).x == data.x && ((CellData)c.Tag).y == data.y);
				mybutton.Background = Brushes.Gray;
				RightMiss++;
				return false;
			}
		}

		private void BExit_Click(object sender, RoutedEventArgs e)
		{
			BDisconnect_Click(sender, e);
			Leave();
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

			DBConnector.MainConnection.Open();
			
			int elo = Convert.ToInt32(new MySqlCommand($"SELECT `elo` FROM `leaderboard` WHERE `user_id` = {DBConnector.IdUser}", DBConnector.MainConnection).ExecuteScalar());
			
			if (Convert.ToInt32(GLeft.Tag) != 0)
			{
				//Победа левого игрока
				Game.Victory = 1;
				elo += 5;
				new MySqlCommand($"UPDATE `leaderboard` SET `elo` = '{elo}' WHERE `id` = {DBConnector.IdUser};", DBConnector.MainConnection).ExecuteNonQuery();
			}
			else
			{
				//Поражение левого игрока
				Game.Victory = 0;
				elo -= 5;
				if (elo < 0)
					elo = 0;
				new MySqlCommand($"UPDATE `leaderboard` SET `elo` = '{elo}' WHERE `id` = {DBConnector.IdUser};", DBConnector.MainConnection).ExecuteNonQuery();
			}

			//Первый ход для левого
			if (IsLeftMoveFirst)
				Game.First_Move = 1;
			else
				Game.First_Move = 0;
			
			string idDestroy = new MySqlCommand($"INSERT INTO `destroy` (`destroy_4`, `destroy_3`, `destroy_2`, `destroy_1`) VALUES ({k[0]}, {k[1]}, {k[2]}, {k[3]}); SELECT LAST_INSERT_ID();", DBConnector.MainConnection).ExecuteScalar().ToString();

			string idDeal = new MySqlCommand($"INSERT INTO `deal` (`deal_4`, `deal_3`, `deal_2`, `deal_1`) VALUES ({d[0]}, {d[1]}, {d[2]}, {d[3]}); SELECT LAST_INSERT_ID();", DBConnector.MainConnection).ExecuteScalar().ToString();

			string idLost = new MySqlCommand($"INSERT INTO `lost` (`lost_4`, `lost_3`, `lost_2`, `lost_1`) VALUES ({l[0]}, {l[1]}, {l[2]}, {l[3]}); SELECT LAST_INSERT_ID();", DBConnector.MainConnection).ExecuteScalar().ToString();

			string idReceive = new MySqlCommand($"INSERT INTO `receive` (`receive_4`, `receive_3`, `receive_2`, `receive_1`) VALUES ({r[0]}, {r[1]}, {r[2]}, {r[3]}); SELECT LAST_INSERT_ID();", DBConnector.MainConnection).ExecuteScalar().ToString();

			string idStat = new MySqlCommand($"INSERT INTO `stat` (`destroy_id`, `deal_id`, `lost_id`, `receive_id`, `my_hit`, `my_miss`, `enemy_hit`, `enemy_miss`, `first_move`) VALUES ({idDestroy}, {idDeal}, {idLost}, {idReceive}, {Game.My_Hit}, {Game.My_Miss}, {Game.Enemy_Hit}, {Game.Enemy_Miss}, {Game.First_Move}); SELECT LAST_INSERT_ID();", DBConnector.MainConnection).ExecuteScalar().ToString();

			string idGame = new MySqlCommand($"INSERT INTO `game` (`datetime_start`, `datetime_end`, `type_id`, `user_id`, `stat_id`, `victory`) VALUES ('{Game.Start.ToString("yyyy-MM-dd HH:mm:ss")}', '{Game.End.ToString("yyyy-MM-dd HH:mm:ss")}', {3}, {DBConnector.IdUser}, {idStat}, {Game.Victory}); SELECT LAST_INSERT_ID();", DBConnector.MainConnection).ExecuteScalar().ToString();

			DBConnector.MainConnection.Close();
		}

		private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			System.Diagnostics.Process.Start("https://tokarchuk.pro/");
		}
	}
}