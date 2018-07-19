namespace HauntedHunch
{
    /// <summary>
    /// Exact same piece with the Kinght in the chess.
    /// </summary>
    public class Runner : Piece
    {
        public Runner(int r, int c, PlayerType p) : base(r, c, p) { }

        public override void PossibleMoves(Square[,] table, int turn)
        {
            // Paint the square that piece is on so that the game feels responsive when you do not have any possible moves.
            table[Row, Column].State = SquareState.ChosenPiece;

            // Frozen check
            if (IsFrozen(table, Row, Column)) return;

            for (int i = 0; i < 8; i++)
                // In bounds & (empty square | opponenet piece | psuedo piece)
                if (Row + j[i, 0] <= nr && Row + j[i, 0] >= 1 && Column + j[i, 1] <= nc && Column + j[i, 1] >= 1 &&

                    // empty square
                    (table[Row + j[i, 0], Column + j[i, 1]].Piece == null ||

                    // opponenet piece
                    (table[Row + j[i, 0], Column + j[i, 1]].Piece.Player != Player && turn % 2 == 1) ||

                    // psuedo piece
                    table[Row + j[i, 0], Column + j[i, 1]].Piece == table[Row + j[i, 0], Column + j[i, 1]].PsuedoPiece))
                {
                    table[Row + j[i, 0], Column + j[i, 1]].State = SquareState.Moveable;
                }
        }

        public override void Move(Square[,] table, int toRow, int toColumn, ref int turn)
        {
            ClearSquareStates(table, Row, Column, j);

            Revealed = true;

            if (IsHiddenlyFrozen(table, Row, Column)) return;

            // Move
            if (table[toRow, toColumn].Piece == null || table[toRow, toColumn].Piece == table[toRow, toColumn].PsuedoPiece)
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

        public override object Clone() => new Runner(Row, Column, Player) { Revealed = Revealed };

        #endregion
    }
}