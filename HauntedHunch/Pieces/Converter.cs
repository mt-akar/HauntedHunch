using System.Windows.Media;

namespace HauntedHunch.Pieces
{
    public class Converter : Piece
    {
        // Moves to adjacent squares. Can change the position of the pieces around it. For more information look: Arimaa

        private static readonly int[,] a = new int[4, 2] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };
        public static int[,] A { get { return a; } }

        public Converter(int r, int c, bool p)
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
            table[row, column].BackgroundColor.Color = MainWindow.AbilityUno_indicator_color; // Convert

            for (int i = 0; i < 4; i++)
            {
                // In bounds & (empty square | psuedo piece)
                if (row + a[i, 0] <= 7 && row + a[i, 0] >= 1 && column + a[i, 1] <= 5 && column + a[i, 1] >= 1 && (table[row + a[i, 0], column + a[i, 1]].Piece == null ||
                    table[row + a[i, 0], column + a[i, 1]].Piece == table[row + a[i, 0], column + a[i, 1]].PsuedoPiece))
                {
                    table[row + a[i, 0], column + a[i, 1]].BackgroundColor.Color = MainWindow.possible_move_color;
                }
            }
        }

        public override void Move(ref Square[,] table, int to_row, int to_column, ref int turn)
        {
            turn++;

            table[to_row, to_column].Piece = table[row, column].Piece;
            table[to_row, to_column].Image = table[row, column].Image;
            table[row, column].Piece = null;
            table[row, column].Image = emptyImage;
            row = to_row;
            column = to_column;
        }

        // Convert
        public override void AbilityUno(ref Square[,] table, ref int turn)
        {
            turn++;

            // 8-adjacency range, memory waste for understadable code
            int[,] rot = { { 1, 0 }, { 0, 1 }, { 1, 1 }, { -1, 1 }, { -1, 0 }, { 0, -1 }, { -1, -1 }, { 1, -1 } };
            
            // MindController check, loops 8 times
            for (int i = 0; i < 8; i++)
            {
                if (!(row + rot[i, 0] > 7 || row + rot[i, 0] < 1 || column + rot[i, 1] > 5 || column + rot[i, 1] < 1) &&
                    table[row + rot[i, 0], column + rot[i, 1]].Piece != null && table[row + rot[i, 0], column + rot[i, 1]].Piece.Player != player &&
                    table[row + rot[i, 0], column + rot[i, 1]].Piece.GetType() == typeof(MindController))
                {
                    turn += (turn % 2 == 1) ? 2 : 3;

                    player = !player;
                    table[row, column].SetImageAccordingToPiece();

                    table[row + rot[i, 0], column + rot[i, 1]].Image = emptyImage;
                    table[row + rot[i, 0], column + rot[i, 1]].Piece = null;

                    break;
                }
            }

            // Convert, loops 4 times
            for (int i = 0; i < 4; i++)
            {
                // Check if places to be converted are on bounds
                bool b1 = row + rot[i, 0] > 7 || row + rot[i, 0] < 1 || column + rot[i, 1] > 5 || column + rot[i, 1] < 1;
                bool b2 = row - rot[i, 0] > 7 || row - rot[i, 0] < 1 || column - rot[i, 1] > 5 || column - rot[i, 1] < 1;

                if(b1 && b2) { }
                else if (b1)
                {
                    table[row - rot[i, 0], column - rot[i, 1]].PsuedoPiece = null;
                    table[row - rot[i, 0], column - rot[i, 1]].Image = emptyImage;
                    table[row - rot[i, 0], column - rot[i, 1]].Piece = null;
                }
                else if (b2)
                {
                    table[row + rot[i, 0], column + rot[i, 1]].PsuedoPiece = null;
                    table[row + rot[i, 0], column + rot[i, 1]].Image = emptyImage;
                    table[row + rot[i, 0], column + rot[i, 1]].Piece = null;
                }
                else
                {
                    Piece tempPsuedoPiece = table[row + rot[i, 0], column + rot[i, 1]].PsuedoPiece;
                    ImageSource tempImage = table[row + rot[i, 0], column + rot[i, 1]].Image;
                    Piece tempPiece = table[row + rot[i, 0], column + rot[i, 1]].Piece;
                    table[row + rot[i, 0], column + rot[i, 1]].PsuedoPiece = table[row - rot[i, 0], column - rot[i, 1]].PsuedoPiece;
                    table[row + rot[i, 0], column + rot[i, 1]].Image = table[row - rot[i, 0], column - rot[i, 1]].Image;
                    table[row + rot[i, 0], column + rot[i, 1]].Piece = table[row - rot[i, 0], column - rot[i, 1]].Piece;
                    table[row - rot[i, 0], column - rot[i, 1]].PsuedoPiece = tempPsuedoPiece;
                    table[row - rot[i, 0], column - rot[i, 1]].Image = tempImage;
                    table[row - rot[i, 0], column - rot[i, 1]].Piece = tempPiece;
                    if (table[row + rot[i, 0], column + rot[i, 1]].Piece != null)
                    {
                        table[row + rot[i, 0], column + rot[i, 1]].Piece.Row = row + rot[i, 0];
                        table[row + rot[i, 0], column + rot[i, 1]].Piece.Column = column + rot[i, 1];
                    }
                    if (table[row - rot[i, 0], column - rot[i, 1]].Piece != null)
                    {
                        table[row - rot[i, 0], column - rot[i, 1]].Piece.Row = row - rot[i, 0];
                        table[row - rot[i, 0], column - rot[i, 1]].Piece.Column = column - rot[i, 1];
                    }
                }

                /*
                // Edge cases
                if ((row + rot[i, 0] > 7 || row + rot[i, 0] < 1 || column + rot[i, 1] > 5 || column + rot[i, 1] < 1) &&
                    row - rot[i, 0] <= 7 && row - rot[i, 0] >= 1 && column - rot[i, 1] <= 5 && column - rot[i, 1] >= 1)
                {
                    table[row - rot[i, 0], column - rot[i, 1]].PsuedoPiece = null;
                    table[row - rot[i, 0], column - rot[i, 1]].Image = emptyImage;
                    table[row - rot[i, 0], column - rot[i, 1]].Piece = null;
                }
                else if ((row - rot[i, 0] > 7 || row - rot[i, 0] < 1 || column - rot[i, 1] > 5 || column - rot[i, 1] < 1) &&
                    row + rot[i, 0] <= 7 && row + rot[i, 0] >= 1 && column + rot[i, 1] <= 5 && column + rot[i, 1] >= 1)
                {
                    table[row + rot[i, 0], column + rot[i, 1]].PsuedoPiece = null;
                    table[row + rot[i, 0], column + rot[i, 1]].Image = emptyImage;
                    table[row + rot[i, 0], column + rot[i, 1]].Piece = null;
                }
                // Corner case
                else if ((row + rot[i, 0] > 7 || row + rot[i, 0] < 1 || column + rot[i, 1] > 5 || column + rot[i, 1] < 1) &&
                    (row - rot[i, 0] > 7 || row - rot[i, 0] < 1 || column - rot[i, 1] > 5 || column - rot[i, 1] < 1))
                {
                    // Do nothing   
                }
                // Full swap
                else
                {
                    Piece tempPsuedoPiece = table[row + rot[i, 0], column + rot[i, 1]].PsuedoPiece;
                    ImageSource tempImage = table[row + rot[i, 0], column + rot[i, 1]].Image;
                    Piece tempPiece = table[row + rot[i, 0], column + rot[i, 1]].Piece;
                    table[row + rot[i, 0], column + rot[i, 1]].PsuedoPiece = table[row - rot[i, 0], column - rot[i, 1]].PsuedoPiece;
                    table[row + rot[i, 0], column + rot[i, 1]].Image = table[row - rot[i, 0], column - rot[i, 1]].Image;
                    table[row + rot[i, 0], column + rot[i, 1]].Piece = table[row - rot[i, 0], column - rot[i, 1]].Piece;
                    table[row - rot[i, 0], column - rot[i, 1]].PsuedoPiece = tempPsuedoPiece;
                    table[row - rot[i, 0], column - rot[i, 1]].Image = tempImage;
                    table[row - rot[i, 0], column - rot[i, 1]].Piece = tempPiece;
                    if (table[row + rot[i, 0], column + rot[i, 1]].Piece != null)
                    {
                        table[row + rot[i, 0], column + rot[i, 1]].Piece.Row = row + rot[i, 0];
                        table[row + rot[i, 0], column + rot[i, 1]].Piece.Column = column + rot[i, 1];
                    }
                    if (table[row - rot[i, 0], column - rot[i, 1]].Piece != null)
                    {
                        table[row - rot[i, 0], column - rot[i, 1]].Piece.Row = row - rot[i, 0];
                        table[row - rot[i, 0], column - rot[i, 1]].Piece.Column = column - rot[i, 1];
                    }
                }
                */
            }
        } // AbilityUno
    }
}
