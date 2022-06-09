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
	public partial class HelpOneW : Window
	{
		int idGame = 0;

		public HelpOneW(int idGame)
		{
			InitializeComponent();
			this.idGame = idGame;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				DBConnector.MainConnection.Open();

				MySqlDataReader reader = new MySqlCommand($"SELECT `g`.`id` AS 'ID', `g`.`datetime_start` AS 'Start', `g`.`datetime_end` AS 'End', `t`.`name` AS 'Тип', `g`.`stat_id` AS 'Статистика', `g`.`victory` AS 'Победа' FROM `game` AS g INNER JOIN `type` AS t ON `g`.`type_id` = `t`.`id` WHERE `g`.`id` = {idGame}", DBConnector.MainConnection).ExecuteReader();

				string idStat = "";

				while (reader.Read())
				{
					TBID.Text += reader["ID"];
					TimeSpan ts = Convert.ToDateTime(reader["End"]) - Convert.ToDateTime(reader["Start"]);
					TBTime.Text += $"{ts.Minutes.ToString()}:{ts.Seconds.ToString()} (м./с.)"; //Минуты + Секунды
					TBType.Text += reader["Тип"].ToString();
					if (Convert.ToBoolean(reader["Победа"].ToString()))
					{
						TBResult.Text = "Победа";
					}
					else
					{
						TBResult.Text = "Поражение";
					}

					idStat = reader["Статистика"].ToString();
				}
				reader.Close();

				reader = new MySqlCommand($"SELECT * FROM `stat` WHERE `id` = {idStat}", DBConnector.MainConnection).ExecuteReader();

				string idDestroy = "";
				string idDeal = "";
				string idLost = "";
				string idReceive = "";

				while (reader.Read())
				{
					if (Convert.ToBoolean(reader["first_move"].ToString()))
					{
						TBMove.Text = "Лидирующий ход";
					}
					else
					{
						TBMove.Text = "Не лидирующий ход";
					}

					TBHit.Text = Math.Round((Convert.ToDouble(reader["my_hit"]) / ((Convert.ToDouble(reader["my_hit"]) + Convert.ToDouble(reader["my_miss"])) / 100)), 2).ToString() + " %";
					TBMiss.Text = Math.Round((Convert.ToDouble(reader["enemy_miss"]) / ((Convert.ToDouble(reader["enemy_hit"]) + Convert.ToDouble(reader["enemy_miss"])) / 100)), 2).ToString() + " %";
					idDestroy = reader["destroy_id"].ToString();
					idDeal = reader["deal_id"].ToString();
					idLost = reader["lost_id"].ToString();
					idReceive = reader["receive_id"].ToString();
				}
				reader.Close();

				List<int> Destroy = Stat($"SELECT * FROM `destroy` WHERE `id` = {idDestroy}");
				List<int> Deal = Stat($"SELECT * FROM `deal` WHERE `id` = {idDeal}");
				List<int> Lost = Stat($"SELECT * FROM `lost` WHERE `id` = {idLost}");
				List<int> Receive = Stat($"SELECT * FROM `receive` WHERE `id` = {idReceive}");

				TBTotalHit.Text = $"{Destroy.Sum().ToString()}\\{Deal.Sum().ToString()}";
				TBTotalMiss.Text = $"{Lost.Sum().ToString()}\\{Receive.Sum().ToString()}";

				FillTable(GAttack, Destroy, 1);
				FillTable(GAttack, Deal, 2);

				FillTable(GDefence, Lost, 1);
				FillTable(GDefence, Receive, 2);

				DBConnector.MainConnection.Close();
			}
			catch (Exception)
			{
				MessageBox.Show("Не удалось посмотреть статистику игры. Возможно нет подключения к интернету. Проверьте подключение и запустите приложение заного", "SeaBattle", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
				Environment.Exit(0);
			}
			
		}

		private void FillTable(Grid grid, List<int> vs, int pos)
		{
			for (int i = 0; i < 4; i++)
			{
				TextBlock tb = new TextBlock
				{
					Text = vs[i].ToString(),
					FontSize = 14,
					VerticalAlignment = VerticalAlignment.Center,
					HorizontalAlignment = HorizontalAlignment.Center,
				};

				Grid.SetRow(tb, pos);
				Grid.SetColumn(tb, i + 1);

				grid.Children.Add(tb);
			}
		}

		private List<int> Stat(string query)
		{
			try
			{
				List<int> vs = new List<int>();

				MySqlDataReader reader = new MySqlCommand(query, DBConnector.MainConnection).ExecuteReader();

				while (reader.Read())
				{
					//Корабли
					vs.Add(Convert.ToInt32(reader[1])); //4
					vs.Add(Convert.ToInt32(reader[2])); //3
					vs.Add(Convert.ToInt32(reader[3])); //2
					vs.Add(Convert.ToInt32(reader[4])); //1
				}
				reader.Close();

				return vs;
			}
			catch (Exception)
			{
				MessageBox.Show("Не удалось посмотреть статистику игры. Возможно нет подключения к интернету. Проверьте подключение и запустите приложение заного", "SeaBattle", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
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