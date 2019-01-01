using System.Windows;
using System.Windows.Controls;
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

		private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			
			((MainWindowViewModel)this.DataContext).ChangeEncryptionType(sender,e);

			e.Handled = true;

		}
	}
}
