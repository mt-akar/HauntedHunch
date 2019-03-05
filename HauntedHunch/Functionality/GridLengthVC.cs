using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace HauntedHunch
{
    /// <summary>
    /// <see cref="double"/> to <see cref="GridLength"/> value converter
    /// </summary>
    public class GridLengthVC : BaseMarkupExtension<GridLengthVC>, IValueConverter
    {
        /// <summary>
        /// Converts given <see cref="double"/> value to <see cref="GridLength"/> object
        /// </summary>
        /// <param name="value">State of the squre, passed by the border that has the squre as its data context</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Throw exception if the given value is not a double
            if (!(value is double))
                throw new ArgumentException();

            // Convert given double to GridLength
            return new GridLength((double)value);
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
