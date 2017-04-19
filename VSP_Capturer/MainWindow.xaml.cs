using System.ComponentModel;
using System.Windows;
using VSP_Capturer.Core;

namespace VSP_Capturer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Capturer _capturer;

		public MainWindow()
		{
			InitializeComponent();

			_capturer = new Capturer(CameraImage, CamerasList, RecordingButton);
		}

		public void CloseAll(object sender, CancelEventArgs e)
		{
			_capturer.Close();
		}
	}
}
