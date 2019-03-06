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
            table[Row, Column].State = SquareState.ChosenPiece;

            // Frozen check
            if (IsFrozen(table, Row, Column)) return;

            for (int i = 0; i < 4; i++)
                // In bounds & (empty square | opponenet piece | psuedo piece)
                if (Row + e[i, 0] <= nr && Row + e[i, 0] >= 1 && Column + e[i, 1] <= nc && Column + e[i, 1] >= 1 &&

                    // empty square
                    (table[Row + e[i, 0], Column + e[i, 1]].Piece == null ||

                    // opponenet piece
                    (table[Row + e[i, 0], Column + e[i, 1]].Piece.Player != Player && turn % 2 == 1) ||

                    // psuedo piece
                    table[Row + e[i, 0], Column + e[i, 1]].Piece == table[Row + e[i, 0], Column + e[i, 1]].PseudoPiece))
                {
                    table[Row + e[i, 0], Column + e[i, 1]].State = SquareState.Moveable;
                }
        }

        public override void Move(Square[,] table, int toRow, int toColumn, ref int turn)
        {
            ClearSquareStates(table, Row, Column, e);

            if (IsHiddenlyFrozen(table, Row, Column)) return;

            // Move
            if (table[toRow, toColumn].Piece == null || table[toRow, toColumn].Piece == table[toRow, toColumn].PseudoPiece)
            {
                turn++;
            }

            // Capture
            else
            {
                Revealed = true;

                if (table[toRow, toColumn].Piece is MindController)
                {
                    turn += 4;
                    Player = 1 - Player;
                }
                else
                {
                    turn += 2;
                }
            }


            table[toRow, toColumn].Piece = table[Row, Column].Piece;
            table[Row, Column].Piece = null;
            Row = toRow;
            Column = toColumn;
        }

        #region IClonable

        public override object Clone() => new Guard(Row, Column, Player) { Revealed = Revealed };

        #endregion
    }
}
