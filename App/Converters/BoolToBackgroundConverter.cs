using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Omnos.Desktop.App.Converters
{
    public class BoolToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string selectedItem && parameter is string menuItem)
            {
                if (selectedItem == menuItem)
                {
                    // Retorna a cor de fundo para itens selecionados
                    return new SolidColorBrush(Color.FromRgb(42, 42, 60)); // #2A2A3C - SidebarHoverBackground
                }
            }
            
            // Retorna transparente para itens não selecionados
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
