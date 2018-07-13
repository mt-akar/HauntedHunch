using System;

namespace HauntedHunch
{
    public abstract class Piece : ICloneable
    {
        #region Static Variables

        /// <summary>
        /// Edge adjacency range
        /// </summary>
        public static int[,] e { get; } = { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };

        /// <summary>
        /// 2-square Rook range
        /// </summary>
        public static int[,] l { get; } = { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 }, { 2, 0 }, { 0, 2 }, { -2, 0 }, { 0, -2 } };

        /// <summary>
        /// 2-square Bishop range
        /// </summary>
        public static int[,] c { get; } = { { 1, 1 }, { -1, 1 }, { -1, -1 }, { 1, -1 }, { 2, 2 }, { -2, 2 }, { -2, -2 }, { 2, -2 } };

        /// <summary>
        /// Knight range
        /// </summary>
        public static int[,] j { get; } = { { 2, 1 }, { 1, 2 }, { -1, 2 }, { -2, 1 }, { -2, -1 }, { -1, -2 }, { 1, -2 }, { 2, -1 } };

        protected const int nr = 7;
        protected const int nc = 6;

        #endregion

        #region Public Properties

        /// <summary>
        /// Row coordinate of the piece
        /// </summary>
        public int Row { get; set; }
        /// <summary>
        /// Column coordinate of the piece
        /// </summary>
        public int Column { get; set; }
        /// <summary>
        /// Owner player of the piece
        /// </summary>
        public PlayerType Player { get; set; }
        /// <summary>
        /// Wheather the piece is revelaed to opponent
        /// </summary>
        public bool Revealed { get; set; } // Disguise mechanic

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="r">Row</param>
        /// <param name="c">Column</param>
        /// <param name="p">Player</param>
        protected Piece(int r, int c, PlayerType p)
        {
            Row = r;
            Column = c;
            Player = p;
            Revealed = false;
        }

        #endregion

        #region Abstract Move Methods

        /// <summary>
        /// Paints the possible movable squares on the board
        /// </summary>
        /// <param name="table">Game board to be updated</param>
        /// <param name="turn">Turn</param>
        public abstract void PossibleMoves(Square[,] table, int turn);

        /// <summary>
        /// Making the actual move
        /// </summary>
        /// <param name="table">Game board to be updated</param>
        /// <param name="to_row">Destination row</param>
        /// <param name="to_column">Destination column</param>
        /// <param name="turn">Turn</param>
        public abstract void Move(Square[,] table, int to_row, int to_column, ref int turn);

        #endregion

        #region Virtual Abilities

        // Abilities should be overritten by the class which wants to utilize them.

        /// <summary>
        /// Any ability that can be used without the interaction of another piece.
        /// </summary>
        /// <param name="table">Game board to be updated</param>
        /// <param name="turn">Turn</param>
        public virtual void AbilityUno(Square[,] table, ref int turn) => throw new Exception("Error: Ability 1 disfunction");

        /// <summary>
        /// Any ability that involves an interacter piece
        /// </summary>
        /// <param name="table">Game board to be updated</param>
        /// <param name="turn">Turn</param>
        /// <returns></returns>
        public virtual Square AbilityWithInteracterStageOne(Square[,] table, ref Square sen) => throw new Exception("Error: Ability 2 disfunction");

        /// <summary>
        /// Stage 2 of <see cref="AbilityWithInteracterStageOne"/>
        /// </summary>
        /// <param name="table">Game board to be updated</param>
        /// <param name="interacter">Interacter piece</param>
        /// <param name="sen">Moving piece</param>
        /// <param name="turn">Turn</param>
        public virtual void AbilityWithInteracterStageTwo(Square[,] table, ref Square interacter, ref Square sen, ref int turn) => throw new Exception("Error: Ability 2 disfunction");

        #endregion

        #region Protected Helpers

        // Paint necessary squares to default color
        protected static void PaintToDefault(Square[,] table, int row, int column, int[,] range)
        {
            table[row, column].BackgroundColor = BoardHelper.DefaultColor(row, column);
            for (int i = 0; i < range.Length / 2; i++)
            {
                if (row + range[i, 0] <= nr && row + range[i, 0] >= 1 && column + range[i, 1] <= nc && column + range[i, 1] >= 1)
                    table[row + range[i, 0], column + range[i, 1]].BackgroundColor = BoardHelper.DefaultColor(row + range[i, 0], column + range[i, 1]);
            }
        }

        // Chack if a piece is frozen
        protected static bool IsFrozen(Square[,] table, int row, int column)
        {
            for (int i = 0; i < 4; i++)
            {
                if (row + e[i, 0] <= nr && row + e[i, 0] >= 1 && column + e[i, 1] <= nc && column + e[i, 1] >= 1 &&
                    table[row + e[i, 0], column + e[i, 1]].Piece != null &&
                    table[row + e[i, 0], column + e[i, 1]].Piece is Freezer &&
                    table[row + e[i, 0], column + e[i, 1]].Piece.Player != table[row, column].Piece.Player &&
                    table[row + e[i, 0], column + e[i, 1]].Piece.Revealed)
                {
                    return true;
                }
            }
            return false;
        }

        // Chack if a piece is frozen
        protected static bool IsHiddenlyFrozen(Square[,] table, int row, int column)
        {
            for (int i = 0; i < 4; i++)
            {
                if (row + e[i, 0] <= nr && row + e[i, 0] >= 1 && column + e[i, 1] <= nc && column + e[i, 1] >= 1 &&
                    table[row + e[i, 0], column + e[i, 1]].Piece != null &&
                    table[row + e[i, 0], column + e[i, 1]].Piece is Freezer &&
                    table[row + e[i, 0], column + e[i, 1]].Piece.Player != table[row, column].Piece.Player &&
                    !table[row + e[i, 0], column + e[i, 1]].Piece.Revealed)
                {
                    table[row + e[i, 0], column + e[i, 1]].Piece.Revealed = true;
                    table[row + e[i, 0], column + e[i, 1]].SetImageAccordingToPiece();
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region IClonable

        /// <summary>
        /// IClonable method to create a deep copy of an object.
        /// </summary>
        /// <returns>A clone of the piece</returns>
        public abstract object Clone();

        #endregion
    }
}
