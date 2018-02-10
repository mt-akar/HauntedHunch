namespace HauntedHunch.Pieces
{
    public class Lotus : Piece
    {
        // Moves to adjacent squares. Cannot move if it is in the range of any opponent piece.
        // Ranges of pieces are defined with their member varable a. Any piece can interfere long ranges of Ranger and InnKeeper.
        // A Jumper doesn't freeze a lotus from 2 squares away even though it has long range.

        // TODO: Triple move

        private static readonly int[,] a = new int[4, 2] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };
        public static int[,] A { get { return a; } }

        public Lotus(int r, int c, bool p)
        {
            row = r;
            column = c;
            player = p;
            frozen = false;
        }

        override public void PossibleMoves(ref Square[,] table, int turnDup)
        {
            table[row, column].BackgroundColor.Color = MainWindow.possible_move_color;

            // Frozen lotus check.

            int[,] r = Runner.A; // Range of Runner
            int[,] R = Ranger.A; // Range of Ranger
            int[,] K = InnKeeper.A; // Range of InnKeeper

            bool able = true;

            for (int j = 0; j < 4; j++)
            {
                // Never seen more booleans in a single if statement. No way to simplify.
                if ((row + a[j, 0] <= 7 && row + a[j, 0] >= 1 && column + a[j, 1] <= 5 && column + a[j, 1] >= 1 &&
                    table[row + a[j, 0], column + a[j, 1]].Piece != null && table[row + a[j, 0], column + a[j, 1]].Piece.Player != player &&
                    (table[row + a[j, 0], column + a[j, 1]].Piece.GetType() == typeof(Guard) ||
                    table[row + a[j, 0], column + a[j, 1]].Piece.GetType() == typeof(Jumper) ||
                    table[row + a[j, 0], column + a[j, 1]].Piece.GetType() == typeof(Freezer) ||
                    table[row + a[j, 0], column + a[j, 1]].Piece.GetType() == typeof(Converter) ||
                    table[row + a[j, 0], column + a[j, 1]].Piece.GetType() == typeof(Courier) ||
                    table[row + a[j, 0], column + a[j, 1]].Piece.GetType() == typeof(Boomer) ||
                    table[row + a[j, 0], column + a[j, 1]].Piece.GetType() == typeof(MindController))) ||

                    // Runner
                    (row + r[j, 0] <= 7 && row + r[j, 0] >= 1 && column + r[j, 1] <= 5 && column + r[j, 1] >= 1 &&
                    table[row + r[j, 0], column + r[j, 1]].Piece != null && table[row + r[j, 0], column + r[j, 1]].Piece.Player != player && table[row + r[j, 0], column + r[j, 1]].Piece.GetType() == typeof(Runner)) ||
                    (row + r[j + 4, 0] <= 7 && row + r[j + 4, 0] >= 1 && column + r[j + 4, 1] <= 5 && column + r[j + 4, 1] >= 1 &&
                    table[row + r[j + 4, 0], column + r[j + 4, 1]].Piece != null && table[row + r[j + 4, 0], column + r[j + 4, 1]].Piece.Player != player && table[row + r[j + 4, 0], column + r[j + 4, 1]].Piece.GetType() == typeof(Runner)) ||

                    // Ranger
                    (row + R[j, 0] <= 7 && row + R[j, 0] >= 1 && column + R[j, 1] <= 5 && column + R[j, 1] >= 1 &&
                    table[row + R[j, 0], column + R[j, 1]].Piece != null && table[row + R[j, 0], column + R[j, 1]].Piece.Player != player && table[row + R[j, 0], column + R[j, 1]].Piece.GetType() == typeof(Ranger)) ||
                    (row + R[j + 4, 0] <= 7 && row + R[j + 4, 0] >= 1 && column + R[j + 4, 1] <= 5 && column + R[j + 4, 1] >= 1 &&
                    table[row + R[j + 4, 0], column + R[j + 4, 1]].Piece != null && table[row + R[j + 4, 0], column + R[j + 4, 1]].Piece.Player != player && table[row + R[j + 4, 0], column + R[j + 4, 1]].Piece.GetType() == typeof(Ranger) &&
                    (table[row + R[j + 4, 0] / 2, column + R[j + 4, 1] / 2].Piece == null || (table[row + R[j + 4, 0] / 2, column + R[j + 4, 1] / 2].PsuedoPiece != null && table[row + R[j + 4, 0] / 2, column + R[j + 4, 1] / 2].PsuedoPiece == table[row + R[j + 4, 0] / 2, column + R[j + 4, 1] / 2].Piece))) ||

                    // Inn Keeper
                    (row + K[j, 0] <= 7 && row + K[j, 0] >= 1 && column + K[j, 1] <= 5 && column + K[j, 1] >= 1 &&
                    table[row + K[j, 0], column + K[j, 1]].Piece != null && table[row + K[j, 0], column + K[j, 1]].Piece.Player != player && table[row + K[j, 0], column + K[j, 1]].Piece.GetType() == typeof(InnKeeper)) ||
                    (row + K[j + 4, 0] <= 7 && row + K[j + 4, 0] >= 1 && column + K[j + 4, 1] <= 5 && column + K[j + 4, 1] >= 1 &&
                    table[row + K[j + 4, 0], column + K[j + 4, 1]].Piece != null && table[row + K[j + 4, 0], column + K[j + 4, 1]].Piece.Player != player && table[row + K[j + 4, 0], column + K[j + 4, 1]].Piece.GetType() == typeof(InnKeeper) && table[row + K[j + 4, 0] / 2, column + K[j + 4, 1] / 2].Piece == null))
                {
                    able = false;
                    break;
                }
            }

            // If able to move
            if (able)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (row + a[i, 0] <= 7 && row + a[i, 0] >= 1 && column + a[i, 1] <= 5 && column + a[i, 1] >= 1 && table[row + a[i, 0], column + a[i, 1]].Piece == null)
                    {
                        table[row + a[i, 0], column + a[i, 1]].BackgroundColor.Color = MainWindow.possible_move_color;
                    }
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
