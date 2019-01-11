using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WindowsUI.Others;

namespace WindowsUI.ValueConverters
{
	public class PathToImageConverter : BaseValueConverter<PathToImageConverter>
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{

			if (!Helpers.IsImage((string) value) || value == null) return null;

			return new BitmapImage(new Uri(value as string, UriKind.Absolute));

		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
