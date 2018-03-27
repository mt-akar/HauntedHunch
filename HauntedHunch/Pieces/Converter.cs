namespace HauntedHunch
{
    /// <summary>
    /// Moves to adjacent squares. Can change the position of the pieces around it.
    /// </summary>
    public class Converter : Piece
    {
        public Converter(int r, int c, PlayerType p) : base(r, c, p) { }

        public override void PossibleMoves(Square[,] table, int turn)
        {
            // Paint the square that piece is on so that the game feels responsive when you do not have any possible moves.
            table[Row, Column].BackgroundColor.Color = BoardHelper.standartMoveColor;

            // Frozen check
            if (IsFrozen(table, Row, Column)) return;

            table[Row, Column].BackgroundColor.Color = BoardHelper.abilityUnoColor; // Convert

            for (int i = 0; i < 4; i++)
            {
                // In bounds & (empty square | psuedo piece)
                if (Row + e[i, 0] <= 7 && Row + e[i, 0] >= 1 && Column + e[i, 1] <= 5 && Column + e[i, 1] >= 1 && (table[Row + e[i, 0], Column + e[i, 1]].Piece == null ||
                    table[Row + e[i, 0], Column + e[i, 1]].Piece == table[Row + e[i, 0], Column + e[i, 1]].PsuedoPiece))
                {
                    table[Row + e[i, 0], Column + e[i, 1]].BackgroundColor.Color = BoardHelper.standartMoveColor;
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

        // Convert
        public override void AbilityUno(Square[,] table, ref int turn)
        {
            PaintToDefault(table, Row, Column, e);

            turn++;

            // 8-adjacency range, memory waste for understadable code
            int[,] rot = { { 1, 0 }, { 0, 1 }, { 1, 1 }, { -1, 1 }, { -1, 0 }, { 0, -1 }, { -1, -1 }, { 1, -1 } };
            
            // MindController check, loops 8 times
            for (int i = 0; i < 8; i++)
            {
                if (!(Row + rot[i, 0] > 7 || Row + rot[i, 0] < 1 || Column + rot[i, 1] > 5 || Column + rot[i, 1] < 1) &&
                    table[Row + rot[i, 0], Column + rot[i, 1]].Piece != null && table[Row + rot[i, 0], Column + rot[i, 1]].Piece.Player != Player &&
                    table[Row + rot[i, 0], Column + rot[i, 1]].Piece is MindController)
                {
                    turn += (turn % 2 == 1) ? 2 : 3;

                    Player = 1 - Player;
                    table[Row, Column].SetImageAccordingToPiece();
                    
                    table[Row + rot[i, 0], Column + rot[i, 1]].Piece = null;

                    break;
                }
            }

            // Convert, loops 4 times
            for (int i = 0; i < 4; i++)
            {
                // Check if places to be converted are on bounds
                bool b1 = Row + rot[i, 0] > 7 || Row + rot[i, 0] < 1 || Column + rot[i, 1] > 5 || Column + rot[i, 1] < 1;
                bool b2 = Row - rot[i, 0] > 7 || Row - rot[i, 0] < 1 || Column - rot[i, 1] > 5 || Column - rot[i, 1] < 1;

                if(b1 && b2) { }
                else if (b1)
                {
                    table[Row - rot[i, 0], Column - rot[i, 1]].PsuedoPiece = null;
                    table[Row - rot[i, 0], Column - rot[i, 1]].Piece = null;
                }
                else if (b2)
                {
                    table[Row + rot[i, 0], Column + rot[i, 1]].PsuedoPiece = null;
                    table[Row + rot[i, 0], Column + rot[i, 1]].Piece = null;
                }
                else
                {
                    Piece tempPsuedoPiece = table[Row + rot[i, 0], Column + rot[i, 1]].PsuedoPiece;
                    Piece tempPiece = table[Row + rot[i, 0], Column + rot[i, 1]].Piece;
                    table[Row + rot[i, 0], Column + rot[i, 1]].PsuedoPiece = table[Row - rot[i, 0], Column - rot[i, 1]].PsuedoPiece;
                    table[Row + rot[i, 0], Column + rot[i, 1]].Piece = table[Row - rot[i, 0], Column - rot[i, 1]].Piece;
                    table[Row - rot[i, 0], Column - rot[i, 1]].PsuedoPiece = tempPsuedoPiece;
                    table[Row - rot[i, 0], Column - rot[i, 1]].Piece = tempPiece;
                    if (table[Row + rot[i, 0], Column + rot[i, 1]].Piece != null)
                    {
                        table[Row + rot[i, 0], Column + rot[i, 1]].Piece.Row = Row + rot[i, 0];
                        table[Row + rot[i, 0], Column + rot[i, 1]].Piece.Column = Column + rot[i, 1];
                    }
                    if (table[Row - rot[i, 0], Column - rot[i, 1]].Piece != null)
                    {
                        table[Row - rot[i, 0], Column - rot[i, 1]].Piece.Row = Row - rot[i, 0];
                        table[Row - rot[i, 0], Column - rot[i, 1]].Piece.Column = Column - rot[i, 1];
                    }
                }
            }
        }
    }
}
