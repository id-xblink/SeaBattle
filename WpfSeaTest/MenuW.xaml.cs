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
	public partial class MenuW : Window
	{
		public MenuW()
		{
			InitializeComponent();
		}

		private void BOnline_Click(object sender, RoutedEventArgs e)
		{
			OnlineW ow = new OnlineW();
			Close();
			ow.Show();
		}

		private void BSolo_Click(object sender, RoutedEventArgs e)
		{
			CreateAreaW caw = new CreateAreaW
			{
				Tag = false, //Чтобы играть против бота
			};
			Close();
			caw.Show();
		}

		private void BDuo_Click(object sender, RoutedEventArgs e)
		{
			CreateAreaW caw = new CreateAreaW
			{
				Tag = true, //Чтобы играть вдвоём
			};
			Close();
			caw.Show();
		}

		private void BExit_Click(object sender, RoutedEventArgs e)
		{
			Environment.Exit(0);
		}

		private void BProfile_Click(object sender, RoutedEventArgs e)
		{
			ProfileW pw = new ProfileW();
			Close();
			pw.Show();
		}

		private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			System.Diagnostics.Process.Start("https://tokarchuk.pro/");
		}
	}
}