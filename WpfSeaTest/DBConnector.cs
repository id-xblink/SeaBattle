using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSeaTest
{
	public static class DBConnector
	{
		public static MySqlConnection MainConnection = GetDBConnection();

		public static int IdUser = 0;

		public static MySqlConnection GetDBConnection()
		{
			string host = "188.120.245.106";
			int port = 3306;
			string database = "Note9_SeaBattle";
			string username = "Note9";
			string password = "NZC9RMrYpdDqPgXI";

			string connString = $"Server = {host}; Database = {database}; port = {port}; User Id = {username}; password = {password}";

			return new MySqlConnection(connString);
		}
	}
}