namespace HauntedHunch
{
    /// <summary>
    /// Moves to adjacent squares. Cannot move if it is in the range of any opponent piece.
    /// </summary>
    public class Lotus : Piece
    {
        // Any piece can interfere long ranges of Ranger and InnKeeper.
        // TODO: Triple move

        public Lotus(int r, int c, PlayerType p) : base(r, c, p) { }

        public override void PossibleMoves(SquareViewModel[,] table, int turn)
        {
            // Paint the square that piece is on so that the game feels responsive when you do not have any possible moves.
            table[Row, Column].State = SquareState.ChosenPiece;

            // Frozen lotus check.

            bool ableToMove = true;

            for (int i = 0; i < 4; i++)
                // Never seen more booleans in a single if statement. No way to simplify??
                if (Row + e[i, 0] <= nr && Row + e[i, 0] >= 1 && Column + e[i, 1] <= nc && Column + e[i, 1] >= 1 &&
                    table[Row + e[i, 0], Column + e[i, 1]].Piece != null && table[Row + e[i, 0], Column + e[i, 1]].Piece.Player != Player &&
                    (table[Row + e[i, 0], Column + e[i, 1]].Piece is Guard ||
                    table[Row + e[i, 0], Column + e[i, 1]].Piece is Jumper ||
                    table[Row + e[i, 0], Column + e[i, 1]].Piece is Freezer ||
                    table[Row + e[i, 0], Column + e[i, 1]].Piece is Converter ||
                    table[Row + e[i, 0], Column + e[i, 1]].Piece is Courier ||
                    table[Row + e[i, 0], Column + e[i, 1]].Piece is Boomer ||
                    table[Row + e[i, 0], Column + e[i, 1]].Piece is MindController) ||

                    // Runner
                    Row + j[i, 0] <= nr && Row + j[i, 0] >= 1 && Column + j[i, 1] <= nc && Column + j[i, 1] >= 1 &&
                    table[Row + j[i, 0], Column + j[i, 1]].Piece != null && table[Row + j[i, 0], Column + j[i, 1]].Piece.Player != Player && table[Row + j[i, 0], Column + j[i, 1]].Piece is Runner ||
                    Row + j[i + 4, 0] <= nr && Row + j[i + 4, 0] >= 1 && Column + j[i + 4, 1] <= nc && Column + j[i + 4, 1] >= 1 &&
                    table[Row + j[i + 4, 0], Column + j[i + 4, 1]].Piece != null && table[Row + j[i + 4, 0], Column + j[i + 4, 1]].Piece.Player != Player && table[Row + j[i + 4, 0], Column + j[i + 4, 1]].Piece is Runner ||

                    // Ranger
                    Row + u[i, 0] <= nr && Row + u[i, 0] >= 1 && Column + u[i, 1] <= nc && Column + u[i, 1] >= 1 &&
                    table[Row + u[i, 0], Column + u[i, 1]].Piece != null && table[Row + u[i, 0], Column + u[i, 1]].Piece.Player != Player && table[Row + u[i, 0], Column + u[i, 1]].Piece is Ranger ||
                    Row + u[i + 4, 0] <= nr && Row + u[i + 4, 0] >= 1 && Column + u[i + 4, 1] <= nc && Column + u[i + 4, 1] >= 1 &&
                    table[Row + u[i + 4, 0], Column + u[i + 4, 1]].Piece != null && table[Row + u[i + 4, 0], Column + u[i + 4, 1]].Piece.Player != Player && table[Row + u[i + 4, 0], Column + u[i + 4, 1]].Piece is Ranger &&
                    (table[Row + u[i + 4, 0] / 2, Column + u[i + 4, 1] / 2].Piece == null || table[Row + u[i + 4, 0] / 2, Column + u[i + 4, 1] / 2].PseudoPiece != null && table[Row + u[i + 4, 0] / 2, Column + u[i + 4, 1] / 2].PseudoPiece == table[Row + u[i + 4, 0] / 2, Column + u[i + 4, 1] / 2].Piece) ||

                    // Inn Keeper
                    Row + l[i, 0] <= nr && Row + l[i, 0] >= 1 && Column + l[i, 1] <= nc && Column + l[i, 1] >= 1 &&
                    table[Row + l[i, 0], Column + l[i, 1]].Piece != null && table[Row + l[i, 0], Column + l[i, 1]].Piece.Player != Player && table[Row + l[i, 0], Column + l[i, 1]].Piece is InnKeeper ||
                    Row + l[i + 4, 0] <= nr && Row + l[i + 4, 0] >= 1 && Column + l[i + 4, 1] <= nc && Column + l[i + 4, 1] >= 1 &&
                    table[Row + l[i + 4, 0], Column + l[i + 4, 1]].Piece != null && table[Row + l[i + 4, 0], Column + l[i + 4, 1]].Piece.Player != Player && table[Row + l[i + 4, 0], Column + l[i + 4, 1]].Piece is InnKeeper && table[Row + l[i + 4, 0] / 2, Column + l[i + 4, 1] / 2].Piece == null)
                {
                    ableToMove = false;
                    break;
                }

            // If able to move
            if (ableToMove)
                for (int i = 0; i < 4; i++)
                    if (Row + e[i, 0] <= nr && Row + e[i, 0] >= 1 && Column + e[i, 1] <= nc && Column + e[i, 1] >= 1 && table[Row + e[i, 0], Column + e[i, 1]].Piece == null)
                    {
                        table[Row + e[i, 0], Column + e[i, 1]].State = SquareState.Moveable;
                    }
        }

        public override void Move(SquareViewModel[,] table, int toRow, int toColumn, ref int turn)
        {
            ClearSquareStates(table, Row, Column, e);

            if (IsHiddenlyFrozen(table, Row, Column)) return;

            turn++;

            table[toRow, toColumn].Piece = table[Row, Column].Piece;
            table[Row, Column].Piece = null;
            Row = toRow;
            Column = toColumn;
        }

        #region IClonable

        public override object Clone() => new Lotus(Row, Column, Player) { Revealed = Revealed };

        #endregion
    }
}
