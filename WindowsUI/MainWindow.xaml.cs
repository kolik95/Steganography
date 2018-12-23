using System.Windows;
using WindowsUI.ViewModels;

namespace WindowsUI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{

			MainWindowViewModel.Build(this);

			InitializeComponent();

		}
	}
}
