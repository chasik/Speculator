using System;
using System.Globalization;
using System.Windows.Data;
using SpeculatorModel.SmartCom;

namespace Speculator.ViewModels.Converters
{
    public class RowColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tick = value as SmartComBidAskValue;
            if (tick == null || tick.Volume == 0)
                return "#00000000";
            else if (tick.IsBid)
                return "#6595ED";
            else if (!tick.IsBid)
                return "#CD5C5C";
            else return "#00000000";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
