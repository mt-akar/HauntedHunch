namespace HauntedHunch.Pieces
{
    public class Runner : Piece
    {
        // Exact same piece with the Kinght in the chess.

        public Runner(int r, int c, bool p)
        {
            row = r;
            column = c;
            player = p;
            frozen = false;
            a = new int[8, 2] { { 2, 1 }, { 1, 2 }, { -1, 2 }, { -2, 1 }, { -2, -1 }, { -1, -2 }, { 1, -2 }, { 2, -1 } };
        }

        override public int[,] PossibleMoves(Square[,] tableDup, int turnDup)
        {
            if (frozen) { return new int[0, 0]; }

            int[,] pos = new int[8, 3];
            for (int i = 0; i < 8; i++)
            {
                if (row + a[i, 0] <= 7 && row + a[i, 0] >= 1 && column + a[i, 1] <= 5 && column + a[i, 1] >= 1 &&
                    (tableDup[row + a[i, 0], column + a[i, 1]].Piece == null || (tableDup[row + a[i, 0], column + a[i, 1]].Piece.Player != player && turnDup % 2 == 1) ||
                    tableDup[row + a[i, 0], column + a[i, 1]].Piece == tableDup[row + a[i, 0], column + a[i, 1]].PsuedoPiece))
                {
                    pos[i, 0] = row + a[i, 0];
                    pos[i, 1] = column + a[i, 1];
                }
            }
            return pos;
        }

        public override void Move(ref Square[,] table, int to_row, int to_column, ref int turn)
        {
            turn += table[to_row, to_column].Piece == null || table[to_row, to_column].Piece == table[to_row, to_column].PsuedoPiece ? 1 : 2;

            table[to_row, to_column].Image = table[row, column].Image;
            table[to_row, to_column].Piece = table[row, column].Piece;
            table[row, column].Image = emptyImage;
            table[row, column].Piece = null;

            row = to_row;
            column = to_column;
        }
    }
}