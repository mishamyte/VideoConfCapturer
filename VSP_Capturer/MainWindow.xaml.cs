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
		private readonly ConfigManager _configManager;
		private readonly FilterSettings _filterSettings;

		public MainWindow()
		{
			_configManager = new ConfigManager();
			_configManager.InitDefaultFilterSettings();
			_filterSettings = _configManager.FilterSettings;

			// Now _filterSettings resets when components are inited (cause change evens are triggered)
			InitializeComponent();

			_capturer = new Capturer(CameraImage, _filterSettings);

			FillCamerasList();
			InitValues();
		}

		public void CloseAll(object sender, CancelEventArgs e)
		{
			_capturer.Close();
		}

		private void InitValues()
		{
			RedSpinner.Value = _filterSettings.Red;
			GreenSpinner.Value = _filterSettings.Green;
			BlueSpinner.Value = _filterSettings.Blue;
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
			if (ChromakeyCheckbox.IsChecked != null) _filterSettings.ApplyFilter = (bool) ChromakeyCheckbox.IsChecked;
		}

		private void RedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			_filterSettings.Red = (int) RedSlider.Value;
			RedSpinner.Value = _filterSettings.Red;
			_capturer?.CreateFilter();
			ChangeCanvasColor();
		}

		private void RedSpinner_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (_filterSettings != null)
			{
				if (RedSpinner.Value != null) _filterSettings.Red = (int) RedSpinner.Value;
				RedSlider.Value = _filterSettings.Red;
				_capturer?.CreateFilter();
				ChangeCanvasColor();
			}
		}

		private void GreenSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			_filterSettings.Green = (int)GreenSlider.Value;
			GreenSpinner.Value = _filterSettings.Green;
			_capturer?.CreateFilter();
			ChangeCanvasColor();
		}

		private void GreenSpinner_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (_filterSettings != null)
			{
				if (GreenSpinner.Value != null) _filterSettings.Green = (int)GreenSpinner.Value;
				GreenSlider.Value = _filterSettings.Green;
				_capturer?.CreateFilter();
				ChangeCanvasColor();
			}
		}

		private void BlueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			_filterSettings.Blue = (int)BlueSlider.Value;
			BlueSpinner.Value = _filterSettings.Blue;
			_capturer?.CreateFilter();
			ChangeCanvasColor();
		}

		private void BlueSpinner_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (_filterSettings != null)
			{
				if (BlueSpinner.Value != null) _filterSettings.Blue = (int) BlueSpinner.Value;
				BlueSlider.Value = _filterSettings.Blue;
				_capturer?.CreateFilter();
				ChangeCanvasColor();
			}
		}

		private void ChangeCanvasColor()
		{
			var color = Color.FromRgb((byte) _filterSettings.Red, (byte) _filterSettings.Green, (byte) _filterSettings.Blue);
			ColorCanvas.Background = new SolidColorBrush(color);
		}

		private void HueSpinner_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (_filterSettings != null)
			{
				if (HueSpinner.Value != null) _filterSettings.Hue = (float) HueSpinner.Value;
				_capturer?.CreateFilter();
			}
		}


		private void SaturationSpinner_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (_filterSettings != null)
			{
				if (SaturationSpinner.Value != null) _filterSettings.Saturation = (float) SaturationSpinner.Value;
				_capturer?.CreateFilter();
			}
		}

		private void BrightnessSpinner_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (_filterSettings != null)
			{
				if (BrightnessSpinner.Value != null) _filterSettings.Brightness = (float) BrightnessSpinner.Value;
				_capturer?.CreateFilter();
			}
		}
	}
}
