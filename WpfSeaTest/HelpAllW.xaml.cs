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
	public partial class HelpAllW : Window
	{
		public HelpAllW()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				DBConnector.MainConnection.Open();

				MySqlDataReader reader = TakeData(1); //Одиночный
				while (reader.Read())
				{
					if (reader["total"].ToString() == "0")
					{
						G1.Visibility = Visibility.Hidden;
						break;
					}

					TBTotal1.Text += reader["total"].ToString();
					TBAllTime1.Text += $"{(Convert.ToInt32(reader["play_time"]) / 60)}:{Convert.ToDouble(reader["play_time"]) % 60}";
					TBTime1.Text += $"{Math.Truncate(Math.Truncate(Convert.ToDouble(reader["play_time"]) / Convert.ToDouble(reader["total"])) / 60)}:{Math.Round(Math.Round(Convert.ToDouble(reader["play_time"]) / Convert.ToDouble(reader["total"])) % 60)}";
					TBWinrate1.Text += Math.Round((Convert.ToDouble(reader["win"]) / ((Convert.ToDouble(reader["win"]) + Convert.ToDouble(reader["lose"])) / 100)), 2).ToString() + " %";
					TBMove1.Text += Math.Round((Convert.ToDouble(reader["first_move"]) / ((Convert.ToDouble(reader["first_move"]) + Convert.ToDouble(reader["second_move"])) / 100)), 2).ToString() + " %";
					TBHit1.Text += Math.Round((Convert.ToDouble(reader["my_hit"]) / ((Convert.ToDouble(reader["my_hit"]) + Convert.ToDouble(reader["my_miss"])) / 100)), 2).ToString() + " %";
					TBMiss1.Text += Math.Round((Convert.ToDouble(reader["enemy_miss"]) / ((Convert.ToDouble(reader["enemy_hit"]) + Convert.ToDouble(reader["enemy_miss"])) / 100)), 2).ToString() + " %";

					List<int> Destroy = new List<int>()
				{
					Convert.ToInt32(reader["destroy_4"]),
					Convert.ToInt32(reader["destroy_3"]),
					Convert.ToInt32(reader["destroy_2"]),
					Convert.ToInt32(reader["destroy_1"]),
				};
					List<int> Deal = new List<int>()
				{
					Convert.ToInt32(reader["deal_4"]),
					Convert.ToInt32(reader["deal_3"]),
					Convert.ToInt32(reader["deal_2"]),
					Convert.ToInt32(reader["deal_1"]),
				};
					List<int> Lost = new List<int>()
				{
					Convert.ToInt32(reader["lost_4"]),
					Convert.ToInt32(reader["lost_3"]),
					Convert.ToInt32(reader["lost_2"]),
					Convert.ToInt32(reader["lost_1"]),
				};
					List<int> Receive = new List<int>()
				{
					Convert.ToInt32(reader["receive_4"]),
					Convert.ToInt32(reader["receive_3"]),
					Convert.ToInt32(reader["receive_2"]),
					Convert.ToInt32(reader["receive_1"]),
				};

					TBDestroy1.Text += Destroy.Sum().ToString();
					TBDeal1.Text += Deal.Sum().ToString();
					TBLost1.Text += Lost.Sum().ToString();
					TBReceive1.Text += Receive.Sum().ToString();

					FillTable(GDestroy1, Destroy);
					FillTable(GDeal1, Deal);
					FillTable(GLost1, Lost);
					FillTable(GReceive1, Receive);
				}
				reader.Close();

				reader = TakeData(2); //Двойной
				while (reader.Read())
				{
					if (reader["total"].ToString() == "0")
					{
						G2.Visibility = Visibility.Hidden;
						break;
					}

					TBTotal2.Text += reader["total"].ToString();
					TBAllTime2.Text += $"{(Convert.ToInt32(reader["play_time"]) / 60)}:{Convert.ToDouble(reader["play_time"]) % 60}";
					TBTime2.Text += $"{Math.Truncate(Math.Truncate(Convert.ToDouble(reader["play_time"]) / Convert.ToDouble(reader["total"])) / 60)}:{Math.Round(Math.Round(Convert.ToDouble(reader["play_time"]) / Convert.ToDouble(reader["total"])) % 60)}";
					TBWinrate2.Text += Math.Round((Convert.ToDouble(reader["win"]) / ((Convert.ToDouble(reader["win"]) + Convert.ToDouble(reader["lose"])) / 100)), 2).ToString() + " %";
					TBMove2.Text += Math.Round((Convert.ToDouble(reader["first_move"]) / ((Convert.ToDouble(reader["first_move"]) + Convert.ToDouble(reader["second_move"])) / 100)), 2).ToString() + " %";
					TBHit2.Text += Math.Round((Convert.ToDouble(reader["my_hit"]) / ((Convert.ToDouble(reader["my_hit"]) + Convert.ToDouble(reader["my_miss"])) / 100)), 2).ToString() + " %";
					TBMiss2.Text += Math.Round((Convert.ToDouble(reader["enemy_miss"]) / ((Convert.ToDouble(reader["enemy_hit"]) + Convert.ToDouble(reader["enemy_miss"])) / 100)), 2).ToString() + " %";

					List<int> Destroy = new List<int>()
				{
					Convert.ToInt32(reader["destroy_4"]),
					Convert.ToInt32(reader["destroy_3"]),
					Convert.ToInt32(reader["destroy_2"]),
					Convert.ToInt32(reader["destroy_1"]),
				};
					List<int> Deal = new List<int>()
				{
					Convert.ToInt32(reader["deal_4"]),
					Convert.ToInt32(reader["deal_3"]),
					Convert.ToInt32(reader["deal_2"]),
					Convert.ToInt32(reader["deal_1"]),
				};
					List<int> Lost = new List<int>()
				{
					Convert.ToInt32(reader["lost_4"]),
					Convert.ToInt32(reader["lost_3"]),
					Convert.ToInt32(reader["lost_2"]),
					Convert.ToInt32(reader["lost_1"]),
				};
					List<int> Receive = new List<int>()
				{
					Convert.ToInt32(reader["receive_4"]),
					Convert.ToInt32(reader["receive_3"]),
					Convert.ToInt32(reader["receive_2"]),
					Convert.ToInt32(reader["receive_1"]),
				};

					TBDestroy2.Text += Destroy.Sum().ToString();
					TBDeal2.Text += Deal.Sum().ToString();
					TBLost2.Text += Lost.Sum().ToString();
					TBReceive2.Text += Receive.Sum().ToString();

					FillTable(GDestroy2, Destroy);
					FillTable(GDeal2, Deal);
					FillTable(GLost2, Lost);
					FillTable(GReceive2, Receive);
				}
				reader.Close();

				reader = TakeData(3); //Сетевой
				while (reader.Read())
				{
					if (reader["total"].ToString() == "0")
					{
						G3.Visibility = Visibility.Hidden;
						break;
					}

					TBTotal3.Text += reader["total"].ToString();
					TBAllTime3.Text += $"{(Convert.ToInt32(reader["play_time"]) / 60)}:{Convert.ToDouble(reader["play_time"]) % 60}";
					TBTime3.Text += $"{Math.Truncate(Math.Truncate(Convert.ToDouble(reader["play_time"]) / Convert.ToDouble(reader["total"])) / 60)}:{Math.Round(Math.Round(Convert.ToDouble(reader["play_time"]) / Convert.ToDouble(reader["total"])) % 60)}";
					TBWinrate3.Text += Math.Round((Convert.ToDouble(reader["win"]) / ((Convert.ToDouble(reader["win"]) + Convert.ToDouble(reader["lose"])) / 100)), 2).ToString() + " %";
					TBMove3.Text += Math.Round((Convert.ToDouble(reader["first_move"]) / ((Convert.ToDouble(reader["first_move"]) + Convert.ToDouble(reader["second_move"])) / 100)), 2).ToString() + " %";
					TBHit3.Text += Math.Round((Convert.ToDouble(reader["my_hit"]) / ((Convert.ToDouble(reader["my_hit"]) + Convert.ToDouble(reader["my_miss"])) / 100)), 2).ToString() + " %";
					TBMiss3.Text += Math.Round((Convert.ToDouble(reader["enemy_miss"]) / ((Convert.ToDouble(reader["enemy_hit"]) + Convert.ToDouble(reader["enemy_miss"])) / 100)), 2).ToString() + " %";

					List<int> Destroy = new List<int>()
				{
					Convert.ToInt32(reader["destroy_4"]),
					Convert.ToInt32(reader["destroy_3"]),
					Convert.ToInt32(reader["destroy_2"]),
					Convert.ToInt32(reader["destroy_1"]),
				};
					List<int> Deal = new List<int>()
				{
					Convert.ToInt32(reader["deal_4"]),
					Convert.ToInt32(reader["deal_3"]),
					Convert.ToInt32(reader["deal_2"]),
					Convert.ToInt32(reader["deal_1"]),
				};
					List<int> Lost = new List<int>()
				{
					Convert.ToInt32(reader["lost_4"]),
					Convert.ToInt32(reader["lost_3"]),
					Convert.ToInt32(reader["lost_2"]),
					Convert.ToInt32(reader["lost_1"]),
				};
					List<int> Receive = new List<int>()
				{
					Convert.ToInt32(reader["receive_4"]),
					Convert.ToInt32(reader["receive_3"]),
					Convert.ToInt32(reader["receive_2"]),
					Convert.ToInt32(reader["receive_1"]),
				};

					TBDestroy3.Text += Destroy.Sum().ToString();
					TBDeal3.Text += Deal.Sum().ToString();
					TBLost3.Text += Lost.Sum().ToString();
					TBReceive3.Text += Receive.Sum().ToString();

					FillTable(GDestroy3, Destroy);
					FillTable(GDeal3, Deal);
					FillTable(GLost3, Lost);
					FillTable(GReceive3, Receive);
				}
				reader.Close();

				DBConnector.MainConnection.Close();
			}
			catch (Exception)
			{
				MessageBox.Show("Не удалось посмотреть статистику игр. Возможно нет подключения к интернету. Проверьте подключение и запустите приложение заного", "SeaBattle", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
				Environment.Exit(0);
			}
		}

		private void FillTable(Grid grid, List<int> vs)
		{
			for (int i = 0; i < 4; i++)
			{
				TextBlock tb = new TextBlock
				{
					Text = vs[i].ToString(),
					FontSize = 13,
					VerticalAlignment = VerticalAlignment.Center,
					HorizontalAlignment = HorizontalAlignment.Center,
					TextTrimming = TextTrimming.CharacterEllipsis,
				};

				Grid.SetRow(tb, 2);
				Grid.SetColumn(tb, i);

				grid.Children.Add(tb);
			}
		}

		private MySqlDataReader TakeData(int game_type)
		{
			try
			{
				MySqlDataReader reader = new MySqlCommand($"SELECT " +
				$"(SELECT COUNT(victory) FROM `game` WHERE `victory` = 1 AND `user_id` = {DBConnector.IdUser} AND `type_id` = {game_type}) AS 'win', " +
				$"(SELECT COUNT(victory) FROM `game` WHERE `victory` = 0 AND `user_id` = {DBConnector.IdUser} AND `type_id` = {game_type}) AS 'lose', " +
				$"(SELECT COUNT(`s`.`first_move`) FROM `game` as g INNER JOIN `stat` AS s ON `g`.`stat_id` = `s`.`id` WHERE `s`.`first_move` = 1 AND `g`.`user_id` = {DBConnector.IdUser} AND `g`.`type_id` = {game_type}) AS 'first_move', " +
				$"(SELECT COUNT(`s`.`first_move`) FROM `game` as g INNER JOIN `stat` AS s ON `g`.`stat_id` = `s`.`id` WHERE `s`.`first_move` = 0 AND `g`.`user_id` = {DBConnector.IdUser} AND `g`.`type_id` = {game_type}) AS 'second_move', " +
				$"COUNT(*) AS 'total', " +
				$"SUM(TIMESTAMPDIFF(SECOND, `g`.`datetime_start`, `g`.`datetime_end`)) AS 'play_time', " +
				$"SUM(`s`.`my_hit`) AS 'my_hit', SUM(`s`.`my_miss`) AS 'my_miss', SUM(`s`.`enemy_hit`) AS 'enemy_hit', SUM(`s`.`enemy_miss`) AS 'enemy_miss', " +
				$"SUM(`ds`.`destroy_4`) AS 'destroy_4', SUM(`ds`.`destroy_3`) AS 'destroy_3', SUM(`ds`.`destroy_2`) AS 'destroy_2', SUM(`ds`.`destroy_1`) AS 'destroy_1', " +
				$"SUM(`dl`.`deal_4`) AS 'deal_4', SUM(`dl`.`deal_3`) AS 'deal_3', SUM(`dl`.`deal_2`) AS 'deal_2', SUM(`dl`.`deal_1`) AS 'deal_1', " +
				$"SUM(`l`.`lost_4`) AS 'lost_4', SUM(`l`.`lost_3`) AS 'lost_3', SUM(`l`.`lost_2`) AS 'lost_2', SUM(`l`.`lost_1`) AS 'lost_1', " +
				$"SUM(`r`.`receive_4`) AS 'receive_4', SUM(`r`.`receive_3`) AS 'receive_3', SUM(`r`.`receive_2`) AS 'receive_2', SUM(`r`.`receive_1`) AS 'receive_1' " +
				$"FROM `game` AS g " +
				$"INNER JOIN `stat` AS s ON `g`.`stat_id` = `s`.`id` " +
				$"INNER JOIN `destroy` as ds ON `s`.`destroy_id` = `ds`.`id` " +
				$"INNER JOIN `deal` as dl ON `s`.`destroy_id` = `dl`.`id` " +
				$"INNER JOIN `lost` as l ON `s`.`lost_id` = `l`.`id` " +
				$"INNER JOIN `receive` as r ON `s`.`receive_id` = `r`.`id` " +
				$"WHERE `g`.`user_id` = {DBConnector.IdUser} AND `g`.`type_id` = {game_type}", DBConnector.MainConnection).ExecuteReader();

				return reader;
			}
			catch (Exception)
			{
				MessageBox.Show("Не удалось посмотреть статистику игр. Возможно нет подключения к интернету. Проверьте подключение и запустите приложение заного", "SeaBattle", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
				Environment.Exit(0);
				return null;
			}
		}

		private void BBack_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}