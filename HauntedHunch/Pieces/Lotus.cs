namespace HauntedHunch.Pieces
{
    public class Lotus : Piece
    {
        // Moves to adjacent squares. Cannot move if it is in the range(*1) of any opponent piece.
        // *1 Ranges of pieces are defined with their member varable a. Any piece can interfe long ranges of Ranger and InnKeeper.
        // *1 A Jumper doesn't freeze a lotus from 2 squares away even though it has long range.

        // TODO: Triple move
        public Lotus(int r, int c, bool p)
        {
            row = r;
            column = c;
            player = p;
            frozen = false;
            a = new int[4, 2] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };
        }

        override public int[,] PossibleMoves(Square[,] tableDup, int turnDup)
        {
            if (frozen) { return new int[0, 0]; }
            // Range of Runner
            int[,] n = { { 2, 1 }, { 1, 2 }, { -1, 2 }, { -2, 1 }, { -2, -1 }, { -1, -2 }, { 1, -2 }, { 2, -1 } };
            // Range of Ranger
            int[,] g = { { 1, 1 }, { -1, 1 }, { -1, -1 }, { 1, -1 }, { 2, 2 }, { -2, 2 }, { -2, -2 }, { 2, -2 } };
            // Range of InnKeeper
            int[,] k = { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 }, { 2, 0 }, { 0, 2 }, { -2, 0 }, { 0, -2 } };

            int[,] pos = new int[4, 3];
            bool able = true;
            
            for (int j = 0; j < 4; j++)
            {
                // Frozen lotus check. (Never seen more booleans in a single if statement. No way to simplify.)
                if ((row + a[j, 0] <= 7 && row + a[j, 0] >= 1 && column + a[j, 1] <= 5 && column + a[j, 1] >= 1 && tableDup[row + a[j, 0], column + a[j, 1]].Piece != null && tableDup[row + a[j, 0], column + a[j, 1]].Piece.Player != player &&
                    (tableDup[row + a[j, 0], column + a[j, 1]].Piece.GetType() == typeof(Guard) ||
                    tableDup[row + a[j, 0], column + a[j, 1]].Piece.GetType() == typeof(Jumper) ||
                    tableDup[row + a[j, 0], column + a[j, 1]].Piece.GetType() == typeof(Freezer) ||
                    tableDup[row + a[j, 0], column + a[j, 1]].Piece.GetType() == typeof(Converter) ||
                    tableDup[row + a[j, 0], column + a[j, 1]].Piece.GetType() == typeof(Courier) ||
                    tableDup[row + a[j, 0], column + a[j, 1]].Piece.GetType() == typeof(Boomer))) ||
                    (row + n[j, 0] <= 7 && row + n[j, 0] >= 1 && column + n[j, 1] <= 5 && column + n[j, 1] >= 1 && tableDup[row + n[j, 0], column + n[j, 1]].Piece != null && tableDup[row + n[j, 0], column + n[j, 1]].Piece.Player != player && tableDup[row + n[j, 0], column + n[j, 1]].Piece.GetType() == typeof(Runner)) ||
                    (row + n[j + 4, 0] <= 7 && row + n[j + 4, 0] >= 1 && column + n[j + 4, 1] <= 5 && column + n[j + 4, 1] >= 1 && tableDup[row + n[j + 4, 0], column + n[j + 4, 1]].Piece != null && tableDup[row + n[j + 4, 0], column + n[j + 4, 1]].Piece.Player != player && tableDup[row + n[j + 4, 0], column + n[j + 4, 1]].Piece.GetType() == typeof(Runner)) ||
                    (row + g[j, 0] <= 7 && row + g[j, 0] >= 1 && column + g[j, 1] <= 5 && column + g[j, 1] >= 1 && tableDup[row + g[j, 0], column + g[j, 1]].Piece != null && tableDup[row + g[j, 0], column + g[j, 1]].Piece.Player != player && tableDup[row + g[j, 0], column + g[j, 1]].Piece.GetType() == typeof(Ranger)) ||
                    (row + g[j + 4, 0] <= 7 && row + g[j + 4, 0] >= 1 && column + g[j + 4, 1] <= 5 && column + g[j + 4, 1] >= 1 && tableDup[row + g[j + 4, 0], column + g[j + 4, 1]].Piece != null && tableDup[row + g[j + 4, 0], column + g[j + 4, 1]].Piece.Player != player && tableDup[row + g[j + 4, 0], column + g[j + 4, 1]].Piece.GetType() == typeof(Ranger) && tableDup[row + g[j + 4, 0] / 2, column + g[j + 4, 1] / 2].Piece == null) ||
                    (row + k[j, 0] <= 7 && row + k[j, 0] >= 1 && column + k[j, 1] <= 5 && column + k[j, 1] >= 1 && tableDup[row + k[j, 0], column + k[j, 1]].Piece != null && tableDup[row + k[j, 0], column + k[j, 1]].Piece.Player != player && tableDup[row + k[j, 0], column + k[j, 1]].Piece.GetType() == typeof(InnKeeper)) ||
                    (row + k[j + 4, 0] <= 7 && row + k[j + 4, 0] >= 1 && column + k[j + 4, 1] <= 5 && column + k[j + 4, 1] >= 1 && tableDup[row + k[j + 4, 0], column + k[j + 4, 1]].Piece != null && tableDup[row + k[j + 4, 0], column + k[j + 4, 1]].Piece.Player != player && tableDup[row + k[j + 4, 0], column + k[j + 4, 1]].Piece.GetType() == typeof(InnKeeper) && tableDup[row + k[j + 4, 0] / 2, column + k[j + 4, 1] / 2].Piece == null))
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
                    if (row + a[i, 0] <= 7 && row + a[i, 0] >= 1 && column + a[i, 1] <= 5 && column + a[i, 1] >= 1 && tableDup[row + a[i, 0], column + a[i, 1]].Piece == null)
                    {
                        pos[i, 0] = row + a[i, 0];
                        pos[i, 1] = column + a[i, 1];
                    }
                }
            }
            return pos;
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
