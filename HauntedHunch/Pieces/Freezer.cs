namespace HauntedHunch.Pieces
{
    public class Freezer : Piece
    {
        // Moves to adjacent squares, freezes adjacent opponenet pieces. Doesn't get frozen.

        private static readonly int[,] a = new int[4, 2] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };
        public static int[,] A { get { return a; } }

        public Freezer(int r, int c, bool p)
        {
            row = r;
            column = c;
            player = p;
        }

        override public void PossibleMoves(ref Square[,] table, int turnDup)
        {
            table[row, column].BackgroundColor.Color = MainWindow.possible_move_color;
            // A Freezer can be frozen(as a state) but doesn't get effected by it.

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

            table[to_row, to_column].Image = table[row, column].Image;
            table[to_row, to_column].Piece = table[row, column].Piece;
            table[row, column].Image = emptyImage;
            table[row, column].Piece = null;
            row = to_row;
            column = to_column;
        }
    }
}
