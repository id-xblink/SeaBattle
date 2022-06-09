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
	public partial class LeaderW : Window
	{
		public LeaderW()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				DBConnector.MainConnection.Open();

				MySqlDataReader reader = new MySqlCommand($"SELECT `u`.`id`, `u`.`login`, `l`.`elo` FROM `leaderboard` AS l INNER JOIN `user` AS u ON `l`.`user_id` = `u`.`id` ORDER BY `l`.`elo` DESC LIMIT 100", DBConnector.MainConnection).ExecuteReader();

				int i = 0;

				while (reader.Read())
				{
					GList.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });

					Grid grid = new Grid();

					switch (i)
					{
						case 0:
							{
								grid.Background = Brushes.Gold;
								break;
							}
						case 1:
							{
								grid.Background = Brushes.Silver;
								break;
							}
						case 2:
							{
								grid.Background = Brushes.RosyBrown; //Бронзовый цвет
								break;
							}
						default:
							{
								grid.Background = Brushes.LightBlue;
								break;
							}
					}

					if (Convert.ToInt32(reader["id"]) == DBConnector.IdUser)
						grid.Background = Brushes.MediumOrchid;

					grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(70) });
					grid.ColumnDefinitions.Add(new ColumnDefinition());
					grid.ColumnDefinitions.Add(new ColumnDefinition());

					TextBlock TBPos = new TextBlock
					{
						Text = (i + 1).ToString(),
						FontSize = 16,
						FontWeight = FontWeights.Bold,
						HorizontalAlignment = HorizontalAlignment.Center,
						VerticalAlignment = VerticalAlignment.Center,
						TextTrimming = TextTrimming.CharacterEllipsis,
						Margin = new Thickness(10, 0, 10, 0),
					};
					TextBlock TBLogin = new TextBlock
					{
						Text = reader["login"].ToString(),
						FontSize = 16,
						FontWeight = FontWeights.Bold,
						HorizontalAlignment = HorizontalAlignment.Center,
						VerticalAlignment = VerticalAlignment.Center,
						TextTrimming = TextTrimming.CharacterEllipsis,
						Margin = new Thickness(10, 0, 10, 0),
					};
					TextBlock TBElo = new TextBlock
					{
						Text = reader["elo"].ToString(),
						FontSize = 16,
						FontWeight = FontWeights.Bold,
						HorizontalAlignment = HorizontalAlignment.Center,
						VerticalAlignment = VerticalAlignment.Center,
						TextTrimming = TextTrimming.CharacterEllipsis,
						Margin = new Thickness(10, 0, 10, 0),
					};

					Grid.SetColumn(TBPos, 0);
					Grid.SetColumn(TBLogin, 1);
					Grid.SetColumn(TBElo, 2);

					grid.Children.Add(TBPos);
					grid.Children.Add(TBLogin);
					grid.Children.Add(TBElo);

					Grid.SetRow(grid, i);
					GList.Children.Add(grid);
					i++;
				}
				reader.Close();
				DBConnector.MainConnection.Close();
			}
			catch (Exception)
			{
				MessageBox.Show("Не удалось посмотреть таблицу лидеров. Возможно нет подключения к интернету. Проверьте подключение и запустите приложение заного", "SeaBattle", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
				Environment.Exit(0);
			}
		}

		private void BBack_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			System.Diagnostics.Process.Start("https://tokarchuk.pro/");
		}
	}
}