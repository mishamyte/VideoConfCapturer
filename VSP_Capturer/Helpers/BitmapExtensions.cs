using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace VSP_Capturer.Helpers
{
	public static class BitmapExtensions
	{
		public static BitmapImage ToBitmapImage(this Bitmap bitmap)
		{
			using (var memory = new MemoryStream())
			{
				bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
				memory.Position = 0;

				var result = new BitmapImage();
				result.BeginInit();
				result.StreamSource = memory;
				result.CacheOption = BitmapCacheOption.OnLoad;
				result.EndInit();

				return result;
			}
		}
	}
}
