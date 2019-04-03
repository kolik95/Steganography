using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsUI.Pages;
using WindowsUI.ViewModels;

namespace WindowsUI.ValueConverters
{
    public class PageConverter: BaseValueConverter<PageConverter>
    {
	    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	    {

		    switch ((Pages.Pages)value)
		    {

				case Pages.Pages.LSBEncPage:
					return MainWindowViewModel.GetInstance.EncPage;

				case Pages.Pages.LSBDecPage:
					return MainWindowViewModel.GetInstance.DecPage;

                default:
					return null;

		    }

	    }

	    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	    {
		    throw new NotImplementedException();
	    }
    }
}
