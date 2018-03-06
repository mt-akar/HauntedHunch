using System;
using System.Windows.Media;

namespace HauntedHunch.Pieces
{
    public class Courier : Piece
    {
        // Moves to adjacent squares. Can push and pull other opponent pieces.

        private static readonly int[,] a = new int[4, 2] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };
        public static int[,] A { get { return a; } }

        public Courier(int r, int c, bool p)
        {
            row = r;
            column = c;
            player = p;
            frozen = false;
        }

        public override void PossibleMoves(ref Square[,] table, int turnDup)
        {
            table[row, column].BackgroundColor.Color = MainWindow.possible_move_color;
            if (frozen) return;

            for (int i = 0; i < 4; i++)
            {
                // In bounds & (empty square | psuedo piece)
                if (row + a[i, 0] <= 7 && row + a[i, 0] >= 1 && column + a[i, 1] <= 5 && column + a[i, 1] >= 1 && (table[row + a[i, 0], column + a[i, 1]].Piece == null ||
                    table[row + a[i, 0], column + a[i, 1]].Piece == table[row + a[i, 0], column + a[i, 1]].PsuedoPiece))
                {
                    table[row + a[i, 0], column + a[i, 1]].BackgroundColor.Color = MainWindow.possible_move_color;
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
                if (row + b[i, 0] <= 7 && row + b[i, 0] >= 1 && column + b[i, 1] <= 5 && column + b[i, 1] >= 1 &&
                    row + b[i, 2] <= 7 && row + b[i, 2] >= 1 && column + b[i, 3] <= 5 && column + b[i, 3] >= 1 &&
                    table[row + b[i, 0], column + b[i, 1]].Piece != null && table[row + b[i, 0], column + b[i, 1]].Piece.Player != player &&
                    table[row + b[i, 0], column + b[i, 1]].Piece != table[row + b[i, 0], column + b[i, 1]].PsuedoPiece &&
                    (table[row + b[i, 2], column + b[i, 3]].Piece == null || table[row + b[i, 2], column + b[i, 3]].Piece == table[row + b[i, 2], column + b[i, 3]].PsuedoPiece))
                {
                    table[row + b[i, 0], column + b[i, 1]].BackgroundColor.Color = MainWindow.AbilityWithInteracter_indicator_color;
                }
            }
        }

        public override void Move(ref Square[,] table, int to_row, int to_column, ref int turn)
        {
            PaintToDefault(ref table, row, column, a, 4);

            turn++;

            table[to_row, to_column].Image = table[row, column].Image;
            table[to_row, to_column].Piece = table[row, column].Piece;
            table[row, column].Image = emptyImage;
            table[row, column].Piece = null;
            row = to_row;
            column = to_column;
        }

        // Intermediate step. Paint second step squares. Returns interacter's square.
        public override Square AbilityWithInteracterStageOne(ref Square[,] table, ref Square sen)
        {
            table[row, column].BackgroundColor.Color = DefaultColor(row, column);
            sen.BackgroundColor.Color = DefaultColor(sen.Row, sen.Column);

            for (int i = 0; i < 4; i++)
            {
                // For pull, in bounds & (empty square | psuedo piece)
                if (row + a[i, 0] <= 7 && row + a[i, 0] >= 1 && column + a[i, 1] <= 5 && column + a[i, 1] >= 1 &&
                    (table[row + a[i, 0], column + a[i, 1]].Piece == null || table[row + a[i, 0], column + a[i, 1]].Piece == table[row + a[i, 0], column + a[i, 1]].PsuedoPiece))
                {
                    table[row + a[i, 0], column + a[i, 1]].BackgroundColor.Color = Color.FromArgb(255, 255, 100, 100);
                }
                // For push, in bounds & (empty square | psuedo piece)
                if (sen.Row + a[i, 0] <= 7 && sen.Row + a[i, 0] >= 1 && sen.Column + a[i, 1] <= 5 && sen.Column + a[i, 1] >= 1 &&
                    (table[sen.Row + a[i, 0], sen.Column + a[i, 1]].Piece == null || table[sen.Row + a[i, 0], sen.Column + a[i, 1]].Piece == table[sen.Row + a[i, 0], sen.Column + a[i, 1]].PsuedoPiece))
                {
                    table[sen.Row + a[i, 0], sen.Column + a[i, 1]].BackgroundColor.Color = Color.FromArgb(255, 255, 100, 100);
                }
            }
            return sen;
        }

        // Do the actual pulling or pushing.
        public override void AbilityWithInteracterStageTwo(ref Square[,] table, ref Square interacter, ref Square sen, ref int turn)
        {
            PaintToDefault(ref table, row, column, a, 4);
            PaintToDefault(ref table, interacter.Row, interacter.Column, a, 4);

            turn++;

            if (interacter.Piece.GetType() == typeof(MindController))
            {
                turn += (turn % 2 == 1) ? 2 : 3;

                player = !player;
                table[row, column].SetImageAccordingToPiece();

                if (Math.Abs(sen.Row - row) + Math.Abs(sen.Column - column) == 2) // Push
                {
                    interacter.Image = table[row, column].Image;
                    interacter.Piece = table[row, column].Piece;
                    table[row, column].Image = emptyImage;
                    table[row, column].Piece = null;
                    interacter.Piece.Row = interacter.Row;
                    interacter.Piece.Column = interacter.Column;
                }
                else // Pull
                {
                    sen.Image = table[row, column].Image;
                    sen.Piece = table[row, column].Piece;
                    table[row, column].Image = emptyImage;
                    table[row, column].Piece = null;
                    interacter.Image = emptyImage;
                    interacter.Piece = null;
                    sen.Piece.Row = sen.Row;
                    sen.Piece.Column = sen.Column;
                }
            }
            else
            {
                if (Math.Abs(sen.Row - row) + Math.Abs(sen.Column - column) == 2) // Push
                {
                    sen.Image = interacter.Image;
                    sen.Piece = interacter.Piece;
                    interacter.Image = table[row, column].Image;
                    interacter.Piece = table[row, column].Piece;
                    table[row, column].Image = emptyImage;
                    table[row, column].Piece = null;

                    sen.Piece.Row = sen.Row;
                    sen.Piece.Column = sen.Column;
                    interacter.Piece.Row = interacter.Row;
                    interacter.Piece.Column = interacter.Column;
                }
                else // Pull
                {
                    sen.Image = table[row, column].Image;
                    sen.Piece = table[row, column].Piece;
                    table[row, column].Image = interacter.Image;
                    table[row, column].Piece = interacter.Piece;
                    interacter.Image = emptyImage;
                    interacter.Piece = null;

                    /* If we swap the order of following line couples, since "row" is the row of courier which is now sen.Piece, table[row, column] is not interacter's new place.
                     * Instead it is cur's new place so interacter's coordinates will not be updated. */
                    table[row, column].Piece.Row = row;
                    table[row, column].Piece.Column = column;
                    sen.Piece.Row = sen.Row;
                    sen.Piece.Column = sen.Column;
                }
            }
        }
    }
}
