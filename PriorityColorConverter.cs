using System.Globalization;

public class PriorityColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.ToString() switch
        {
            "High" => Colors.Red,
            "Medium" => Colors.Orange,
            "Low" => Colors.Green,
            _ => Colors.Black
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
