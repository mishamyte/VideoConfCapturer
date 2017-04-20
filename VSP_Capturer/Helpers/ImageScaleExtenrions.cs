using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.Scaling;

namespace VSP_Capturer.Helpers
{
	public static class ImageScaleExtenrions
	{
		public static KalikoImage Scale(this KalikoImage image)
		{
			return image.Scale(new FitScaling(640, (int) (640*image.ImageRatio)));
		}
	}
}
