using System.Windows;
using System.Windows.Controls;
using WindowsUI.Others;
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

			instance = new MainWindowViewModel(ref window);

		}

		#endregion

		public int CurrentPage => ToggleText=="Rozšifrovat" ? 0 : 1;

		public Window AppWindow { get; set; }

		public RelayCommand TogglePageCommand { get; }

		public string ToggleText { get; set; }

		public BaseEncryption Encrypter
		{

			get
			{

				if (EncrytionType == 0)
					return ASCIIEncrypter;
				return UTF8Encrypter;

			}

		}

		private LSB_ASCIIEncryption ASCIIEncrypter { get; }

		private LSB_UTF8Encryption UTF8Encrypter { get; }

		private EncTypes EncrytionType { get; set; }

		public MainWindowViewModel(ref Window window)
	    {

		    AppWindow = window;

			TogglePageCommand = new RelayCommand(ChangePage);

		    ToggleText = "Rozšifrovat";

		    EncrytionType = 0;

		    new Others.WindowResizer(AppWindow);

			ASCIIEncrypter = new LSB_ASCIIEncryption();
			UTF8Encrypter = new LSB_UTF8Encryption();

	    }

		private void ChangePage()
		{

			ToggleText = ToggleText == "Zašifrovat" ? "Rozšifrovat" : "Zašifrovat";

		}

		public void ChangeEncryptionType(ref object sender, ref SelectionChangedEventArgs e)
		{

			EncrytionType = (EncTypes)((ComboBox)sender).SelectedIndex;

		}
	}
}
