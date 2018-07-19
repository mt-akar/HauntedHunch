﻿namespace HauntedHunch
{
    /// <summary>
    /// Takes control of any piece that tries to interact with this piece, disapperaing in the process.
    /// </summary>
    public class MindController : Piece
    {
        public MindController(int r, int c, PlayerType p) : base(r, c, p) { }

        public override void PossibleMoves(Square[,] table, int turn)
        {
            // Paint the square that piece is on so that the game feels responsive when you do not have any possible moves.
            table[Row, Column].State = SquareState.ChosenPiece;

            // A mind controller can never be frozen

            for (int i = 0; i < 4; i++)
                // In bounds & (empty square | psuedo piece)
                if (Row + e[i, 0] <= nr && Row + e[i, 0] >= 1 && Column + e[i, 1] <= nc && Column + e[i, 1] >= 1 &&

                    // empty square
                    (table[Row + e[i, 0], Column + e[i, 1]].Piece == null ||

                    // psuedo piece
                    table[Row + e[i, 0], Column + e[i, 1]].Piece == table[Row + e[i, 0], Column + e[i, 1]].PsuedoPiece))
                {
                    table[Row + e[i, 0], Column + e[i, 1]].State = SquareState.Moveable;
                }
        }

        public override void Move(Square[,] table, int toRow, int toColumn, ref int turn)
        {
            ClearSquareStates(table, Row, Column, e);

            turn++;

            table[toRow, toColumn].Piece = table[Row, Column].Piece;
            table[Row, Column].Piece = null;
            Row = toRow;
            Column = toColumn;

            // Check for a opponent freezer around it
            for (int i = 0; i < 4; i++)
                if (Row + e[i, 0] <= nr && Row + e[i, 0] >= 1 && Column + e[i, 1] <= nc && Column + e[i, 1] >= 1 &&
                    table[Row + e[i, 0], Column + e[i, 1]].Piece != null &&
                    table[Row + e[i, 0], Column + e[i, 1]].Piece is Freezer &&
                    table[Row + e[i, 0], Column + e[i, 1]].Piece.Player != table[Row, Column].Piece.Player)
                {
                    table[Row + e[i, 0], Column + e[i, 1]].Piece.Player ^= PlayerType.Black;
                    table[Row + e[i, 0], Column + e[i, 1]].Piece.Revealed = true;
                    table[Row + e[i, 0], Column + e[i, 1]].SetImageAccordingToPiece();
                    table[Row, Column].Piece = null;

                    // Check for another opponent mind controller around mind controller freezer
                    for (int j = 0; j < 4; j++)
                        if (Row + e[i, 0] + e[j, 0] <= nr && Row + e[i, 0] + e[j, 0] >= 1 && Column + e[i, 1] + e[j, 1] <= nc && Column + e[i, 1] + e[j, 1] >= 1 &&
                            table[Row + e[i, 0] + e[j, 0], Column + e[i, 1] + e[j, 1]].Piece != null &&
                            table[Row + e[i, 0] + e[j, 0], Column + e[i, 1] + e[j, 1]].Piece is MindController &&
                            table[Row + e[i, 0] + e[j, 0], Column + e[i, 1] + e[j, 1]].Piece.Player != table[Row + e[i, 0], Column + e[i, 1]].Piece.Player)
                        {
                            table[Row + e[i, 0], Column + e[i, 1]].Piece.Player ^= PlayerType.Black;
                            table[Row + e[i, 0], Column + e[i, 1]].SetImageAccordingToPiece();
                            table[Row + e[i, 0] + e[j, 0], Column + e[i, 1] + e[j, 1]].Piece = null;
                        }

                }
        }

        #region IClonable

        public override object Clone() => new MindController(Row, Column, Player);

        #endregion
    }
}
