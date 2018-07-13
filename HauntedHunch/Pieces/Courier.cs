using System;

namespace HauntedHunch
{
    /// <summary>
    /// Moves to adjacent squares. Can push and pull other opponent pieces.
    /// </summary>
    public class Courier : Piece
    {
        public Courier(int r, int c, PlayerType p) : base(r, c, p) { }

        public override void PossibleMoves(Square[,] table, int turn)
        {
            // Paint the square that piece is on so that the game feels responsive when you do not have any possible moves.
            table[Row, Column].BackgroundColor = BoardHelper.standartMoveColor;

            // Frozen check
            if (IsFrozen(table, Row, Column)) return;

            for (int i = 0; i < 4; i++)
            {
                // In bounds & (empty square | psuedo piece)
                if (Row + e[i, 0] <= nr && Row + e[i, 0] >= 1 && Column + e[i, 1] <= nc && Column + e[i, 1] >= 1 && (table[Row + e[i, 0], Column + e[i, 1]].Piece == null ||
                    table[Row + e[i, 0], Column + e[i, 1]].Piece == table[Row + e[i, 0], Column + e[i, 1]].PsuedoPiece))
                {
                    table[Row + e[i, 0], Column + e[i, 1]].BackgroundColor = BoardHelper.standartMoveColor;
                }
            }

            // pull and push range { interacter_row, interacter_column, to_row, to_column }
            int[,] b = {
                { 1, 0, 1, -1 }, { 1, 0, 2, 0 }, { 1, 0, 1, 1 }, { 0, 1, 1, 1 }, { 0, 1, 0, 2 }, { 0, 1, -1, 1 },
                { -1, 0, -1, 1 }, { -1, 0, -2, 0 }, { -1, 0, -1, -1 },  { 0, -1, -1, -1 }, { 0, -1, 0, -2 }, { 0, -1, 1, -1 },
                { 1, 0, 0, 1 }, { 1, 0, -1, 0 }, { 1, 0, 0, -1 }, { 0, 1, -1, 0 }, { 0, 1, 0, -1 }, { 0, 1, 1, 0 },
                { -1, 0, 0, -1 }, { -1, 0, 1, 0 }, { -1, 0, 0, 1 }, { 0, -1, 1, 0 }, { 0, -1, 0, 1 }, { 0, -1, -1, 0 }
            };

            for (int i = 0; i < 24; i++)
            {
                // In bounds & interacter is opponenet piece, not psuedo & to is null or psuedo
                if (Row + b[i, 0] <= nr && Row + b[i, 0] >= 1 && Column + b[i, 1] <= nc && Column + b[i, 1] >= 1 &&
                    Row + b[i, 2] <= nr && Row + b[i, 2] >= 1 && Column + b[i, 3] <= nc && Column + b[i, 3] >= 1 &&
                    table[Row + b[i, 0], Column + b[i, 1]].Piece != null && table[Row + b[i, 0], Column + b[i, 1]].Piece.Player != Player &&
                    table[Row + b[i, 0], Column + b[i, 1]].Piece != table[Row + b[i, 0], Column + b[i, 1]].PsuedoPiece &&
                    (table[Row + b[i, 2], Column + b[i, 3]].Piece == null || table[Row + b[i, 2], Column + b[i, 3]].Piece == table[Row + b[i, 2], Column + b[i, 3]].PsuedoPiece))
                {
                    table[Row + b[i, 0], Column + b[i, 1]].BackgroundColor = BoardHelper.abilityWithInteracterColor;
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

        // Intermediate step. Paint second step squares. Returns interacter's square.
        public override Square AbilityWithInteracterStageOne(Square[,] table, ref Square sen)
        {
            table[Row, Column].BackgroundColor = BoardHelper.DefaultColor(Row, Column);
            sen.BackgroundColor = BoardHelper.DefaultColor(sen.Row, sen.Column);

            for (int i = 0; i < 4; i++)
            {
                // For pull, in bounds & (empty square | psuedo piece)
                if (Row + e[i, 0] <= nr && Row + e[i, 0] >= 1 && Column + e[i, 1] <= nc && Column + e[i, 1] >= 1 &&
                    (table[Row + e[i, 0], Column + e[i, 1]].Piece == null || table[Row + e[i, 0], Column + e[i, 1]].Piece == table[Row + e[i, 0], Column + e[i, 1]].PsuedoPiece))
                {
                    table[Row + e[i, 0], Column + e[i, 1]].BackgroundColor = BoardHelper.abilityWithInteracterColor;
                }
                // For push, in bounds & (empty square | psuedo piece)
                if (sen.Row + e[i, 0] <= nr && sen.Row + e[i, 0] >= 1 && sen.Column + e[i, 1] <= nc && sen.Column + e[i, 1] >= 1 &&
                    (table[sen.Row + e[i, 0], sen.Column + e[i, 1]].Piece == null || table[sen.Row + e[i, 0], sen.Column + e[i, 1]].Piece == table[sen.Row + e[i, 0], sen.Column + e[i, 1]].PsuedoPiece))
                {
                    table[sen.Row + e[i, 0], sen.Column + e[i, 1]].BackgroundColor = BoardHelper.abilityWithInteracterColor;
                }
            }
            return sen;
        }

        // Do the actual pulling or pushing.
        public override void AbilityWithInteracterStageTwo(Square[,] table, ref Square interacter, ref Square sen, ref int turn)
        {
            PaintToDefault(table, Row, Column, e);
            PaintToDefault(table, interacter.Row, interacter.Column, e);

            turn++;

            if (interacter.Piece is MindController)
            {
                turn += (turn % 2 == 1) ? 2 : 3;

                Player = 1 - Player;

                if (Math.Abs(sen.Row - Row) + Math.Abs(sen.Column - Column) == 2) // Push
                {
                    interacter.Piece = table[Row, Column].Piece;
                    table[Row, Column].Piece = null;

                    interacter.Piece.Row = interacter.Row;
                    interacter.Piece.Column = interacter.Column;
                }
                else // Pull
                {
                    sen.Piece = table[Row, Column].Piece;
                    table[Row, Column].Piece = null;
                    interacter.Piece = null;

                    sen.Piece.Row = sen.Row;
                    sen.Piece.Column = sen.Column;
                }
            }
            else
            {
                if (Math.Abs(sen.Row - Row) + Math.Abs(sen.Column - Column) == 2) // Push
                {
                    sen.Piece = interacter.Piece;
                    interacter.Piece = table[Row, Column].Piece;
                    table[Row, Column].Piece = null;

                    sen.Piece.Row = sen.Row;
                    sen.Piece.Column = sen.Column;
                    interacter.Piece.Row = interacter.Row;
                    interacter.Piece.Column = interacter.Column;
                }
                else // Pull
                {
                    sen.Piece = table[Row, Column].Piece;
                    table[Row, Column].Piece = interacter.Piece;
                    interacter.Piece = null;

                    /* If we swap the order of following line couples, since "row" is the row of courier which is now sen.Piece, table[row, column] is not interacter's new place.
                     * Instead it is cur's new place so interacter's coordinates will not be updated. */
                    table[Row, Column].Piece.Row = Row;
                    table[Row, Column].Piece.Column = Column;
                    sen.Piece.Row = sen.Row;
                    sen.Piece.Column = sen.Column;
                }
            }
        }

        #region IClonable

        public override object Clone() => new Courier(Row, Column, Player);

        #endregion
    }
}
