using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using AForge.Video.DirectShow;
using VSP_Capturer.Config;
using VSP_Capturer.Core;

namespace VSP_Capturer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private readonly Capturer _capturer;
		private readonly Sender _sender;
		private readonly ConfigManager _configManager;

		public MainWindow()
		{
			_configManager = new ConfigManager();

			// Now _filterSettings resets when components are inited (cause change evens are triggered)
			InitializeComponent();

			_configManager.LoadFilterSettings();
			_configManager.LoadSocketSettings();

			_sender = new Sender(_configManager);
			_capturer = new Capturer(CameraImage, _sender, _configManager);

			FillCamerasList();
			InitValues();
		}

		public void CloseAll(object sender, CancelEventArgs e)
		{
			_capturer.Close();
			_sender.Disconnect();
		}

		private void InitValues()
		{
			RedSpinner.Value = _configManager.FilterSettings.Red;
			GreenSpinner.Value = _configManager.FilterSettings.Green;
			BlueSpinner.Value = _configManager.FilterSettings.Blue;
			HueSpinner.Value = _configManager.FilterSettings.Hue;
			SaturationSpinner.Value = _configManager.FilterSettings.Saturation;
			BrightnessSpinner.Value = _configManager.FilterSettings.Brightness;
		}

		private void FillCamerasList()
		{
			CamerasList.Items.Clear();
			var cameras = _capturer.GetCamerasList();
			foreach(FilterInfo camera in cameras)
			{
				CamerasList.Items.Add(camera.Name);
			}
			CamerasList.SelectedIndex = 0;
		}

		private void RecordingButton_Click(object sender, RoutedEventArgs e)
		{
			if (!_capturer.CameraIsRunning())
			{
				RecordingButton.Content = "Stop";
			}
			else
			{
				FillCamerasList();
				RecordingButton.Content = "Start";
			}
			_capturer.RecordingButtonClicked(CamerasList.SelectedIndex);
		}

		private void ChromakeyCheckbox_StateChanged(object sender, RoutedEventArgs e)
		{
			if (ChromakeyCheckbox.IsChecked != null) _configManager.FilterSettings.ApplyFilter = (bool) ChromakeyCheckbox.IsChecked;
		}

		private void RedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			_configManager.FilterSettings.Red = (int) RedSlider.Value;
			RedSpinner.Value = _configManager.FilterSettings.Red;
			_capturer?.CreateFilter();
			ChangeCanvasColor();
			_configManager.SaveFilterSettings();
		}

		private void RedSpinner_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (_configManager.FilterSettings != null)
			{
				if (RedSpinner.Value != null) _configManager.FilterSettings.Red = (int) RedSpinner.Value;
				RedSlider.Value = _configManager.FilterSettings.Red;
				_capturer?.CreateFilter();
				ChangeCanvasColor();
				_configManager.SaveFilterSettings();
			}
		}

		private void GreenSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			_configManager.FilterSettings.Green = (int)GreenSlider.Value;
			GreenSpinner.Value = _configManager.FilterSettings.Green;
			_capturer?.CreateFilter();
			ChangeCanvasColor();
			_configManager.SaveFilterSettings();
		}

		private void GreenSpinner_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (_configManager.FilterSettings != null)
			{
				if (GreenSpinner.Value != null) _configManager.FilterSettings.Green = (int)GreenSpinner.Value;
				GreenSlider.Value = _configManager.FilterSettings.Green;
				_capturer?.CreateFilter();
				ChangeCanvasColor();
				_configManager.SaveFilterSettings();
			}
		}

		private void BlueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			_configManager.FilterSettings.Blue = (int)BlueSlider.Value;
			BlueSpinner.Value = _configManager.FilterSettings.Blue;
			_capturer?.CreateFilter();
			ChangeCanvasColor();
			_configManager.SaveFilterSettings();
		}

		private void BlueSpinner_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (_configManager.FilterSettings != null)
			{
				if (BlueSpinner.Value != null) _configManager.FilterSettings.Blue = (int) BlueSpinner.Value;
				BlueSlider.Value = _configManager.FilterSettings.Blue;
				_capturer?.CreateFilter();
				ChangeCanvasColor();
				_configManager.SaveFilterSettings();
			}
		}

		private void ChangeCanvasColor()
		{
			var color = Color.FromRgb((byte)_configManager.FilterSettings.Red, (byte)_configManager.FilterSettings.Green, (byte)_configManager.FilterSettings.Blue);
			ColorCanvas.Background = new SolidColorBrush(color);
		}

		private void HueSpinner_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (_configManager.FilterSettings != null)
			{
				if (HueSpinner.Value != null) _configManager.FilterSettings.Hue = (float) HueSpinner.Value;
				_capturer?.CreateFilter();
				_configManager.SaveFilterSettings();
			}
		}


		private void SaturationSpinner_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (_configManager.FilterSettings != null)
			{
				if (SaturationSpinner.Value != null) _configManager.FilterSettings.Saturation = (float) SaturationSpinner.Value;
				_capturer?.CreateFilter();
				_configManager.SaveFilterSettings();
			}
		}

		private void BrightnessSpinner_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (_configManager.FilterSettings != null)
			{
				if (BrightnessSpinner.Value != null) _configManager.FilterSettings.Brightness = (float) BrightnessSpinner.Value;
				_capturer?.CreateFilter();
				_configManager.SaveFilterSettings();
			}
		}

		private void ConnectButton_Click(object sender, RoutedEventArgs e)
		{
			if (_configManager.SocketSettings.IsSendActive)
			{
				ConnectButton.Content = "Connect";
				_sender.Disconnect();
			}
			else
			{
				ConnectButton.Content = "Disconnect";
				_sender.Connect();
			}
		}
	}
}
