using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfSeaTest
{
	public class ClientObject
	{
		protected internal string Id { get; private set; }
		protected internal NetworkStream Stream { get; private set; }
		string userName;
		TcpClient client;
		ServerObject server; // объект сервера

		public ClientObject(TcpClient tcpClient, ServerObject serverObject)
		{
			Id = Guid.NewGuid().ToString();
			client = tcpClient;
			server = serverObject;
			serverObject.AddConnection(this);
		}
		
		public void Process()
		{
			try
			{
				Stream = client.GetStream();
				// получаем имя пользователя
				string message = GetMessage();
				userName = message;

				//message = userName + " вошел в чат";
				message = userName + "Connected вошел в чат";
				// посылаем сообщение о входе в чат всем подключенным пользователям
				server.BroadcastMessage(message, Id);
				// в бесконечном цикле получаем сообщения от клиента
				do
				{
					try
					{
						message = GetMessage();
						string send;
						if (message != "")
						{
							send = string.Format(message);
						}
						else
						{
							send = string.Format("{0}: покинул чат", userName);
						}
						server.BroadcastMessage(send, Id);
					}
					catch
					{
						string send = string.Format("{0}: покинул чат", userName);
						server.BroadcastMessage(send, Id);
						break;
					}
				} while (message != "");
			}
			catch (Exception)
			{
				
			}
			finally
			{
				server.RemoveConnection(Id);
				Close();
			}
		}

		// чтение входящего сообщения и преобразование в строку
		private string GetMessage()
		{
			byte[] data = new byte[64]; // буфер для получаемых данных
			StringBuilder builder = new StringBuilder();
			int bytes = 0;
			do
			{
				bytes = Stream.Read(data, 0, data.Length);
				builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
			}
			while (Stream.DataAvailable);
			return builder.ToString();
		}

		// закрытие подключения
		protected internal void Close()
		{
			if (Stream != null)
				Stream.Close();
			if (client != null)
				client.Close();
		}
	}
}