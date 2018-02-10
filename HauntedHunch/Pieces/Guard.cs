namespace HauntedHunch.Pieces
{
    public class Guard : Piece
    {
        // Basic piece. Moves to adjacent squares, captures adjacent opponent pieces.

        private static readonly int[,] a = new int[4, 2] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };
        public static int[,] A { get { return a; } }

        public Guard(int r, int c, bool p)
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

            for (int i = 0; i < 4; i++)
            {
                // In bounds & (empty square | opponenet piece | psuedo piece)
                if (row + a[i, 0] <= 7 && row + a[i, 0] >= 1 && column + a[i, 1] <= 5 && column + a[i, 1] >= 1 &&
                    (table[row + a[i, 0], column + a[i, 1]].Piece == null || (table[row + a[i, 0], column + a[i, 1]].Piece.Player != player && turnDup % 2 == 1) ||
                    table[row + a[i, 0], column + a[i, 1]].Piece == table[row + a[i, 0], column + a[i, 1]].PsuedoPiece))
                {
                    table[row + a[i, 0], column + a[i, 1]].BackgroundColor.Color = MainWindow.possible_move_color;
                }
            }
        }

        public override void Move(ref Square[,] table, int to_row, int to_column, ref int turn)
        {
            if (table[to_row, to_column].Piece == null || table[to_row, to_column].Piece == table[to_row, to_column].PsuedoPiece)
            {
                turn++;
            }
            else // Capture
            {
                turn += 2;

                if (table[to_row, to_column].Piece.GetType() == typeof(MindController))
                {
                    turn += 2;

                    player = !player;
                    table[row, column].SetImageAccordingToPiece();
                }
            }

            table[to_row, to_column].Image = table[row, column].Image;
            table[to_row, to_column].Piece = table[row, column].Piece;
            table[row, column].Image = emptyImage;
            table[row, column].Piece = null;
            row = to_row;
            column = to_column;
        }
    }
}
