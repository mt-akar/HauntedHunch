namespace HauntedHunch
{
    /// <summary>
    /// Exact same piece with the Kinght in the chess.
    /// </summary>
    public class Runner : Piece
    {
        public Runner(int r, int c, PlayerType p) : base(r, c, p) { }

        override public void PossibleMoves(Square[,] table, int turn)
        {
            // Paint the square that piece is on so that the game feels responsive when you do not have any possible moves.
            table[Row, Column].BackgroundColor = BoardHelper.standartMoveColor;

            // Frozen check
            if (IsFrozen(table, Row, Column)) return;

            for (int i = 0; i < 8; i++)
            {
                // In bounds & (empty square | opponenet piece | psuedo piece)
                if (Row + j[i, 0] <= nr && Row + j[i, 0] >= 1 && Column + j[i, 1] <= nc && Column + j[i, 1] >= 1 &&

                    // empty square
                    (table[Row + j[i, 0], Column + j[i, 1]].Piece == null ||

                    // opponenet piece
                    (table[Row + j[i, 0], Column + j[i, 1]].Piece.Player != Player && turn % 2 == 1) ||

                    // psuedo piece
                    table[Row + j[i, 0], Column + j[i, 1]].Piece == table[Row + j[i, 0], Column + j[i, 1]].PsuedoPiece))
                {
                    table[Row + j[i, 0], Column + j[i, 1]].BackgroundColor = BoardHelper.standartMoveColor;
                }
            }
        }

        public override void Move(Square[,] table, int to_row, int to_column, ref int turn)
        {
            PaintToDefault(table, Row, Column, j);

            Revealed = true;

            if (table[to_row, to_column].Piece == null || table[to_row, to_column].Piece == table[to_row, to_column].PsuedoPiece) // Move
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

        #region IClonable

        public override object Clone() => new Runner(Row, Column, Player);

        #endregion
    }
}