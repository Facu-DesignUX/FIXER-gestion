using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FIXER.Desktop.Converters;

public class BoolToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue && boolValue)
        {
            // Stock crítico: Fondo suave de alerta (rojo/naranja cálido)
            return new SolidColorBrush(Color.FromRgb(254, 226, 226)); // Red 100
        }
        return Brushes.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class StockCriticoTextColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue && boolValue)
        {
            return new SolidColorBrush(Color.FromRgb(185, 28, 28)); // Red 700
        }
        return new SolidColorBrush(Color.FromRgb(15, 23, 42)); // Slate 900
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class EstadoColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool activo)
        {
            return activo 
                ? new SolidColorBrush(Color.FromRgb(22, 163, 74))  // Verde 600
                : new SolidColorBrush(Color.FromRgb(100, 116, 139)); // Slate 500
        }
        return Brushes.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

