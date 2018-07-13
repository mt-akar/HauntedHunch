using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HauntedHunch
{
    public class ColorToBrush : BaseMarkupExtension<ColorToBrush>, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!(value is Color))
                throw new ArgumentException();

            return new SolidColorBrush((Color)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
