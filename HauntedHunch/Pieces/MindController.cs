namespace HauntedHunch
{
    /// <summary>
    /// Takes control of any piece that tries to interact with this piece, disapperaing in the process.
    /// </summary>
    public class MindController : Piece
    {
        public MindController(int r, int c, PlayerType p) : base(r, c, p) { }

        override public void PossibleMoves(Square[,] table, int turn)
        {
            // Paint the square that piece is on so that the game feels responsive when you do not have any possible moves.
            table[Row, Column].BackgroundColor = BoardHelper.standartMoveColor;

            for (int i = 0; i < 4; i++)
            {
                // In bounds & (empty square | psuedo piece)
                if (Row + e[i, 0] <= nr && Row + e[i, 0] >= 1 && Column + e[i, 1] <= nc && Column + e[i, 1] >= 1 &&

                    // empty square
                    (table[Row + e[i, 0], Column + e[i, 1]].Piece == null ||

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

            // Move
            if (table[to_row, to_column].Piece == null || table[to_row, to_column].Piece == table[to_row, to_column].PsuedoPiece)
            {
                turn++;

                table[to_row, to_column].Piece = table[Row, Column].Piece;
                table[Row, Column].Piece = null;

                Row = to_row;
                Column = to_column;
            }

            // Control the freezer
            bool controlFreezer = false;
            int i = 0;
            for (; i < 4; i++)
            {
                if (Row + e[i, 0] <= nr && Row + e[i, 0] >= 1 && Column + e[i, 1] <= nc && Column + e[i, 1] >= 1 &&
                    table[Row + e[i, 0], Column + e[i, 1]].Piece != null &&
                    table[Row + e[i, 0], Column + e[i, 1]].Piece is Freezer &&
                    table[Row + e[i, 0], Column + e[i, 1]].Piece.Player != table[Row, Column].Piece.Player)
                {
                    controlFreezer = true;
                    break;
                }
            }
            if (controlFreezer)
            {
                table[Row + e[i, 0], Column + e[i, 1]].Piece.Player = table[Row + e[i, 0], Column + e[i, 1]].Piece.Player == PlayerType.White ? PlayerType.Black : PlayerType.White;
                table[Row + e[i, 0], Column + e[i, 1]].SetImageAccordingToPiece();
                table[Row, Column].Piece = null;
            }
        }

        #region IClonable

        public override object Clone() => new MindController(Row, Column, Player);

        #endregion
    }
}
