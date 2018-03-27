namespace HauntedHunch
{
    /// <summary>
    /// The bishop in chess with reduced (2 square) range.
    /// </summary>
    public class Ranger : Piece
    {
        public Ranger(int r, int c, PlayerType p) : base(r, c, p) { }

        override public void PossibleMoves(Square[,] table, int turn)
        {
            // Paint the square that piece is on so that the game feels responsive when you do not have any possible moves.
            table[Row, Column].BackgroundColor.Color = BoardHelper.standartMoveColor;

            // Frozen check
            if (IsFrozen(table, Row, Column)) return;

            for (int i = 0; i < 8; i++)
            {
                // In bounds & (short range | in between is (empty | psuedo)) && (empty square | opponenet piece | psuedo piece)
                if (Row + c[i, 0] <= 7 && Row + c[i, 0] >= 1 && Column + c[i, 1] <= 5 && Column + c[i, 1] >= 1 &&

                    // short range | in between is (empty | psuedo)
                    (i <= 3 || (i >= 4 && table[Row + c[i, 0] / 2, Column + c[i, 1] / 2].Piece == null || table[Row + c[i, 0] / 2, Column + c[i, 1] / 2].Piece == table[Row + c[i, 0] / 2, Column + c[i, 1] / 2].PsuedoPiece)) &&

                    // empty square
                    (table[Row + c[i, 0], Column + c[i, 1]].Piece == null ||

                    // opponenet piece
                    (table[Row + c[i, 0], Column + c[i, 1]].Piece.Player != Player && turn % 2 == 1) ||

                    // psuedo piece
                    table[Row + c[i, 0], Column + c[i, 1]].Piece == table[Row + c[i, 0], Column + c[i, 1]].PsuedoPiece))
                {
                    table[Row + c[i, 0], Column + c[i, 1]].BackgroundColor.Color = BoardHelper.standartMoveColor;
                }
            }
        }

        public override void Move(Square[,] table, int to_row, int to_column, ref int turn)
        {
            PaintToDefault(table, Row, Column, c);

            if (table[to_row, to_column].Piece == null || table[to_row, to_column].Piece == table[to_row, to_column].PsuedoPiece)
            {
                turn++;
            }
            else // Capture
            {
                turn += 2;

                if (table[to_row, to_column].Piece is MindController)
                {
                    turn += 2;

                    Player = 1 - Player;
                }
            }

            table[to_row, to_column].Piece = table[Row, Column].Piece;
            table[Row, Column].Piece = null;
            Row = to_row;
            Column = to_column;
        }
    }
}
