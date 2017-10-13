namespace HauntedHunch.Pieces
{
    public class Freezer : Piece
    {
        public Freezer(int r, int c, bool p)
        {
            row = r;
            column = c;
            player = p;
            a = new int[4, 2] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };
        }

        override public int[,] PossibleMoves(Square[,] tableDup, int turnDup)
        {
            int[,] pos = new int[4, 3];
            for (int i = 0; i < 4; i++)
            {
                if (row + a[i, 0] <= 7 && row + a[i, 0] >= 1 && column + a[i, 1] <= 5 && column + a[i, 1] >= 1 && (tableDup[row + a[i, 0], column + a[i, 1]].Piece == null ||
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
