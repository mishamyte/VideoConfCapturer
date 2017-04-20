using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using AForge.Video;
using AForge.Video.DirectShow;
using Kaliko.ImageLibrary;
using VSP_Capturer.Config;
using VSP_Capturer.Helpers;
using Image = System.Windows.Controls.Image;

namespace VSP_Capturer.Core
{
    public class Capturer
    {
	    private readonly Image _cameraImage;

	    private readonly Chromakey _chromakey;
	    private readonly Sender _sender;
	    private readonly ConfigManager _configManager;

	    private FilterInfoCollection _cameras;
	    private VideoCaptureDevice _device;

	    public Capturer(Image cameraImage, Sender sender, ConfigManager configManager)
	    {
		    _cameraImage = cameraImage;

		    _sender = sender;

		    _configManager = configManager;
			_chromakey = new Chromakey(_configManager.FilterSettings);
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
		    BitmapImage frameImage;
		    if (_configManager.FilterSettings.ApplyFilter)
		    {
			    var filteredImage = _chromakey.ApplyFilter(frame);
				frameImage = filteredImage.ToBitmapImage();
			    if (_configManager.SocketSettings.IsSendActive)
			    {
				    _sender.Send(filteredImage.Scale().ToBitmapImage().ToJpegByteArray());
			    }
		    }
		    else
		    {
			    frameImage = frame.ToBitmapImage();
				if (_configManager.SocketSettings.IsSendActive)
				{
					_sender.Send(new KalikoImage(frame).Scale().ToBitmapImage().ToJpegByteArray());
				}
			}
		
			frameImage.Freeze();

			Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
					new Action(() => _cameraImage.Source = frameImage));

	    }

    }
}
