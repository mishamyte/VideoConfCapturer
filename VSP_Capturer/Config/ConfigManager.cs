using System.Configuration;
using System.Globalization;

namespace VSP_Capturer.Config
{
	public class ConfigManager
	{
		private readonly Configuration _config;
		public FilterSettings FilterSettings { get; set; }
		public SocketSettings SocketSettings { get; set; }

		public ConfigManager()
		{
			_config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
		}
		
		public void LoadFilterSettings()
		{
			FilterSettings = new FilterSettings
			{
				ApplyFilter = false,
				Red = ParseIntParam("Red"),
				Green = ParseIntParam("Green"),
				Blue = ParseIntParam("Blue"),
				Hue = ParseFloatParam("Hue"),
				Saturation = ParseFloatParam("Saturation"),
				Brightness = ParseFloatParam("Brightness")
			};
		}

		public void LoadSocketSettings()
		{
			SocketSettings = new SocketSettings
			{
				ConnectionString = _config.AppSettings.Settings["ServerMainImageEndpoint"].Value
			};
		}

		public void SaveFilterSettings()
		{
			_config.AppSettings.Settings["Red"].Value = FilterSettings.Red.ToString();
			_config.AppSettings.Settings["Green"].Value = FilterSettings.Green.ToString();
			_config.AppSettings.Settings["Blue"].Value = FilterSettings.Blue.ToString();
			_config.AppSettings.Settings["Hue"].Value = FilterSettings.Hue.ToString("R");
			_config.AppSettings.Settings["Saturation"].Value = FilterSettings.Saturation.ToString("R");
			_config.AppSettings.Settings["Brightness"].Value = FilterSettings.Brightness.ToString("R");
			_config.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("appSettings");
		}

		public void InitDefaultFilterSettings()
		{
			FilterSettings = new FilterSettings
			{
				ApplyFilter = false,
				Red = 88,
				Green = 195,
				Blue = 169,
				Hue = 40,
				Saturation = 0.7f,
				Brightness = 0.5f
			};
		}

		public void InitDefaultSocketSettings()
		{
			SocketSettings = new SocketSettings
			{
				ConnectionString = "ws://localhost:8888/main"
			};
		}

		private int ParseIntParam(string paramName)
		{
			int param;
			var settings = _config.AppSettings.Settings;
			int.TryParse(_config.AppSettings.Settings[paramName].Value, out param);
			return param;
		}

		private float ParseFloatParam(string paramName)
		{
			float param;
			float.TryParse(_config.AppSettings.Settings[paramName].Value, out param);
			return param;
		}
	}
}
