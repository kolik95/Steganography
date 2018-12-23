using System.Windows.Controls;
using WindowsUI.ViewModels;

namespace WindowsUI.Pages
{
	public class BasePage<VM> : Page
		where VM : BaseViewModel,new()
    {

	    private VM _viewModel { get; set; }

	    public VM ViewModel
	    {
		    get => _viewModel;

		    set
		    {
			    if (_viewModel == value)
				    return;

			    _viewModel = value;

			    DataContext = _viewModel;
		    }
	    }

		public BasePage()
	    {

		    ViewModel = new VM();

	    }

	}
}
