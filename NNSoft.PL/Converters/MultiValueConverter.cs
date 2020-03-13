using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace NNSoft.PL.Converters
{
    /// <summary>
    /// https://www.codeproject.com/Articles/459958/Bindable-Converter-Converter-Parameter-and-StringF
    /// </summary>
    [ContentProperty("Converter")]
    class MultiValueConverterAdapter : IMultiValueConverter
    {
        public IValueConverter Converter { get; set; }

        private object lastParameter;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (Converter == null) 
                return values[0];
            if (values.Length > 1) 
                lastParameter = values[1];
            return Converter.Convert(values[0], targetType, lastParameter, culture);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (Converter == null) return new object[] { value };

            return new object[] { Converter.ConvertBack(value, targetTypes[0], lastParameter, culture) };
        }
    }
}
