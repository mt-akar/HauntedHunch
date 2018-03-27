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
            table[Row, Column].BackgroundColor.Color = BoardHelper.standartMoveColor;

            // Frozen check
            for (int i = 0; i < 4; i++)
            {
                if (Row + e[i, 0] <= 7 && Row + e[i, 0] >= 1 && Column + e[i, 1] <= 5 && Column + e[i, 1] >= 1 &&
                    table[Row + e[i, 0], Column + e[i, 1]].Piece != null &&
                    table[Row + e[i, 0], Column + e[i, 1]].Piece.GetType() == typeof(Freezer) &&
                    table[Row + e[i, 0], Column + e[i, 1]].Piece.Player != table[Row, Column].Piece.Player)
                {
                    return;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                // In bounds & (empty square | opponenet piece | psuedo piece)
                if (Row + e[i, 0] <= 7 && Row + e[i, 0] >= 1 && Column + e[i, 1] <= 5 && Column + e[i, 1] >= 1 &&
                    (table[Row + e[i, 0], Column + e[i, 1]].Piece == null || (table[Row + e[i, 0], Column + e[i, 1]].Piece.Player != Player && turn % 2 == 1) ||
                    table[Row + e[i, 0], Column + e[i, 1]].Piece == table[Row + e[i, 0], Column + e[i, 1]].PsuedoPiece))
                {
                    table[Row + e[i, 0], Column + e[i, 1]].BackgroundColor.Color = BoardHelper.standartMoveColor;
                }
            }
        }

        public override void Move(Square[,] table, int to_row, int to_column, ref int turn)
        {
            PaintToDefault(table, Row, Column, e);

            if (table[to_row, to_column].Piece == null || table[to_row, to_column].Piece == table[to_row, to_column].PsuedoPiece) // Move
            {
                turn++;

                table[to_row, to_column].Piece = table[Row, Column].Piece;
                table[Row, Column].Piece = null;

                Row = to_row;
                Column = to_column;
            }
            else // Mind control
            {
                turn += 2;

                if (table[to_row, to_column].Piece is MindController)
                {
                    table[to_row, to_column].Piece = null;
                    table[Row, Column].Piece = null;
                }
                else
                {
                    table[to_row, to_column].Piece.Player = 1 - table[to_row, to_column].Piece.Player;
                    table[to_row, to_column].SetImageAccordingToPiece();
                }
                table[Row, Column].Piece = null;
            }
        }
    }
}
