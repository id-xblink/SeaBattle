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
using System.Security.Cryptography;

namespace WpfSeaTest
{
	public partial class LoginW : Window
	{
		public LoginW()
		{
			InitializeComponent();
		}

		private void BGo_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				DBConnector.MainConnection.Open();
				if (Convert.ToBoolean(TBlockSwap.Tag))
				{
					//Авторизация
					Authorization(TBLogin.Text, PBPass.Password);
				}
				else
				{
					//Регистрация
					TBLogin.Text = TBLogin.Text.Trim();
					PBPass.Password = PBPass.Password.Trim();
					if (TBLogin.Text != "" && PBPass.Password != "")
					{
						string query = $"SELECT `id`, `login`, `password` FROM `user` WHERE `login` = '{TBLogin.Text}'";
						int id = Convert.ToInt32(new MySqlCommand(query, DBConnector.MainConnection).ExecuteScalar());
						if (id == 0)
						{
							if (PBPass.Password == PBRepPass.Password)
							{
								var md5 = MD5.Create();
								var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(PBPass.Password));
								string hashPassword = Convert.ToBase64String(hash);

								string idUser = new MySqlCommand($"INSERT INTO `user` (`login`, `password`, `registration_datetime`) VALUES ('{TBLogin.Text}', '{hashPassword}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'); SELECT LAST_INSERT_ID();", DBConnector.MainConnection).ExecuteScalar().ToString();
								new MySqlCommand($"INSERT INTO `leaderboard` (`user_id`, `elo`) VALUES ({idUser}, {0})", DBConnector.MainConnection).ExecuteNonQuery();
								MessageBox.Show("Регистрация прошла успешно", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
								TBlockSwap_PreviewMouseLeftButtonUp(sender, null);
								PBRepPass.Password = "";
								PBPass.Password = "";
								TBLogin.Text = "";
							}
							else
								MessageBox.Show("Для регистрации пароли должны совпадать", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
						}
						else
							MessageBox.Show("Такой логин уже занят", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
					}
					else
						MessageBox.Show("Введите логин и пароль", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
				}
				DBConnector.MainConnection.Close();
			}
			catch (Exception)
			{
				if (Convert.ToBoolean(TBlockSwap.Tag)) //Авторизация
					MessageBox.Show("Не удалось авторизоваться. Возможно нет подключения к интернету. Проверьте подключение и запустите приложение заного", "SeaBattle", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
				else //Регистрация
					MessageBox.Show("Не удалось зарегистрироваться. Возможно нет подключения к интернету. Проверьте подключение и запустите приложение заного", "SeaBattle", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
				Environment.Exit(0);
			}
		}

		private void Authorization(string login, string password)
		{
			var md5 = MD5.Create();
			var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
			string hashPassword = Convert.ToBase64String(hash);
			string query = $"SELECT `id`, `login`, `password` FROM `user` WHERE `login` = '{login}' and `password` = '{hashPassword}'";
			int id = Convert.ToInt32(new MySqlCommand(query, DBConnector.MainConnection).ExecuteScalar());
			if (id != 0)
			{
				DBConnector.IdUser = id;
				MenuW mw = new MenuW();
				mw.Show();
				Close();
				return;
			}
			else
				MessageBox.Show("Введены неверные логин или пароль", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			TBLogin.Focus();
		}

		private void TBlockSwap_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (Convert.ToBoolean(TBlockSwap.Tag))
			{
				TBlockSwap.Content = "Регистрация";
				PBRepPass.Visibility = Visibility.Visible;
			}
			else
			{
				TBlockSwap.Content = "Авторизация";
				PBRepPass.Visibility = Visibility.Collapsed;
			}
			TBlockSwap.Tag = !Convert.ToBoolean(TBlockSwap.Tag);
			TBLogin.Focus();
		}

		private void Field_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (e.Text == "'" || e.Text == "\"" || e.Text == "\\")
				e.Handled = true;
		}
	}
}