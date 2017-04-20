using System.Drawing;
using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.Filters;

namespace VSP_Capturer.Core
{
	public class Chromakey
	{
		private ChromaKeyFilter _filter;

		public Chromakey()
		{
			CreateFilter();
		}

		public void CreateFilter()
		{
			_filter = new ChromaKeyFilter
			{
				KeyColor = Color.FromArgb(88, 195, 169),
				ToleranceHue = 40,
				ToleranceSaturnation = 0.7f,
				ToleranceBrightness = 0.5f
			};
		}

		public KalikoImage ApplyFilter(Image sourceFrame)
		{
			var frame = new KalikoImage(sourceFrame);
			var result = new KalikoImage(frame.Width, frame.Height, Color.Black);
			if (_filter != null) frame.ApplyFilter(_filter);
			result.BlitImage(frame);
			return result;
		}
	}
}
