namespace HauntedHunch
{
    /// <summary>
    /// Moves to adjacent squares. Can suicide, removing all adjacent pieces.
    /// </summary>
    public class Boomer : Piece
    {
        public Boomer(int r, int c, PlayerType p) : base(r, c, p) { }

        public override void PossibleMoves(Square[,] table, int turn)
        {
            // Paint the square that piece is on so that the game feels responsive when you do not have any possible moves.
            table[Row, Column].State = SquareState.ChosenPiece;

            // Frozen check
            if (IsFrozen(table, Row, Column)) return;

            table[Row, Column].State = SquareState.AbilityUnoable; // Suicide

            for (int i = 0; i < 4; i++)
                // In bounds & (empty square | psuedo piece)
                if (Row + e[i, 0] <= nr && Row + e[i, 0] >= 1 && Column + e[i, 1] <= nc && Column + e[i, 1] >= 1 && (table[Row + e[i, 0], Column + e[i, 1]].Piece == null ||
                    table[Row + e[i, 0], Column + e[i, 1]].Piece == table[Row + e[i, 0], Column + e[i, 1]].PseudoPiece))
                {
                    table[Row + e[i, 0], Column + e[i, 1]].State = SquareState.Moveable;
                }
        }

        public override void Move(Square[,] table, int toRow, int toColumn, ref int turn)
        {
            ClearSquareStates(table, Row, Column, e);

            if (IsHiddenlyFrozen(table, Row, Column)) return;

            turn++;

            table[toRow, toColumn].Piece = table[Row, Column].Piece;
            table[Row, Column].Piece = null;
            Row = toRow;
            Column = toColumn;
        }

        // Suicide
        public override void AbilityUno(Square[,] table, ref int turn)
        {
            ClearSquareStates(table, Row, Column, e);

            if (IsHiddenlyFrozen(table, Row, Column))
            {
                Revealed = true;
                return;
            }

            turn++;

            for (int i = 0; i < 4; i++)
                // In bounds & not null
                if (Row + e[i, 0] <= nr && Row + e[i, 0] >= 1 && Column + e[i, 1] <= nc && Column + e[i, 1] >= 1 && table[Row + e[i, 0], Column + e[i, 1]].Piece != null)
                {
                    table[Row + e[i, 0], Column + e[i, 1]].PseudoPiece = null;
                    table[Row + e[i, 0], Column + e[i, 1]].Piece = null;
                }
            table[Row, Column].PseudoPiece = null;
            table[Row, Column].Piece = null;
        }

        #region IClonable

        public override object Clone() => new Boomer(Row, Column, Player) { Revealed = Revealed };

        #endregion
    }
}
