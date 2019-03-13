using System.Windows;
using WindowsUI.Others;
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

			((LSBEncViewModel)this.DataContext).ImagePath = Helpers.Drop(ref sender, ref e);

			e.Handled = true;

		}
	}
}
