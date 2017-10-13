using System;

namespace HauntedHunch.Pieces
{
    public class Jumper : Piece
    {
        // TODO: Double capture jump
        public Jumper(int r, int c, bool p)
        {
            row = r;
            column = c;
            player = p;
            frozen = false;
            a = new int[8, 2] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 }, { 2, 0 }, { 0, 2 }, { -2, 0 }, { 0, -2 } };
        }

        public override int[,] PossibleMoves(Square[,] tableDup, int turnDup)
        {
            if (frozen) { return new int[0, 0]; }

            int[,] pos = new int[8, 3];
            for (int i = 0; i < 8; i++)
            {
                if (row + a[i, 0] <= 7 && row + a[i, 0] >= 1 && column + a[i, 1] <= 5 && column + a[i, 1] >= 1 &&
                    (i <= 3 || (i >= 4 && tableDup[row + a[i, 0] / 2, column + a[i, 1] / 2].Piece != null && tableDup[row + a[i, 0] / 2, column + a[i, 1] / 2].Piece.Player != player)) &&
                    (tableDup[row + a[i, 0], column + a[i, 1]].Piece == null))
                {
                    pos[i, 0] = row + a[i, 0];
                    pos[i, 1] = column + a[i, 1];
                }
            }
            return pos;
        }

        public override void Move(ref Square[,] table, int to_row, int to_column, ref int turn)
        {
            turn += Math.Abs(row + column - to_row - to_column) == 2 && table[(row + to_row) / 2, (column + to_column) / 2].Piece.GetType() != typeof(InnKeeper) ? 2 : 1;

            table[to_row, to_column].Image = table[row, column].Image;
            table[to_row, to_column].Piece = table[row, column].Piece;
            table[row, column].Image = emptyImage;
            table[row, column].Piece = null;

            if (Math.Abs(row + column - to_row - to_column) == 2)
            {
                table[Math.Abs(row + to_row) / 2, Math.Abs(column + to_column) / 2].Image = emptyImage;
                table[Math.Abs(row + to_row) / 2, Math.Abs(column + to_column) / 2].Piece = null;
            }

            row = to_row;
            column = to_column;
        }
    }
}
