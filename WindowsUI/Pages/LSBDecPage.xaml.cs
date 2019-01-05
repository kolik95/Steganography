using System.Windows;
using WindowsUI.ViewModels;

namespace WindowsUI.Pages
{
	/// <summary>
	/// Interaction logic for LSBDecPage.xaml
	/// </summary>
	public partial class LSBDecPage : BasePage<LSBDecViewModel>
    {
        public LSBDecPage()
        {
            InitializeComponent();
        }

	    private void Grid_Drop(object sender, DragEventArgs e)
	    {

			((LSBDecViewModel)this.DataContext).Drop(ref sender, ref e);

		    e.Handled = true;

	    }
	}
}
