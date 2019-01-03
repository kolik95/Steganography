using System.Windows;
using System.Windows.Controls;
using WindowsUI.RelayCommands;
using Steganografie.Encryption;

namespace WindowsUI.ViewModels
{
	class MainWindowViewModel : BaseViewModel
	{

		#region Singleton

		private static MainWindowViewModel instance { get; set; }

		public static MainWindowViewModel GetInstance => instance;

		public static void Build(Window window)
		{

			instance = new MainWindowViewModel(window);

		}

		#endregion

		public int CurrentPage => ToggleText=="Rozšifrovat" ? 0 : 1;

		public EncTypes EncrytionType { get; set; }

		public Window AppWindow { get; set; }

		public RelayCommand TogglePageCommand { get; }

		public string ToggleText { get; set; }

		public MainWindowViewModel(Window window)
	    {

		    AppWindow = window;

			TogglePageCommand = new RelayCommand(ChangePage);

		    ToggleText = "Rozšifrovat";

		    EncrytionType = 0;

		    new Others.WindowResizer(AppWindow);

	    }

		private void ChangePage()
		{

			ToggleText = ToggleText == "Zašifrovat" ? "Rozšifrovat" : "Zašifrovat";

		}

		public void ChangeEncryptionType(object sender, SelectionChangedEventArgs e)
		{

			EncrytionType = (EncTypes)((ComboBox)sender).SelectedIndex;

		}
	}
}
