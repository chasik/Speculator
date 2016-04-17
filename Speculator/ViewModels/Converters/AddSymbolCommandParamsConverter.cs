using System;
using System.Globalization;
using System.Windows.Data;
using DevExpress.Xpf.Docking;

namespace Speculator.ViewModels.Converters
{
    public class AddSymbolCommandParamsConverter: IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values[0] as DocumentGroup;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
