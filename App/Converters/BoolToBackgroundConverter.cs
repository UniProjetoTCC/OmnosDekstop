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
            // Verificar se o valor é nulo ou DependencyProperty.UnsetValue
            if (value == null || value == System.Windows.DependencyProperty.UnsetValue)
            {
                return Brushes.Transparent;
            }

            // Verificar se o parâmetro é nulo ou DependencyProperty.UnsetValue
            if (parameter == null || parameter == System.Windows.DependencyProperty.UnsetValue)
            {
                return Brushes.Transparent;
            }

            // Converter os valores para string para comparação segura
            string selectedItem = value?.ToString() ?? string.Empty;
            string menuItem = parameter?.ToString() ?? string.Empty;

            // Verificar se o item selecionado corresponde ao item do menu
            if (selectedItem == menuItem)
            {
                // Retorna a cor de fundo para itens selecionados
                return new SolidColorBrush(Color.FromRgb(42, 42, 60)); // #2A2A3C - SidebarHoverBackground
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
