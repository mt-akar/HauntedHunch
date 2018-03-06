namespace HauntedHunch.Pieces
{
    public class InnKeeper : Piece
    {
        // Weird af.

        private static readonly int[,] a = new int[8, 2] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 }, { 2, 0 }, { 0, 2 }, { -2, 0 }, { 0, -2 } };
        public static int[,] A { get { return a; } }

        public InnKeeper(int r, int c, bool p)
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

            for (int i = 0; i < 8; i++)
            {
                if (row + a[i, 0] <= 7 && row + a[i, 0] >= 1 && column + a[i, 1] <= 5 && column + a[i, 1] >= 1 &&
                    (i <= 3 || (i >= 4 && table[row + a[i, 0] / 2, column + a[i, 1] / 2].Piece == null)) &&
                    (table[row + a[i, 0], column + a[i, 1]].Piece == null ||
                    (table[row + a[i, 0], column + a[i, 1]].Piece.GetType() == typeof(Lotus) && table[row + a[i, 0], column + a[i, 1]].Piece.Player != player && turnDup % 2 == 1)))
                {
                    table[row + a[i, 0], column + a[i, 1]].BackgroundColor.Color = MainWindow.possible_move_color;
                }
            }
        }

        public override void Move(ref Square[,] table, int to_row, int to_column, ref int turn)
        {
            PaintToDefault(ref table, row, column, a, 8);

            turn += table[to_row, to_column].Piece == null ? 1 : 2;

            table[to_row, to_column].PsuedoPiece = table[row, column].PsuedoPiece;
            table[to_row, to_column].Image = table[row, column].Image;
            table[to_row, to_column].Piece = table[row, column].Piece;
            table[row, column].PsuedoPiece = null;
            table[row, column].Image = emptyImage;
            table[row, column].Piece = null;

            row = to_row;
            column = to_column;
        }
    }
}
