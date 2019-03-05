namespace HauntedHunch
{
    /// <summary>
    /// Moves to adjacent squares. Can change the position of the pieces around it.
    /// </summary>
    public class Converter : Piece
    {
        public Converter(int r, int c, PlayerType p) : base(r, c, p) { }

        public override void PossibleMoves(SquareViewModel[,] table, int turn)
        {
            // Paint the square that piece is on so that the game feels responsive when you do not have any possible moves.
            table[Row, Column].State = SquareState.ChosenPiece;

            // Frozen check
            if (IsFrozen(table, Row, Column)) return;

            table[Row, Column].State = SquareState.AbilityUnoable; // Convert

            for (int i = 0; i < 4; i++)
                // In bounds & (empty square | psuedo piece)
                if (Row + e[i, 0] <= nr && Row + e[i, 0] >= 1 && Column + e[i, 1] <= nc && Column + e[i, 1] >= 1 && (table[Row + e[i, 0], Column + e[i, 1]].Piece == null ||
                    table[Row + e[i, 0], Column + e[i, 1]].Piece == table[Row + e[i, 0], Column + e[i, 1]].PseudoPiece))
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

        // Convert
        public override void AbilityUno(SquareViewModel[,] table, ref int turn)
        {
            ClearSquareStates(table, Row, Column, e);

            if (IsHiddenlyFrozen(table, Row, Column))
            {
                Revealed = true;
                return;
            }

            turn++;

            // 8-adjacency range, memory waste for understadable code
            int[,] rot = { { 1, 0 }, { 0, 1 }, { 1, 1 }, { -1, 1 }, { -1, 0 }, { 0, -1 }, { -1, -1 }, { 1, -1 } };

            // MindController check, loops 8 times
            for (int i = 0; i < 8; i++)
                if (!(Row + rot[i, 0] > nr || Row + rot[i, 0] < 1 || Column + rot[i, 1] > nc || Column + rot[i, 1] < 1) &&
                    table[Row + rot[i, 0], Column + rot[i, 1]].Piece != null && table[Row + rot[i, 0], Column + rot[i, 1]].Piece.Player != Player &&
                    table[Row + rot[i, 0], Column + rot[i, 1]].Piece is MindController)
                {
                    turn += (turn % 2 == 1) ? 2 : 3;

                    Player = 1 - Player;
                    table[Row, Column].SetImageAccordingToPiece();

                    table[Row + rot[i, 0], Column + rot[i, 1]].Piece = null;

                    break;
                }

            // Convert, loops 4 times
            for (int i = 0; i < 4; i++)
            {
                // Check if places to be converted are on bounds
                bool outOfRange1 = Row + rot[i, 0] > nr || Row + rot[i, 0] < 1 || Column + rot[i, 1] > nc || Column + rot[i, 1] < 1;
                bool outOfRange2 = Row - rot[i, 0] > nr || Row - rot[i, 0] < 1 || Column - rot[i, 1] > nc || Column - rot[i, 1] < 1;

                if (outOfRange1 && outOfRange2) { }
                else if (outOfRange1)
                {
                    table[Row - rot[i, 0], Column - rot[i, 1]].PseudoPiece = null;
                    table[Row - rot[i, 0], Column - rot[i, 1]].Piece = null;
                }
                else if (outOfRange2)
                {
                    table[Row + rot[i, 0], Column + rot[i, 1]].PseudoPiece = null;
                    table[Row + rot[i, 0], Column + rot[i, 1]].Piece = null;
                }
                else
                {
                    Piece tempPsuedoPiece = table[Row + rot[i, 0], Column + rot[i, 1]].PseudoPiece;
                    Piece tempPiece = table[Row + rot[i, 0], Column + rot[i, 1]].Piece;
                    table[Row + rot[i, 0], Column + rot[i, 1]].PseudoPiece = table[Row - rot[i, 0], Column - rot[i, 1]].PseudoPiece;
                    table[Row + rot[i, 0], Column + rot[i, 1]].Piece = table[Row - rot[i, 0], Column - rot[i, 1]].Piece;
                    table[Row - rot[i, 0], Column - rot[i, 1]].PseudoPiece = tempPsuedoPiece;
                    table[Row - rot[i, 0], Column - rot[i, 1]].Piece = tempPiece;
                    if (table[Row + rot[i, 0], Column + rot[i, 1]].Piece != null)
                    {
                        table[Row + rot[i, 0], Column + rot[i, 1]].Piece.Row = Row + rot[i, 0];
                        table[Row + rot[i, 0], Column + rot[i, 1]].Piece.Column = Column + rot[i, 1];
                    }
                    if (table[Row - rot[i, 0], Column - rot[i, 1]].Piece != null)
                    {
                        table[Row - rot[i, 0], Column - rot[i, 1]].Piece.Row = Row - rot[i, 0];
                        table[Row - rot[i, 0], Column - rot[i, 1]].Piece.Column = Column - rot[i, 1];
                    }
                }
            }
        }

        #region IClonable

        public override object Clone() => new Converter(Row, Column, Player) { Revealed = Revealed };

        #endregion
    }
}
