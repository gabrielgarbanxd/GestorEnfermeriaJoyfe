using System;
using System.Windows.Data;

namespace GestorEnfermeriaJoyfe.UI.Converters
{
    public class RadioButtonIndexToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int index = (int)value;
            int targetIndex = System.Convert.ToInt32(parameter);

            return index == targetIndex;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }
}
