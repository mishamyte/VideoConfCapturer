using System.Drawing;
using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.Filters;
using VSP_Capturer.Config;

namespace VSP_Capturer.Core
{
	public class Chromakey
	{
		private ChromaKeyFilter _filter;
		private readonly FilterSettings _filterSettings;

		public Chromakey(FilterSettings filterSettings)
		{
			_filterSettings = filterSettings;

			CreateFilter();
		}

		public void CreateFilter()
		{
			_filter = new ChromaKeyFilter
			{
				KeyColor = Color.FromArgb(_filterSettings.Red, _filterSettings.Green, _filterSettings.Blue),
				ToleranceHue = _filterSettings.Hue,
				ToleranceSaturnation = _filterSettings.Saturation,
				ToleranceBrightness = _filterSettings.Brightness
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
