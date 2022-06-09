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
	public partial class CreateAreaW : Window
	{
		List<CellData> SaveCells = new List<CellData>();
		List<Button> buttons = new List<Button>();

		static Random random = new Random();

		public CreateAreaW()
		{
			InitializeComponent();
		}

		string[] symbol = new string[10] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
		string[] number = new string[10] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Fill(GArea);
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

		private void BGo_Click(object sender, RoutedEventArgs e)
		{
			if (Convert.ToInt32(TBCount4.Tag) == 0 && Convert.ToInt32(TBCount3.Tag) == 0 && Convert.ToInt32(TBCount2.Tag) == 0 && Convert.ToInt32(TBCount1.Tag) == 0)
			{
				//Дальше
				if (Convert.ToBoolean(Tag))
				{
					//Если тег true, то duo
					if (!Convert.ToBoolean(GArea.Tag))
					{
						//Перенос данных клеток
						foreach (Button check in buttons)
						{
							CellData data = (CellData)check.Tag;
							SaveCells.Add(new CellData(data.x, data.y, data.busy, data.length, data.number, data.health));
						}
						BClear_Click(sender, e);
						GArea.Tag = true;
						MessageBox.Show("Выбор игрока правой стороны", "SeaBattle", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
					}
					else
					{
						DuoW dw = new DuoW
						{
							LeftCells = SaveCells
						};
						//Перенос данных клеток
						foreach (Button check in buttons)
						{
							CellData data = (CellData)check.Tag;
							dw.RightCells.Add(data);
						}
						Close();
						dw.Show();
					}
				}
				else
				{
					//Если тег false, то solo
					//Перенос данных о клетках
					foreach (Button check in buttons)
					{
						CellData data = (CellData)check.Tag;
						SaveCells.Add(new CellData(data.x, data.y, data.busy, data.length, data.number, data.health));
					}
					BRandom_Click(sender, e); //Выбор для бота
					SoloW sw = new SoloW
					{
						LeftCells = SaveCells
					};
					//Перенос данных о клетках
					foreach (Button check in buttons)
					{
						CellData data = (CellData)check.Tag;
						sw.RightCells.Add(data);
					}
					Close();
					sw.Show();
				}
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
			MenuW mw = new MenuW();
			Close();
			mw.Show();
		}
	}
}