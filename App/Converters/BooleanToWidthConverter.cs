using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Omnos.Desktop.App.Converters
{
    public class BooleanToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isExpanded && isExpanded)
            {
                return new GridLength(220); // Largura quando expandido
            }
            return new GridLength(70); // Largura quando recolhido
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}