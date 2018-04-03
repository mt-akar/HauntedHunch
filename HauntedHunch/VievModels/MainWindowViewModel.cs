using System;
using System.Windows.Input;

namespace HauntedHunch
{
    public class MainWindowViewModel
    {
        #region Variables and Properties

        private static int nc = 6; // Number of columns
        private static int nr = 7; // Number of rows

        public Square[,] table; // Game board
        public Square[,,] history; // Game history, for undo
        public Square cur; // Current moving piece
        public Square interacter; // Interacting piece in moves where there is more than one piece involved
        public int turn;
        public bool placementStage;
        public bool gameEnded;
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////// Design time attribute, remove later /////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool turnConstraintsEnabled;
        ICommand DisableTurnConstraintsCommand { get; set; }

        #endregion

        #region Constructor

        public MainWindowViewModel()
        {
            table = new Square[nr + 1, nc + 1]; // Table is 7x5. Zero indexes are ignored for a better understanding of the coodinates, will always stay null.
            for (int i = 1; i <= nr; i++)
            {
                for (int j = 1; j <= nc; j++)
                {
                    if (i == 1 && j == 1) table[i, j] = new Square(i, j, new Guard(i, j, PlayerType.White));
                    else if (i == 1 && j == 2) table[i, j] = new Square(i, j, new Runner(i, j, PlayerType.White));
                    else if (i == 1 && j == 3) table[i, j] = new Square(i, j, new Ranger(i, j, PlayerType.White));
                    else if (i == 1 && j == 4) table[i, j] = new Square(i, j, new Jumper(i, j, PlayerType.White));
                    else if (i == 1 && j == 5) table[i, j] = new Square(i, j, new Lotus(i, j, PlayerType.White));
                    else if (i == 1 && j == 6) table[i, j] = new Square(i, j, new Guard(i, j, PlayerType.White));
                    else if (i == 2 && j == 1) table[i, j] = new Square(i, j, new Converter(i, j, PlayerType.White));
                    else if (i == 2 && j == 2) table[i, j] = new Square(i, j, new Courier(i, j, PlayerType.White));
                    else if (i == 2 && j == 3) table[i, j] = new Square(i, j, new Boomer(i, j, PlayerType.White));
                    else if (i == 2 && j == 4) { table[i, j] = new Square(i, j, new InnKeeper(i, j, PlayerType.White)); table[i, j].PsuedoPiece = table[i, j].Piece; }
                    else if (i == 2 && j == 5) table[i, j] = new Square(i, j, new Freezer(i, j, PlayerType.White));
                    else if (i == 2 && j == 6) table[i, j] = new Square(i, j, new MindController(i, j, PlayerType.White));
                    else if (i == 7 && j == 1) table[i, j] = new Square(i, j, new Guard(i, j, PlayerType.Black));
                    else if (i == 7 && j == 2) table[i, j] = new Square(i, j, new Runner(i, j, PlayerType.Black));
                    else if (i == 7 && j == 3) table[i, j] = new Square(i, j, new Ranger(i, j, PlayerType.Black));
                    else if (i == 7 && j == 4) table[i, j] = new Square(i, j, new Jumper(i, j, PlayerType.Black));
                    else if (i == 7 && j == 5) table[i, j] = new Square(i, j, new Lotus(i, j, PlayerType.Black));
                    else if (i == 7 && j == 6) table[i, j] = new Square(i, j, new Guard(i, j, PlayerType.Black));
                    else if (i == 6 && j == 1) table[i, j] = new Square(i, j, new Converter(i, j, PlayerType.Black));
                    else if (i == 6 && j == 2) table[i, j] = new Square(i, j, new Courier(i, j, PlayerType.Black));
                    else if (i == 6 && j == 3) table[i, j] = new Square(i, j, new Boomer(i, j, PlayerType.Black));
                    else if (i == 6 && j == 4) { table[i, j] = new Square(i, j, new InnKeeper(i, j, PlayerType.Black)); table[i, j].PsuedoPiece = table[i, j].Piece; }
                    else if (i == 6 && j == 5) table[i, j] = new Square(i, j, new Freezer(i, j, PlayerType.Black));
                    else if (i == 6 && j == 6) table[i, j] = new Square(i, j, new MindController(i, j, PlayerType.Black));
                    else table[i, j] = new Square(i, j, null);
                }
            } // Set up the initial board position.

            history = new Square[1000, nr + 1, nc + 1];
            UpdateHistory();

            cur = null;
            interacter = null;
            turn = 0; // 4k+3 & 4k are white's turns, 4k+1 & 4k+2 are black's turns.
            placementStage = true;
            gameEnded = false;
            turnConstraintsEnabled = true;

            DisableTurnConstraintsCommand = new RelayCommand(DisableTurnConstraints);
        }

        #endregion

        #region LMDown

        /// <summary>
        /// Most complicated methos of the project. Activated when clicked on a square.
        /// Does different things depending on the input and the state of the variables table, turn, cur and interacter.
        /// </summary>
        /// <param name="sen"> Sqaure that is just clicked </param>
        public void LMDown(Square sen)
        {
            // If we are at an in-between move of an ability with interacter
            if (interacter != null)
            {
                if (sen.BackgroundColor.Color == BoardHelper.abilityWithInteracterColor)
                {
                    cur.Piece.AbilityWithInteracterStageTwo(table, ref interacter, ref sen, ref turn);
                    UpdateHistory();
                    UpdatePits();
                }
                interacter = null;
                cur = null;
                return;
            }

            tunnel1: // Used when selecting a friendly piece to move while already had been selected a friendly piece to move

            if (cur == null) // If no piece is chosen yet
            {
                // If a valid piece is chosen, paint the possible moves for preview.
                if (sen.Piece != null && (!turnConstraintsEnabled ||
                    ((sen.Piece.Player == PlayerType.White && (turn % 4 == 0 || turn % 4 == 3)) || (sen.Piece.Player == PlayerType.Black && (turn % 4 == 1 || turn % 4 == 2)))))
                {
                    sen.Piece.PossibleMoves(table, turn);
                    cur = sen;
                }
                // If a non-valid square is chosen, do nothing
                else
                {
                    // We explicitly kick other (curr == null) cases, don't merge 2 if statements.
                }
            }
            else // (cur != null)
            {
                // Ability Uno
                if (sen.BackgroundColor.Color == BoardHelper.abilityUnoColor)
                {
                    cur.Piece.AbilityUno(table, ref turn);
                    UpdateHistory();

                    UpdatePits();
                    cur = null;
                }

                // Ability With Interacter
                else if (sen.BackgroundColor.Color == BoardHelper.abilityWithInteracterColor)
                {
                    interacter = cur.Piece.AbilityWithInteracterStageOne(table, ref sen);
                }

                // Standart Move
                else if (sen.BackgroundColor.Color == BoardHelper.standartMoveColor && cur != sen)
                {
                    cur.Piece.Move(table, sen.Row, sen.Column, ref turn);
                    UpdateHistory();

                    UpdatePits();
                    cur = null;
                }

                // If another friendly piece is chosen
                else if (cur != sen && sen.Piece != null && cur.Piece.Player == sen.Piece.Player)
                {
                    cur = null;
                    Repaint();
                    goto tunnel1; // Behave as if the cur was null and go back to the top of the LMDown method
                }

                // Unvalid square chosen
                else
                {
                    cur = null;
                    Repaint();
                }
            }

            // Update gameEnded
            bool whiteLotusIsOnBoard = false;
            bool blackLotusIsOnBoard = false;
            for (int i = 1; i <= nr; i++)
            {
                for (int j = 1; j <= nc; j++)
                {
                    if (table[i, j].Piece != null && table[i, j].Piece is Lotus)
                    {
                        // If either lotus is removed, game ends
                        if (table[i, j].Piece.Player == PlayerType.White)
                            whiteLotusIsOnBoard = true;
                        else
                            blackLotusIsOnBoard = true;

                        // If a lotus reaches the last row for either player, game ends.
                        if ((table[i, j].Piece.Player == PlayerType.White && i == nr) || (table[i, j].Piece.Player == PlayerType.Black && i == 1))
                            gameEnded = true;
                    }
                }
            }
            // If any lotus is removed, game ends.
            if (!whiteLotusIsOnBoard || !blackLotusIsOnBoard)
                gameEnded = true;
        }

        #endregion

        #region Tool methods

        /// <summary>
        /// Remove the pieces that are on the pits and that have no adjacent friendly piece from the game.
        /// </summary>
        public void UpdatePits()
        {
            int[,] pits = { { 3, 2 }, { 3, 5 }, { 5, 2 }, { 5, 5 } };
            for (int i = 0; i < 4; i++)
            {
                if (table[pits[i, 0], pits[i, 1]].Piece != null &&
                   (table[pits[i, 0] + 1, pits[i, 1]].Piece == null || table[pits[i, 0] + 1, pits[i, 1]].Piece.Player != table[pits[i, 0], pits[i, 1]].Piece.Player) &&
                   (table[pits[i, 0], pits[i, 1] + 1].Piece == null || table[pits[i, 0], pits[i, 1] + 1].Piece.Player != table[pits[i, 0], pits[i, 1]].Piece.Player) &&
                   (table[pits[i, 0] - 1, pits[i, 1]].Piece == null || table[pits[i, 0] - 1, pits[i, 1]].Piece.Player != table[pits[i, 0], pits[i, 1]].Piece.Player) &&
                   (table[pits[i, 0], pits[i, 1] - 1].Piece == null || table[pits[i, 0], pits[i, 1] - 1].Piece.Player != table[pits[i, 0], pits[i, 1]].Piece.Player) &&
                   (table[pits[i, 0] + 1, pits[i, 1]].PsuedoPiece == null || table[pits[i, 0] + 1, pits[i, 1]].PsuedoPiece.Player != table[pits[i, 0], pits[i, 1]].Piece.Player) &&
                   (table[pits[i, 0], pits[i, 1] + 1].PsuedoPiece == null || table[pits[i, 0], pits[i, 1] + 1].PsuedoPiece.Player != table[pits[i, 0], pits[i, 1]].Piece.Player) &&
                   (table[pits[i, 0] - 1, pits[i, 1]].PsuedoPiece == null || table[pits[i, 0] - 1, pits[i, 1]].PsuedoPiece.Player != table[pits[i, 0], pits[i, 1]].Piece.Player) &&
                   (table[pits[i, 0], pits[i, 1] - 1].PsuedoPiece == null || table[pits[i, 0], pits[i, 1] - 1].PsuedoPiece.Player != table[pits[i, 0], pits[i, 1]].Piece.Player))
                {
                    table[pits[i, 0], pits[i, 1]].Piece = null;
                }
                // If the piece is Psuedo. (For PsuedoPiece concept, refer to Square.cs or the game manual)
                if (table[pits[i, 0], pits[i, 1]].PsuedoPiece != null &&
                   (table[pits[i, 0] + 1, pits[i, 1]].Piece == null || table[pits[i, 0] + 1, pits[i, 1]].Piece.Player != table[pits[i, 0], pits[i, 1]].PsuedoPiece.Player) &&
                   (table[pits[i, 0], pits[i, 1] + 1].Piece == null || table[pits[i, 0], pits[i, 1] + 1].Piece.Player != table[pits[i, 0], pits[i, 1]].PsuedoPiece.Player) &&
                   (table[pits[i, 0] - 1, pits[i, 1]].Piece == null || table[pits[i, 0] - 1, pits[i, 1]].Piece.Player != table[pits[i, 0], pits[i, 1]].PsuedoPiece.Player) &&
                   (table[pits[i, 0], pits[i, 1] - 1].Piece == null || table[pits[i, 0], pits[i, 1] - 1].Piece.Player != table[pits[i, 0], pits[i, 1]].PsuedoPiece.Player) &&
                   (table[pits[i, 0] + 1, pits[i, 1]].PsuedoPiece == null || table[pits[i, 0] + 1, pits[i, 1]].PsuedoPiece.Player != table[pits[i, 0], pits[i, 1]].Piece.Player) &&
                   (table[pits[i, 0], pits[i, 1] + 1].PsuedoPiece == null || table[pits[i, 0], pits[i, 1] + 1].PsuedoPiece.Player != table[pits[i, 0], pits[i, 1]].Piece.Player) &&
                   (table[pits[i, 0] - 1, pits[i, 1]].PsuedoPiece == null || table[pits[i, 0] - 1, pits[i, 1]].PsuedoPiece.Player != table[pits[i, 0], pits[i, 1]].Piece.Player) &&
                   (table[pits[i, 0], pits[i, 1] - 1].PsuedoPiece == null || table[pits[i, 0], pits[i, 1] - 1].PsuedoPiece.Player != table[pits[i, 0], pits[i, 1]].Piece.Player))
                {
                    // There isn't another piece on the psuedo piece
                    if (table[pits[i, 0], pits[i, 1]].Piece == table[pits[i, 0], pits[i, 1]].PsuedoPiece)
                    {
                        table[pits[i, 0], pits[i, 1]].PsuedoPiece = null;
                        table[pits[i, 0], pits[i, 1]].Piece = null;
                    }
                    // There is another piece on the psuedo piece
                    else
                    {
                        table[pits[i, 0], pits[i, 1]].PsuedoPiece = null;
                    }
                }
            }
        }

        /// <summary>
        /// Record the game to have an undo button
        /// </summary>
        public void UpdateHistory()
        {
            for (int i = 1; i <= nr; i++)
            {
                for (int j = 1; j <= nc; j++) { }
                    //history[turn, i, j] = (Square)table[i, j].Clone();
            }
        }

        /// <summary>
        /// Gets executed when undo command is given
        /// </summary>
        public void UndoClicked()
        {
            do turn--;
            while (history[turn, 1, 1] == null);


            for (int i = 1; i <= nr; i++)
            {
                for (int j = 1; j <= nc; j++)
                    table[i, j] = (Square)history[turn, i, j].Clone();
            }
        }

        /// <summary>
        /// Repaint the board to default colors
        /// </summary>
        public void Repaint()
        {
            for (int i = 1; i <= nr; i++)
            {
                for (int j = 1; j <= nc; j++)
                {
                    table[i, j].BackgroundColor.Color = BoardHelper.DefaultColor(i, j);
                }
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////// Design time attribute, remove later /////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void DisableTurnConstraints()
        {
            turnConstraintsEnabled = false;
            Console.WriteLine("activated");
        }
    }
}
