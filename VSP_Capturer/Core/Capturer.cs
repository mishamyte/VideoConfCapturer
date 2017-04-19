using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using AForge.Video;
using AForge.Video.DirectShow;
using VSP_Capturer.Helpers;
using Image = System.Windows.Controls.Image;

namespace VSP_Capturer.Core
{
    public class Capturer
    {
	    private readonly ComboBox _camerasList;
	    private readonly Button _recordingButton;
	    private readonly Image _cameraImage;

	    private FilterInfoCollection _cameras;
	    private VideoCaptureDevice _device;

	    public Capturer(Image cameraImage, ComboBox camerasList, Button recordingButton)
	    {
		    _cameraImage = cameraImage;
		    _camerasList = camerasList;
		    _recordingButton = recordingButton;

		    _recordingButton.Click += RecordingButtonClicked;

			FillCamerasList();
	    }

	    public void Close()
	    {
		    _device.Stop();
	    }

	    private void FillCamerasList()
	    {
		    _cameras = new FilterInfoCollection(FilterCategory.VideoInputDevice);
		    foreach (FilterInfo camera in _cameras)
		    {
			    _camerasList.Items.Add(camera.Name);
		    }
		    _camerasList.SelectedIndex = 0;
	    }

	    private void RecordingButtonClicked(object sender, EventArgs e)
	    {
		    if (_device == null || !_device.IsRunning)
		    {
				_device = new VideoCaptureDevice(_cameras[_camerasList.SelectedIndex].MonikerString);
			    _device.NewFrame += FrameHandler;
			    _recordingButton.Content = "Stop";
				_device.Start();
		    }
		    else
		    {
			    _recordingButton.Content = "Start";
				_device.Stop();
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
					new Action(() => _cameraImage.Source = null));
			}
	    }

	    private void FrameHandler(object sender, NewFrameEventArgs eventArgs)
	    {
		    var frame = (Bitmap) eventArgs.Frame.Clone();
		    var frameImage = frame.ToBitmapImage();

		    frameImage.Freeze();

			Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
					new Action(() => _cameraImage.Source = frameImage));

	    }

    }
}
