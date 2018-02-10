using System;

namespace HauntedHunch.Pieces
{
    public class Jumper : Piece
    {
        // The checkers piece which is able to move backwards and can't capture more than 1 piece at once.
        // TODO: Double capture jump

        private static readonly int[,] a = new int[8, 2] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 }, { 2, 0 }, { 0, 2 }, { -2, 0 }, { 0, -2 } };
        public static int[,] A { get { return a; } }

        public Jumper(int r, int c, bool p)
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

            for (int i = 0; i < 8; i++)
            {
                // In bounds & if capture, in between is opponent piece && (empty square | psuedo piece)
                if (row + a[i, 0] <= 7 && row + a[i, 0] >= 1 && column + a[i, 1] <= 5 && column + a[i, 1] >= 1 &&
                    (i <= 3 || (i >= 4 && table[row + a[i, 0] / 2, column + a[i, 1] / 2].Piece != null && table[row + a[i, 0] / 2, column + a[i, 1] / 2].Piece.Player != player &&
                    (table[row + a[i, 0] / 2, column + a[i, 1] / 2].Piece.GetType() == typeof(InnKeeper) || turnDup % 2 == 1))) &&
                    (table[row + a[i, 0], column + a[i, 1]].Piece == null || table[row + a[i, 0], column + a[i, 1]].Piece.GetType() == typeof(InnKeeper)))
                {
                    table[row + a[i, 0], column + a[i, 1]].BackgroundColor.Color = MainWindow.possible_move_color;
                }
            }
        }

        public override void Move(ref Square[,] table, int to_row, int to_column, ref int turn)
        {
            if (Math.Abs(row + column - to_row - to_column) == 1)
            {
                turn++;
            }
            else
            {
                turn += 2;

                if (table[(row + to_row) / 2, (column + to_column) / 2].Piece.GetType() == typeof(MindController))
                {
                    turn += 2;

                    player = !player;
                    table[row, column].SetImageAccordingToPiece();
                }

                table[(row + to_row) / 2, (column + to_column) / 2].Image = emptyImage;
                table[(row + to_row) / 2, (column + to_column) / 2].Piece = null;
            }

            table[to_row, to_column].Image = table[row, column].Image;
            table[to_row, to_column].Piece = table[row, column].Piece;
            table[row, column].Image = emptyImage;
            table[row, column].Piece = null;

            row = to_row;
            column = to_column;
        }
    }
}
