using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HauntedHunch
{
    /// <summary>
    /// Squre State To Solid Color Brush Value Converter
    /// </summary>
    public class SSTSCBVC : BaseMarkupExtension<SSTSCBVC>, IValueConverter
    {
        // Board palette
        static readonly Color standartTrapColor = Color.FromArgb(255, 255, 200, 200);
        static readonly Color standartWhiteColor = Color.FromArgb(255, 255, 255, 255);
        static readonly Color standartBlackColor = Color.FromArgb(255, 240, 240, 240);
        
        static readonly Color chosenPieceColor = Color.FromArgb(255, 175, 219, 189);
        static readonly Color moveColor = Color.FromArgb(255, 204, 255, 220);
        static readonly Color abilityUnoColor = Color.FromArgb(255, 100, 100, 100);
        static readonly Color abilityWithInteracterColor = Color.FromArgb(255, 255, 100, 100);

        /// <summary>
        /// Squares only hold State information. This method converts the square state to solid color brush for displayed square background.
        /// </summary>
        /// <param name="value">State of the squre, passed by the border that has the squre as its data context</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is SquareState))
                throw new ArgumentException();

            switch ((SquareState)value)
            {
                case SquareState.White: return new SolidColorBrush(standartWhiteColor);
                case SquareState.Black: return new SolidColorBrush(standartBlackColor);
                case SquareState.Trap: return new SolidColorBrush(standartTrapColor);
                case SquareState.ChosenPiece: return new SolidColorBrush(chosenPieceColor);
                case SquareState.Moveable: return new SolidColorBrush(moveColor);
                case SquareState.AbilityUnoable: return new SolidColorBrush(abilityUnoColor);
                case SquareState.AbilityWithInteracterable: return new SolidColorBrush(abilityWithInteracterColor);
                default: throw new ArgumentException();
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
