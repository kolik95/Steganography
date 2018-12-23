using System.Windows;

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

		public int CurrentPage { get; set; } = 0;

		public Window AppWindow { get; set; }

	    public MainWindowViewModel(Window window)
	    {

		    AppWindow = window;

	    }
    }
}
