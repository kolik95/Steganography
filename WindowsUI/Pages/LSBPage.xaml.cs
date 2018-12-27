using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WindowsUI.Pages;
using WindowsUI.ViewModels;

namespace WindowsUI
{
	/// <summary>
	/// Interaction logic for LSBPage.xaml
	/// </summary>
	public partial class LSBPage : BasePage<LSBViewModel>
	{
		public LSBPage()
		{
			InitializeComponent();
		}


		private void Grid_Drop(object sender, DragEventArgs e)
		{
			
			((LSBViewModel)this.DataContext).Drop(sender, e);

			ImageBorder.Background = new SolidColorBrush(Colors.Transparent);

			e.Handled = true;

		}

		private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{

			((LSBViewModel) this.DataContext).ChangeEncryptionType(sender, e);

			e.Handled = true;

		}
	}
}
