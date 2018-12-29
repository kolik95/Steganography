using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsUI.ValueConverters
{
    public class PageConverter: BaseValueConverter<PageConverter>
    {
	    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	    {

		    switch ((Pages.Pages)value)
		    {

				case Pages.Pages.LSBPage:
					return new LSBEncPage();


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
