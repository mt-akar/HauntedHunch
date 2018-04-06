namespace HauntedHunch
{
    /// <summary>
    /// Is a psuedo piece. Refer to the manual.
    /// </summary>
    public class InnKeeper : Piece
    {
        public InnKeeper(int r, int c, PlayerType p) : base(r, c, p) { }

        override public void PossibleMoves(Square[,] table, int turn)
        {
            // Paint the square that piece is on so that the game feels responsive when you do not have any possible moves.
            table[Row, Column].BackgroundColor.Color = BoardHelper.standartMoveColor;

            // Frozen check
            if (IsFrozen(table, Row, Column)) return;

            for (int i = 0; i < 8; i++)
            {
                // In bounds & (short range | in between is empty) && (empty square | opponenet Lotus)
                if (Row + l[i, 0] <= nr && Row + l[i, 0] >= 1 && Column + l[i, 1] <= nc && Column + l[i, 1] >= 1 &&
                    (i <= 3 || (i >= 4 && table[Row + l[i, 0] / 2, Column + l[i, 1] / 2].Piece == null)) &&
                    (table[Row + l[i, 0], Column + l[i, 1]].Piece == null ||
                    (table[Row + l[i, 0], Column + l[i, 1]].Piece is Lotus && table[Row + l[i, 0], Column + l[i, 1]].Piece.Player != Player && turn % 2 == 1)))
                {
                    table[Row + l[i, 0], Column + l[i, 1]].BackgroundColor.Color = BoardHelper.standartMoveColor;
                }
            }
        }

        public override void Move(Square[,] table, int to_row, int to_column, ref int turn)
        {
            PaintToDefault(table, Row, Column, l);

            turn += table[to_row, to_column].Piece == null ? 1 : 2;
            
            table[to_row, to_column].PsuedoPiece = table[Row, Column].PsuedoPiece;
            table[to_row, to_column].Piece = table[Row, Column].Piece;
            table[Row, Column].PsuedoPiece = null;
            table[Row, Column].Piece = null;
            Row = to_row;
            Column = to_column;
        }

        #region IClonable

        public override object Clone() => new InnKeeper(Row, Column, Player);

        #endregion
    }
}
