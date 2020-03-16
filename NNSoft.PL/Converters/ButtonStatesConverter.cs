using NNSoft.PL.Common;
using System;
using System.Globalization;
using System.Windows.Data;

namespace NNSoft.PL.Converters
{
    public class ButtonStatesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as ServiceInfo)?.State != (ServiceState)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
