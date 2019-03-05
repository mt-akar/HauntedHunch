namespace HauntedHunch
{
    /// <summary>
    /// Moves to adjacent squares, freezes adjacent opponenet pieces. Doesn't get frozen.
    /// </summary>
    public class Freezer : Piece
    {
        public Freezer(int r, int c, PlayerType p) : base(r, c, p) { }

        public override void PossibleMoves(SquareViewModel[,] table, int turn)
        {
            // Paint the square that piece is on so that the game feels responsive when you do not have any possible moves.
            table[Row, Column].State = SquareState.ChosenPiece;

            // A Freezer can be frozen(as a state) but doesn't get effected by it.

            for (int i = 0; i < 4; i++)
                // In bounds & (empty square | pseudo piece)
                if (Row + e[i, 0] <= nr && Row + e[i, 0] >= 1 && Column + e[i, 1] <= nc && Column + e[i, 1] >= 1 && (table[Row + e[i, 0], Column + e[i, 1]].Piece == null ||
                    table[Row + e[i, 0], Column + e[i, 1]].Piece == table[Row + e[i, 0], Column + e[i, 1]].PseudoPiece))
                {
                    table[Row + e[i, 0], Column + e[i, 1]].State = SquareState.Moveable;
                }
        }

        public override void Move(SquareViewModel[,] table, int toRow, int toColumn, ref int turn)
        {
            ClearSquareStates(table, Row, Column, e);

            turn++;

            table[toRow, toColumn].Piece = table[Row, Column].Piece;
            table[Row, Column].Piece = null;
            Row = toRow;
            Column = toColumn;

            // Check for a opponent mind controller around it
            anotherMindController:
            for (int i = 0; i < 4; i++)
                if (Row + e[i, 0] <= nr && Row + e[i, 0] >= 1 && Column + e[i, 1] <= nc && Column + e[i, 1] >= 1 &&
                    table[Row + e[i, 0], Column + e[i, 1]].Piece != null &&
                    table[Row + e[i, 0], Column + e[i, 1]].Piece is MindController &&
                    table[Row + e[i, 0], Column + e[i, 1]].Piece.Player != table[Row, Column].Piece.Player)
                {
                    table[Row, Column].Piece.Player ^= PlayerType.Black;
                    table[Row, Column].Piece.Revealed = true;
                    table[Row, Column].SetImageAccordingToPiece();
                    table[Row + e[i, 0], Column + e[i, 1]].Piece = null;
                    goto anotherMindController; // Check for another mind controller, possibly initially friendly
                }
        }

        #region IClonable

        public override object Clone() => new Freezer(Row, Column, Player) { Revealed = Revealed };

        #endregion
    }
}
