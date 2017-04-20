using System;
using System.Drawing;
using System.Windows;
using System.Windows.Threading;
using AForge.Video;
using AForge.Video.DirectShow;
using VSP_Capturer.Config;
using VSP_Capturer.Helpers;
using Image = System.Windows.Controls.Image;

namespace VSP_Capturer.Core
{
    public class Capturer
    {
	    private readonly Image _cameraImage;

	    private readonly Chromakey _chromakey;
	    private readonly FilterSettings _filterSettings;

	    private FilterInfoCollection _cameras;
	    private VideoCaptureDevice _device;

	    public Capturer(Image cameraImage, FilterSettings filterSettings)
	    {
		    _cameraImage = cameraImage;

		    _filterSettings = filterSettings;
			_chromakey = new Chromakey(filterSettings);
	    }

	    public void Close()
	    {
		    if (_device != null && _device.IsRunning) _device.Stop();
	    }

	    public void CreateFilter()
	    {
		    _chromakey.CreateFilter();
	    }

	    public FilterInfoCollection GetCamerasList()
	    {
			_cameras = new FilterInfoCollection(FilterCategory.VideoInputDevice);
		    return _cameras;
	    }

	    public bool CameraIsRunning()
	    {
		    return !(_device == null || !_device.IsRunning);

	    }

	    public void RecordingButtonClicked(int index)
	    {
		    if (!CameraIsRunning())
		    {
				_device = new VideoCaptureDevice(_cameras[index].MonikerString);
			    _device.NewFrame += FrameHandler;
				_device.Start();
		    }
		    else
		    {
				_device.Stop();
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
					new Action(() => _cameraImage.Source = null));
			}
	    }

	    private void FrameHandler(object sender, NewFrameEventArgs eventArgs)
	    {
		    var frame = eventArgs.Frame;
		    var frameImage = 
				_filterSettings.ApplyFilter ? _chromakey.ApplyFilter(frame).ToBitmapImage() : frame.ToBitmapImage();

			frameImage.Freeze();

			Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
					new Action(() => _cameraImage.Source = frameImage));

	    }

    }
}
