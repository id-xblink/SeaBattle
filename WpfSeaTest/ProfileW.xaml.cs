using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
	public partial class ProfileW : Window
	{
		public ProfileW()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				DBConnector.MainConnection.Open();
				DataSet ds = new DataSet();
				MySqlDataAdapter sda = new MySqlDataAdapter(CreateQuery(), DBConnector.MainConnection);
				sda.Fill(ds);
				DGShow.ItemsSource = ds.Tables[0].DefaultView;
				DBConnector.MainConnection.Close();
			}
			catch (Exception)
			{
				MessageBox.Show("Не удалось посмотреть статистику игр. Возможно нет подключения к интернету. Проверьте подключение и запустите приложение заного", "SeaBattle", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
				Environment.Exit(0);
			}
		}

		private string CreateQuery()
		{
			string result = $"SELECT `g`.`id` AS 'ID', `g`.`datetime_start` AS 'Начало', `g`.`datetime_end` AS 'Конец', `t`.`name` AS 'Тип', `g`.`victory` AS 'Победа' FROM `game` AS g INNER JOIN `type` AS t ON `g`.`type_id` = `t`.`id` WHERE `g`.`user_id` = {DBConnector.IdUser} ";

			if (DPStart.Text == "" && DPEnd.Text == "")
				result += " ";
			if (DPStart.Text != "" && DPEnd.Text == "")
				result += $"AND `g`.`datetime_start` >= '{Convert.ToDateTime(DPStart.Text).ToString("yyyy-MM-dd")} 0:0:0' ";
			if (DPStart.Text == "" && DPEnd.Text != "")
				result += $"AND `g`.`datetime_end` <= '{Convert.ToDateTime(DPEnd.Text).ToString("yyyy-MM-dd")} 23:59:59' ";
			if (DPStart.Text != "" && DPEnd.Text != "")
				result += $"AND `g`.`datetime_start` >= '{Convert.ToDateTime(DPStart.Text).ToString("yyyy-MM-dd")} 0:0:0' AND `g`.`datetime_end` <= '{Convert.ToDateTime(DPEnd.Text).ToString("yyyy-MM-dd")} 23:59:59' ";


			if (Convert.ToBoolean(CBVictory.IsChecked) && Convert.ToBoolean(CBDefeat.IsChecked))
				result += " ";
			if (!Convert.ToBoolean(CBVictory.IsChecked) && Convert.ToBoolean(CBDefeat.IsChecked))
				result += "AND `g`.`victory` != 1 ";
			if (Convert.ToBoolean(CBVictory.IsChecked) && !Convert.ToBoolean(CBDefeat.IsChecked))
				result += "AND `g`.`victory` != 0 ";
			if (!Convert.ToBoolean(CBVictory.IsChecked) && !Convert.ToBoolean(CBDefeat.IsChecked))
				result += "AND `g`.`victory` != 0 AND `g`.`victory` != 1 ";


			if (Convert.ToBoolean(CBSolo.IsChecked) && Convert.ToBoolean(CBDuo.IsChecked) && Convert.ToBoolean(CBOnline.IsChecked))
				result += "";
			if (!Convert.ToBoolean(CBSolo.IsChecked) && Convert.ToBoolean(CBDuo.IsChecked) && Convert.ToBoolean(CBOnline.IsChecked))
				result += "AND `g`.`type_id` != 1";
			if (Convert.ToBoolean(CBSolo.IsChecked) && !Convert.ToBoolean(CBDuo.IsChecked) && Convert.ToBoolean(CBOnline.IsChecked))
				result += "AND `g`.`type_id` != 2";
			if (Convert.ToBoolean(CBSolo.IsChecked) && Convert.ToBoolean(CBDuo.IsChecked) && !Convert.ToBoolean(CBOnline.IsChecked))
				result += "AND `g`.`type_id` != 3";
			if (!Convert.ToBoolean(CBSolo.IsChecked) && !Convert.ToBoolean(CBDuo.IsChecked) && Convert.ToBoolean(CBOnline.IsChecked))
				result += "AND `g`.`type_id` = 3";
			if (Convert.ToBoolean(CBSolo.IsChecked) && !Convert.ToBoolean(CBDuo.IsChecked) && !Convert.ToBoolean(CBOnline.IsChecked))
				result += "AND `g`.`type_id` = 1";
			if (!Convert.ToBoolean(CBSolo.IsChecked) && Convert.ToBoolean(CBDuo.IsChecked) && !Convert.ToBoolean(CBOnline.IsChecked))
				result += "AND `g`.`type_id` = 2";
			if (!Convert.ToBoolean(CBSolo.IsChecked) && !Convert.ToBoolean(CBDuo.IsChecked) && !Convert.ToBoolean(CBOnline.IsChecked))
				result += "AND `g`.`type_id` != 1 AND `g`.`type_id` != 2 AND `g`.`type_id` != 3";
			
			return result;
		}

		private void BLeader_Click(object sender, RoutedEventArgs e)
		{
			LeaderW lw = new LeaderW
			{
				Owner = this,
			};
			IsEnabled = false;
			lw.ShowDialog();
			IsEnabled = true;
		}

		private void BExit_Click(object sender, RoutedEventArgs e)
		{
			MenuW mw = new MenuW();
			Close();
			mw.Show();
		}

		private void BAll_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show("Получение полной статистики может занять некоторое время. Продолжить?", "SeaBattle", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (result == MessageBoxResult.Yes)
			{
				//Сбор всей информация
				IsEnabled = false;
				HelpAllW haw = new HelpAllW
				{
					Owner = this,
				};
				haw.ShowDialog();
				IsEnabled = true;
			}
		}

		private void BOne_Click(object sender, RoutedEventArgs e)
		{
			//Сбор информации об одной игре
			if (DGShow.SelectedItem != null)
			{
				DataGridCellInfo cellInfo = DGShow.SelectedCells[0];
				FrameworkElement content = cellInfo.Column.GetCellContent(cellInfo.Item);
				TextBlock tbc = content as TextBlock;
				int id = Convert.ToInt32(tbc.Text); //Индентификатор игры
				IsEnabled = false;
				HelpOneW how = new HelpOneW(id)
				{
					Owner = this,
				};
				how.ShowDialog();
				IsEnabled = true;
			}
		}

		private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			Window_Loaded(sender, e);
		}

		private void CheckBox_Click(object sender, RoutedEventArgs e)
		{
			Window_Loaded(sender, e);
		}
	}
}