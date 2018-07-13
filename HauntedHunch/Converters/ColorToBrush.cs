using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace HauntedHunch
{
    public class ColorToBrush : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new ColorToBrush();
        }

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
