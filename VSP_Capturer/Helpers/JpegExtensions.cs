using System.IO;
using System.Windows.Media.Imaging;

namespace VSP_Capturer.Helpers
{
	public static class JpegExtensions
	{
		public static byte[] ToJpegByteArray(this BitmapImage image)
		{
			var encoder = new JpegBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(image));
			using (var ms = new MemoryStream())
			{
				encoder.Save(ms);
				return ms.ToArray();
			}
		}
	}
}
