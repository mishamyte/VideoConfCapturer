using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using Kaliko.ImageLibrary;

namespace VSP_Capturer.Helpers
{
	public static class BitmapImageExtensions
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

		public static BitmapImage ToBitmapImage(this KalikoImage image)
		{
			using (var memory = new MemoryStream())
			{
				image.SaveBmp(memory);
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
