namespace HauntedHunch.Pieces
{
    public class Boomer : Piece
    {
        // Moves to adjacent squares. Can suicide, removing all adjacent pieces.

        private static readonly int[,] a = new int[4, 2] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };
        public static int[,] A { get { return a; } }

        public Boomer(int r, int c, bool p)
        {
            row = r;
            column = c;
            player = p;
            frozen = false;
        }

        override public void PossibleMoves(ref Square[,] table, int turnDup)
        {
            table[row, column].BackgroundColor.Color = MainWindow.possible_move_color;
            if (frozen) return;
            table[row, column].BackgroundColor.Color = MainWindow.AbilityUno_indicator_color; // Suicide

            for (int i = 0; i < 4; i++)
            {
                // In bounds & (empty square | psuedo piece)
                if (row + a[i, 0] <= 7 && row + a[i, 0] >= 1 && column + a[i, 1] <= 5 && column + a[i, 1] >= 1 && (table[row + a[i, 0], column + a[i, 1]].Piece == null ||
                    table[row + a[i, 0], column + a[i, 1]].Piece == table[row + a[i, 0], column + a[i, 1]].PsuedoPiece))
                {
                    table[row + a[i, 0], column + a[i, 1]].BackgroundColor.Color = MainWindow.possible_move_color;
                }
            }
        }

        public override void Move(ref Square[,] table, int to_row, int to_column, ref int turn)
        {
            turn++;

            table[to_row, to_column].Piece = table[row, column].Piece;
            table[to_row, to_column].Image = table[row, column].Image;
            table[row, column].Piece = null;
            table[row, column].Image = emptyImage;

            row = to_row;
            column = to_column;
        }

        // Suicide
        public override void AbilityUno(ref Square[,] table, ref int turn)
        {
            turn++;

            for (int i = 0; i < 4; i++)
            {
                // In bounds & not null
                if (row + a[i, 0] <= 7 && row + a[i, 0] >= 1 && column + a[i, 1] <= 5 && column + a[i, 1] >= 1 && table[row + a[i, 0], column + a[i, 1]].Piece != null)
                {
                    table[row + a[i, 0], column + a[i, 1]].PsuedoPiece = null;
                    table[row + a[i, 0], column + a[i, 1]].Image = emptyImage;
                    table[row + a[i, 0], column + a[i, 1]].Piece = null;
                }
            }
            table[row, column].PsuedoPiece = null;
            table[row, column].Image = emptyImage;
            table[row, column].Piece = null;
        }
    }
}
