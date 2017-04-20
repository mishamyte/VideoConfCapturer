using VSP_Capturer.Config;
using WebSocketSharp;

namespace VSP_Capturer.Core
{
	public class Sender
	{
		private readonly WebSocket _socket;
		private readonly ConfigManager _configManager;

		public Sender(ConfigManager configManager)
		{
			// TODO: add text labels in GUI
			_configManager = configManager;
			_socket = new WebSocket(_configManager.SocketSettings.ConnectionString);
			_socket.OnOpen+= (sender, e) => _configManager.SocketSettings.IsSendActive = true;
			_socket.OnClose+= (sender, e) => _configManager.SocketSettings.IsSendActive = false;
		}

		public void Connect()
		{
			_socket.ConnectAsync();
		}

		public void Disconnect()
		{
			_socket.Close();
		}

		public void Send(byte[] data)
		{
			if (_socket.IsAlive)
			{
				_socket.SendAsync(data, null);
			}
		}
	}
}
