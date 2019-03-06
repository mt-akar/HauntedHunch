using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HauntedHunch
{
    public class PieceColorToSCBVC : BaseMarkupExtension<PieceColorToSCBVC>, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If there is no piece on the square, piece color is transparent
            if (!(value is Square.PieceColor))
                throw new ArgumentException();

            switch ((Square.PieceColor)value)
            {
                case Square.PieceColor.Nothing:
                    return new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                case Square.PieceColor.WhiteUnrevealed:
                    return new SolidColorBrush(Color.FromRgb(0, 22, 165));
                case Square.PieceColor.WhiteRevealed:
                    return new SolidColorBrush(Color.FromRgb(102, 130, 255));
                case Square.PieceColor.BlackUnrevealed:
                    return new SolidColorBrush(Color.FromRgb(181, 126, 0));
                case Square.PieceColor.BlackRevealed:
                    return new SolidColorBrush(Color.FromRgb(255, 215, 122));
                default:
                    throw new Exception($"This cannot happen. Logic error in {nameof(PieceColorToSCBVC)}.cs");
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
