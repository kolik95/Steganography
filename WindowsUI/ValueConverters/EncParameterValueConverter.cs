using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsUI.Others;

namespace WindowsUI.ValueConverters
{
    public class EncParameterValueConverter : BaseMultiValueConverter<EncParameterValueConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new EncryptionParameters
            {
                Password = (string) values[1],
                Text = (string) values[0],
            };
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
