namespace HauntedHunch.Pieces
{
    public class Lotus : Piece
    {
        // Triple move
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
            int[,] b = { { 2, 1 }, { 1, 2 }, { -1, 2 }, { -2, 1 }, { -2, -1 }, { -1, -2 }, { 1, -2 }, { 2, -1 } };
            int[,] c = { { 1, 1 }, { -1, 1 }, { -1, -1 }, { 1, -1 }, { 2, 2 }, { -2, 2 }, { -2, -2 }, { 2, -2 } };
            int[,] d = { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 }, { 2, 0 }, { 0, 2 }, { -2, 0 }, { 0, -2 } };

            int[,] pos = new int[4, 3];
            bool able = true;
            
            for (int j = 0; j < 4; j++)
            {
                if ((row + a[j, 0] <= 7 && row + a[j, 0] >= 1 && column + a[j, 1] <= 5 && column + a[j, 1] >= 1 && tableDup[row + a[j, 0], column + a[j, 1]].Piece != null && tableDup[row + a[j, 0], column + a[j, 1]].Piece.Player != player &&
                    (tableDup[row + a[j, 0], column + a[j, 1]].Piece.GetType() == typeof(Guard) ||
                    tableDup[row + a[j, 0], column + a[j, 1]].Piece.GetType() == typeof(Jumper) ||
                    tableDup[row + a[j, 0], column + a[j, 1]].Piece.GetType() == typeof(Freezer) ||
                    tableDup[row + a[j, 0], column + a[j, 1]].Piece.GetType() == typeof(Converter) ||
                    tableDup[row + a[j, 0], column + a[j, 1]].Piece.GetType() == typeof(Courier) ||
                    tableDup[row + a[j, 0], column + a[j, 1]].Piece.GetType() == typeof(Boomer))) ||
                    (row + b[j, 0] <= 7 && row + b[j, 0] >= 1 && column + b[j, 1] <= 5 && column + b[j, 1] >= 1 && tableDup[row + b[j, 0], column + b[j, 1]].Piece != null && tableDup[row + b[j, 0], column + b[j, 1]].Piece.Player != player && tableDup[row + b[j, 0], column + b[j, 1]].Piece.GetType() == typeof(Runner)) ||
                    (row + b[j + 4, 0] <= 7 && row + b[j + 4, 0] >= 1 && column + b[j + 4, 1] <= 5 && column + b[j + 4, 1] >= 1 && tableDup[row + b[j + 4, 0], column + b[j + 4, 1]].Piece != null && tableDup[row + b[j + 4, 0], column + b[j + 4, 1]].Piece.Player != player && tableDup[row + b[j + 4, 0], column + b[j + 4, 1]].Piece.GetType() == typeof(Runner)) ||
                    (row + c[j, 0] <= 7 && row + c[j, 0] >= 1 && column + c[j, 1] <= 5 && column + c[j, 1] >= 1 && tableDup[row + c[j, 0], column + c[j, 1]].Piece != null && tableDup[row + c[j, 0], column + c[j, 1]].Piece.Player != player && tableDup[row + c[j, 0], column + c[j, 1]].Piece.GetType() == typeof(Ranger)) ||
                    (row + c[j + 4, 0] <= 7 && row + c[j + 4, 0] >= 1 && column + c[j + 4, 1] <= 5 && column + c[j + 4, 1] >= 1 && tableDup[row + c[j + 4, 0], column + c[j + 4, 1]].Piece != null && tableDup[row + c[j + 4, 0], column + c[j + 4, 1]].Piece.Player != player && tableDup[row + c[j + 4, 0], column + c[j + 4, 1]].Piece.GetType() == typeof(Ranger) && tableDup[row + c[j + 4, 0] / 2, column + c[j + 4, 1] / 2].Piece == null) ||
                    (row + d[j, 0] <= 7 && row + d[j, 0] >= 1 && column + d[j, 1] <= 5 && column + d[j, 1] >= 1 && tableDup[row + d[j, 0], column + d[j, 1]].Piece != null && tableDup[row + d[j, 0], column + d[j, 1]].Piece.Player != player && tableDup[row + d[j, 0], column + d[j, 1]].Piece.GetType() == typeof(InnKeeper)) ||
                    (row + d[j + 4, 0] <= 7 && row + d[j + 4, 0] >= 1 && column + d[j + 4, 1] <= 5 && column + d[j + 4, 1] >= 1 && tableDup[row + d[j + 4, 0], column + d[j + 4, 1]].Piece != null && tableDup[row + d[j + 4, 0], column + d[j + 4, 1]].Piece.Player != player && tableDup[row + d[j + 4, 0], column + d[j + 4, 1]].Piece.GetType() == typeof(InnKeeper) && tableDup[row + d[j + 4, 0] / 2, column + d[j + 4, 1] / 2].Piece == null))
                {
                    able = false;
                    break;
                }
            }
            
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
