namespace HauntedHunch
{
    /// <summary>
    /// Moves to adjacent squares. Cannot move if it is in the range of any opponent piece.
    /// </summary>
    public class Lotus : Piece
    {
        // Ranges of pieces are defined with their member varable a. Any piece can interfere long ranges of Ranger and InnKeeper.
        // A Jumper doesn't freeze a lotus from 2 squares away even though it has long range.

        // TODO: Triple move

        public Lotus(int r, int c, PlayerType p) : base(r, c, p) { }

        override public void PossibleMoves(Square[,] table, int turn)
        {
            // Paint the square that piece is on so that the game feels responsive when you do not have any possible moves.
            table[Row, Column].BackgroundColor = BoardHelper.standartMoveColor;

            // Frozen lotus check.

            bool ableToMove = true;

            for (int i = 0; i < 4; i++)
            {
                // Never seen more booleans in a single if statement. No way to simplify.
                if ((Row + e[i, 0] <= nr && Row + e[i, 0] >= 1 && Column + e[i, 1] <= nc && Column + e[i, 1] >= 1 &&
                    table[Row + e[i, 0], Column + e[i, 1]].Piece != null && table[Row + e[i, 0], Column + e[i, 1]].Piece.Player != Player &&
                    (table[Row + e[i, 0], Column + e[i, 1]].Piece is Guard ||
                    table[Row + e[i, 0], Column + e[i, 1]].Piece is Jumper ||
                    table[Row + e[i, 0], Column + e[i, 1]].Piece is Freezer ||
                    table[Row + e[i, 0], Column + e[i, 1]].Piece is Converter ||
                    table[Row + e[i, 0], Column + e[i, 1]].Piece is Courier ||
                    table[Row + e[i, 0], Column + e[i, 1]].Piece is Boomer ||
                    table[Row + e[i, 0], Column + e[i, 1]].Piece is MindController)) ||

                    // Runner
                    (Row + j[i, 0] <= nr && Row + j[i, 0] >= 1 && Column + j[i, 1] <= nc && Column + j[i, 1] >= 1 &&
                    table[Row + j[i, 0], Column + j[i, 1]].Piece != null && table[Row + j[i, 0], Column + j[i, 1]].Piece.Player != Player && table[Row + j[i, 0], Column + j[i, 1]].Piece is Runner) ||
                    (Row + j[i + 4, 0] <= nr && Row + j[i + 4, 0] >= 1 && Column + j[i + 4, 1] <= nc && Column + j[i + 4, 1] >= 1 &&
                    table[Row + j[i + 4, 0], Column + j[i + 4, 1]].Piece != null && table[Row + j[i + 4, 0], Column + j[i + 4, 1]].Piece.Player != Player && table[Row + j[i + 4, 0], Column + j[i + 4, 1]].Piece is Runner) ||

                    // Ranger
                    (Row + c[i, 0] <= nr && Row + c[i, 0] >= 1 && Column + c[i, 1] <= nc && Column + c[i, 1] >= 1 &&
                    table[Row + c[i, 0], Column + c[i, 1]].Piece != null && table[Row + c[i, 0], Column + c[i, 1]].Piece.Player != Player && table[Row + c[i, 0], Column + c[i, 1]].Piece is Ranger) ||
                    (Row + c[i + 4, 0] <= nr && Row + c[i + 4, 0] >= 1 && Column + c[i + 4, 1] <= nc && Column + c[i + 4, 1] >= 1 &&
                    table[Row + c[i + 4, 0], Column + c[i + 4, 1]].Piece != null && table[Row + c[i + 4, 0], Column + c[i + 4, 1]].Piece.Player != Player && table[Row + c[i + 4, 0], Column + c[i + 4, 1]].Piece is Ranger &&
                    (table[Row + c[i + 4, 0] / 2, Column + c[i + 4, 1] / 2].Piece == null || (table[Row + c[i + 4, 0] / 2, Column + c[i + 4, 1] / 2].PsuedoPiece != null && table[Row + c[i + 4, 0] / 2, Column + c[i + 4, 1] / 2].PsuedoPiece == table[Row + c[i + 4, 0] / 2, Column + c[i + 4, 1] / 2].Piece))) ||

                    // Inn Keeper
                    (Row + l[i, 0] <= nr && Row + l[i, 0] >= 1 && Column + l[i, 1] <= nc && Column + l[i, 1] >= 1 &&
                    table[Row + l[i, 0], Column + l[i, 1]].Piece != null && table[Row + l[i, 0], Column + l[i, 1]].Piece.Player != Player && table[Row + l[i, 0], Column + l[i, 1]].Piece is InnKeeper) ||
                    (Row + l[i + 4, 0] <= nr && Row + l[i + 4, 0] >= 1 && Column + l[i + 4, 1] <= nc && Column + l[i + 4, 1] >= 1 &&
                    table[Row + l[i + 4, 0], Column + l[i + 4, 1]].Piece != null && table[Row + l[i + 4, 0], Column + l[i + 4, 1]].Piece.Player != Player && table[Row + l[i + 4, 0], Column + l[i + 4, 1]].Piece is InnKeeper && table[Row + l[i + 4, 0] / 2, Column + l[i + 4, 1] / 2].Piece == null))
                {
                    ableToMove = false;
                    break;
                }
            }

            // If able to move
            if (ableToMove)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (Row + e[i, 0] <= nr && Row + e[i, 0] >= 1 && Column + e[i, 1] <= nc && Column + e[i, 1] >= 1 && table[Row + e[i, 0], Column + e[i, 1]].Piece == null)
                        table[Row + e[i, 0], Column + e[i, 1]].BackgroundColor = BoardHelper.standartMoveColor;
                }
            }
        }

        public override void Move(Square[,] table, int to_row, int to_column, ref int turn)
        {
            PaintToDefault(table, Row, Column, e);

            turn++;
            
            table[to_row, to_column].Piece = table[Row, Column].Piece;
            table[Row, Column].Piece = null;

            Row = to_row;
            Column = to_column;
        }

        #region IClonable

        public override object Clone() => new Lotus(Row, Column, Player);

        #endregion
    }
}
