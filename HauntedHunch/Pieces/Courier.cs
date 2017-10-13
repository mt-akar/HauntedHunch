using System;
using System.Windows.Media;

namespace HauntedHunch.Pieces
{
    public class Courier : Piece
    {
        // Moves to adjacent squares. Can push and pull other opponent pieces.

        public Courier(int r, int c, bool p)
        {
            row = r;
            column = c;
            player = p;
            frozen = false;
            a = new int[4, 2] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };
        }

        public override int[,] PossibleMoves(Square[,] tableDup, int turnDup)
        {
            if (frozen) { return new int[0, 0]; }

            int[,] pos = new int[28, 3];
            for (int i = 0; i < 4; i++)
            {
                if (row + a[i, 0] <= 7 && row + a[i, 0] >= 1 && column + a[i, 1] <= 5 && column + a[i, 1] >= 1 && (tableDup[row + a[i, 0], column + a[i, 1]].Piece == null ||
                    tableDup[row + a[i, 0], column + a[i, 1]].Piece == tableDup[row + a[i, 0], column + a[i, 1]].PsuedoPiece))
                {
                    pos[i, 0] = row + a[i, 0];
                    pos[i, 1] = column + a[i, 1];
                }
            }

            // Pull or push range
            int[,] b = {
                { 1, 0, 1, -1 }, { 1, 0, 2, 0 }, { 1, 0, 1, 1 }, { 0, 1, 1, 1 }, { 0, 1, 0, 2 }, { 0, 1, -1, 1 },
                { -1, 0, -1, 1 }, { -1, 0, -2, 0 }, { -1, 0, -1, -1 },  { 0, -1, -1, -1 }, { 0, -1, 0, -2 }, { 0, -1, 1, -1 },
                { 1, 0, 0, 1 }, { 1, 0, -1, 0 }, { 1, 0, 0, -1 }, { 0, 1, -1, 0 }, { 0, 1, 0, -1 }, { 0, 1, 1, 0 },
                { -1, 0, 0, -1 }, { -1, 0, 1, 0 }, { -1, 0, 0, 1 }, { 0, -1, 1, 0 }, { 0, -1, 0, 1 }, { 0, -1, -1, 0 }
            };
            for (int i = 0; i < 24; i++)
            {
                if (row + b[i, 0] <= 7 && row + b[i, 0] >= 1 && column + b[i, 1] <= 5 && column + b[i, 1] >= 1 &&
                    row + b[i, 2] <= 7 && row + b[i, 2] >= 1 && column + b[i, 3] <= 5 && column + b[i, 3] >= 1 &&
                    tableDup[row + b[i, 0], column + b[i, 1]].Piece != null && tableDup[row + b[i, 0], column + b[i, 1]].Piece.Player != player &&
                    tableDup[row + b[i, 2], column + b[i, 3]].Piece == null)
                {
                    pos[i + 4, 0] = row + b[i, 0];
                    pos[i + 4, 1] = column + b[i, 1];
                    pos[i + 4, 2] = 2;
                }
            }

            return pos;
        }

        public override void Move(ref Square[,] table, int to_row, int to_column, ref int turn)
        {
            turn++;

            table[to_row, to_column].Image = table[row, column].Image;
            table[to_row, to_column].Piece = table[row, column].Piece;
            table[row, column].Image = emptyImage;
            table[row, column].Piece = null;

            row = to_row;
            column = to_column;
        }

        // Pull and push
        public override int AbilityWithInteracter(ref Square[,] table, ref Square interacter, ref Square sen, ref int turn)
        {
            // Intermediate step. Set the interacter, piece that will be pulled or pushed.
            if (interacter == null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (row + a[i, 0] <= 7 && row + a[i, 0] >= 1 && column + a[i, 1] <= 5 && column + a[i, 1] >= 1 &&
                        table[row + a[i, 0], column + a[i, 1]].Piece == null)
                    {
                        table[row + a[i, 0], column + a[i, 1]].BackgroundColor.Color = Color.FromArgb(255, 255, 100, 100);
                    }
                    if (sen.Row + a[i, 0] <= 7 && sen.Row + a[i, 0] >= 1 && sen.Column + a[i, 1] <= 5 && sen.Column + a[i, 1] >= 1 &&
                        table[sen.Row + a[i, 0], sen.Column + a[i, 1]].Piece == null)
                    {
                        table[sen.Row + a[i, 0], sen.Column + a[i, 1]].BackgroundColor.Color = Color.FromArgb(255, 255, 100, 100);
                    }
                }
                return 1;
            }
            // Do the actual pulling or pushing.
            else
            {
                turn++;

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

                    table[row, column].Piece.Row = row;
                    table[row, column].Piece.Column = column;
                    sen.Piece.Row = sen.Row;
                    sen.Piece.Column = sen.Column;
                }
                return 2;
            }
        }
    }
}
