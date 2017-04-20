namespace VSP_Capturer.Config
{
	public class ConfigManager
	{
		public FilterSettings FilterSettings { get; set; }

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
	}
}
