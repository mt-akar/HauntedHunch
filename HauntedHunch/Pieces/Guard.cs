namespace HauntedHunch
{
    /// <summary>
    /// Basic piece. Moves to adjacent squares, captures adjacent opponent pieces.
    /// </summary>
    public class Guard : Piece
    {
        public Guard(int r, int c, PlayerType p) : base(r, c, p) { }

        public override void PossibleMoves(Square[,] table, int turn)
        {
            // Paint the square that piece is on so that the game feels responsive when you do not have any possible moves.
            table[Row, Column].BackgroundColor = BoardHelper.standartMoveColor;

            // Frozen check
            if (IsFrozen(table, Row, Column)) return;

            for (int i = 0; i < 4; i++)
            {
                // In bounds & (empty square | opponenet piece | psuedo piece)
                if (Row + e[i, 0] <= nr && Row + e[i, 0] >= 1 && Column + e[i, 1] <= nc && Column + e[i, 1] >= 1 &&

                    // empty square
                    (table[Row + e[i, 0], Column + e[i, 1]].Piece == null ||

                    // opponenet piece
                    (table[Row + e[i, 0], Column + e[i, 1]].Piece.Player != Player && turn % 2 == 1) ||

                    // psuedo piece
                    table[Row + e[i, 0], Column + e[i, 1]].Piece == table[Row + e[i, 0], Column + e[i, 1]].PsuedoPiece))
                {
                    table[Row + e[i, 0], Column + e[i, 1]].BackgroundColor = BoardHelper.standartMoveColor;
                }
            }
        }

        public override void Move(Square[,] table, int to_row, int to_column, ref int turn)
        {
            PaintToDefault(table, Row, Column, e);

            if (IsHiddenlyFrozen(table, Row, Column)) return;

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

        #region IClonable

        public override object Clone() => new Guard(Row, Column, Player) { Revealed = Revealed };

        #endregion
    }
}
