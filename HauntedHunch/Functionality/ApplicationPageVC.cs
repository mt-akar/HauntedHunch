using System;
using System.Globalization;
using System.Windows.Data;

namespace HauntedHunch
{
    public class ApplicationPageVC : BaseMarkupExtension<ApplicationPageVC>, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ApplicationPageEnum))
                throw new ArgumentException();

            switch ((ApplicationPageEnum)value)
            {
                case ApplicationPageEnum.MenuPage:
                    return new MenuPage();
                case ApplicationPageEnum.GamePage:
                    return new GamePage();
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Convert back is unnecessary
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
