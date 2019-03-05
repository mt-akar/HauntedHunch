using System;

namespace HauntedHunch
{
    /// <summary>
    /// Is a psuedo piece. Refer to the manual.
    /// </summary>
    public class InnKeeper : Piece
    {
        public InnKeeper(int r, int c, PlayerType p) : base(r, c, p) { }

        public override void PossibleMoves(SquareViewModel[,] table, int turn)
        {
            // Paint the square that piece is on so that the game feels responsive when you do not have any possible moves.
            table[Row, Column].State = SquareState.ChosenPiece;

            // Frozen check
            if (IsFrozen(table, Row, Column)) return;

            for (int i = 0; i < 8; i++)
                // In bounds & (short range | in between is empty) && (empty square | opponent Lotus)
                if (Row + l[i, 0] <= nr && Row + l[i, 0] >= 1 && Column + l[i, 1] <= nc && Column + l[i, 1] >= 1 &&
                    (i <= 3 || i >= 4 && table[Row + l[i, 0] / 2, Column + l[i, 1] / 2].Piece == null) &&
                    (table[Row + l[i, 0], Column + l[i, 1]].Piece == null ||
                    table[Row + l[i, 0], Column + l[i, 1]].Piece is Lotus && table[Row + l[i, 0], Column + l[i, 1]].Piece.Player != Player && turn % 2 == 1))
                {
                    table[Row + l[i, 0], Column + l[i, 1]].State = SquareState.Moveable;
                }
        }

        public override void Move(SquareViewModel[,] table, int toRow, int toColumn, ref int turn)
        {
            ClearSquareStates(table, Row, Column, l);

            if (Math.Abs(Row + Column - toRow - toColumn) == 2)
                Revealed = true;

            if (IsHiddenlyFrozen(table, Row, Column)) return;

            turn += table[toRow, toColumn].Piece == null ? 1 : 2;
            
            table[toRow, toColumn].Piece = table[Row, Column].Piece;
            table[Row, Column].PseudoPiece = null;
            table[Row, Column].Piece = null;
            Row = toRow;
            Column = toColumn;
        }

        #region IClonable

        public override object Clone() => new InnKeeper(Row, Column, Player) { Revealed = Revealed };

        #endregion
    }
}
