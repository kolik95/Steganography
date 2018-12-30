using System.Windows;
using WindowsUI.RelayCommands;

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

		public Window AppWindow { get; set; }

		public RelayCommand TogglePageCommand { get; }

		public string ToggleText { get; set; }

		public MainWindowViewModel(Window window)
	    {

		    AppWindow = window;

			TogglePageCommand = new RelayCommand(ChangePage);

		    ToggleText = "Rozšifrovat";

	    }

		private void ChangePage()
		{
			ToggleText = ToggleText == "Zašifrovat" ? "Rozšifrovat" : "Zašifrovat";
		}
    }
}
