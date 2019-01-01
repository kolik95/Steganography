using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WindowsUI.Pages;
using WindowsUI.ViewModels;

namespace WindowsUI
{
	/// <summary>
	/// Interaction logic for LSBEncPage.xaml
	/// </summary>
	public partial class LSBEncPage : BasePage<LSBEncViewModel>
	{
		public LSBEncPage()
		{
			InitializeComponent();
		}


		private void Grid_Drop(object sender, DragEventArgs e)
		{
			
			((LSBEncViewModel)this.DataContext).Drop(sender, e);

			e.Handled = true;

		}
	}
}
