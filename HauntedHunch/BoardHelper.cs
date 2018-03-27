using System.Windows.Media;

namespace HauntedHunch
{
    public static class BoardHelper
    {
        private static Color trapSquareColor = Color.FromArgb(255, 255, 200, 200);
        private static Color standartWhite = Color.FromArgb(255, 255, 255, 255);
        private static Color standartBlack = Color.FromArgb(255, 240, 240, 240);
        
        public static Color standartMoveColor = Color.FromArgb(255, 204, 255, 220);
        public static Color abilityUnoColor = Color.FromArgb(255, 100, 100, 100);
        public static Color abilityWithInteracterColor = Color.FromArgb(255, 255, 100, 100);
        
        public static Color DefaultColor(int row, int column)
        {
            // 4 trap squares are pink.
            if ((row == 3 || row == 5) && (column == 2 || column == 5))
                return trapSquareColor;

            // Rest of the board is white/black
            else
                return ((row + column) % 2 == 0) ? standartWhite : standartBlack;
        }
    }
}
