namespace HauntedHunch
{
    /// <summary>
    /// The bishop in chess with reduced (2 square) range.
    /// </summary>
    public class Ranger : Piece
    {
        public Ranger(int r, int c, PlayerType p) : base(r, c, p) { }

        public override void PossibleMoves(Square[,] table, int turn)
        {
            // Paint the square that piece is on so that the game feels responsive when you do not have any possible moves.
            table[Row, Column].State = SquareState.ChosenPiece;

            // Frozen check
            if (IsFrozen(table, Row, Column)) return;

            for (int i = 0; i < 8; i++)
                // In bounds & (short range | in between is (empty | psuedo)) && (empty square | opponenet piece | psuedo piece)
                if (Row + u[i, 0] <= nr && Row + u[i, 0] >= 1 && Column + u[i, 1] <= nc && Column + u[i, 1] >= 1 &&

                    // short range | in between is (empty | psuedo)
                    (i <= 3 || (i >= 4 && table[Row + u[i, 0] / 2, Column + u[i, 1] / 2].Piece == null || table[Row + u[i, 0] / 2, Column + u[i, 1] / 2].Piece == table[Row + u[i, 0] / 2, Column + u[i, 1] / 2].PseudoPiece)) &&

                    // empty square
                    (table[Row + u[i, 0], Column + u[i, 1]].Piece == null ||

                    // opponenet piece
                    (table[Row + u[i, 0], Column + u[i, 1]].Piece.Player != Player && turn % 2 == 1) ||

                    // psuedo piece
                    table[Row + u[i, 0], Column + u[i, 1]].Piece == table[Row + u[i, 0], Column + u[i, 1]].PseudoPiece))
                {
                    table[Row + u[i, 0], Column + u[i, 1]].State = SquareState.Moveable;
                }
        }

        public override void Move(Square[,] table, int toRow, int toColumn, ref int turn)
        {
            ClearSquareStates(table, Row, Column, u);

            Revealed = true;

            if (IsHiddenlyFrozen(table, Row, Column)) return;

            // Move
            if (table[toRow, toColumn].Piece == null || table[toRow, toColumn].Piece == table[toRow, toColumn].PseudoPiece)
            {
                turn++;
            }

            // Capture
            else if (table[toRow, toColumn].Piece is MindController)
            {
                turn += 4;
                Player = 1 - Player;
            }
            else
            {
                turn += 2;
            }


            table[toRow, toColumn].Piece = table[Row, Column].Piece;
            table[Row, Column].Piece = null;
            Row = toRow;
            Column = toColumn;
        }

        #region IClonable

        public override object Clone() => new Ranger(Row, Column, Player) { Revealed = Revealed };

        #endregion
    }
}
