using System;

namespace HauntedHunch
{
    public abstract class Piece : ICloneable
    {
        #region Static Variables

        /// <summary>
        /// Edge adjacency range
        /// </summary>
        public static int[,] e { get; } = new int[4, 2] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };

        /// <summary>
        /// 2-square Rook range
        /// </summary>
        public static int[,] l { get; } = new int[8, 2] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 }, { 2, 0 }, { 0, 2 }, { -2, 0 }, { 0, -2 } };

        /// <summary>
        /// 2-square Bishop range
        /// </summary>
        public static int[,] c { get; } = new int[8, 2] { { 1, 1 }, { -1, 1 }, { -1, -1 }, { 1, -1 }, { 2, 2 }, { -2, 2 }, { -2, -2 }, { 2, -2 } };

        /// <summary>
        /// Knight range
        /// </summary>
        public static int[,] j { get; } = new int[8, 2] { { 2, 1 }, { 1, 2 }, { -1, 2 }, { -2, 1 }, { -2, -1 }, { -1, -2 }, { 1, -2 }, { 2, -1 } };

        protected static int nr = 7;
        protected static int nc = 6;

        #endregion

        #region Public Properties

        public int Row { get; set; }
        public int Column { get; set; }
        public PlayerType Player { get; set; }
        public bool Revealed { get; set; } // Disguise mechanic

        #endregion

        #region Constructor

        protected Piece(int r, int c, PlayerType p)
        {
            Row = r;
            Column = c;
            Player = p;
            Revealed = false;
        }

        #endregion

        #region Abstract Move Methods

        public abstract void PossibleMoves(Square[,] table, int turn);

        public abstract void Move(Square[,] table, int to_row, int to_column, ref int turn);

        #endregion

        #region Virtual Abilities

        // Abilities should be overritten by the class which wants to utilize them.
        public virtual void AbilityUno(Square[,] table, ref int turn) => throw new Exception("Error: Ability 1 disfunction");

        public virtual Square AbilityWithInteracterStageOne(Square[,] table, ref Square sen) => throw new Exception("Error: Ability 2 disfunction");

        public virtual void AbilityWithInteracterStageTwo(Square[,] table, ref Square interacter, ref Square sen, ref int turn) => throw new Exception("Error: Ability 2 disfunction");

        #endregion

        #region Protected Helpers

        // Paint necessary squares to default color
        protected static void PaintToDefault(Square[,] table, int row, int column, int[,] range)
        {
            table[row, column].BackgroundColor.Color = BoardHelper.DefaultColor(row, column);
            for (int i = 0; i < range.Length / 2; i++)
            {
                if (row + range[i, 0] <= nr && row + range[i, 0] >= 1 && column + range[i, 1] <= nc && column + range[i, 1] >= 1)
                    table[row + range[i, 0], column + range[i, 1]].BackgroundColor.Color = BoardHelper.DefaultColor(row + range[i, 0], column + range[i, 1]);
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
                    table[row + e[i, 0], column + e[i, 1]].Piece.Player != table[row, column].Piece.Player)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region IClonable

        public abstract object Clone();

        #endregion
    }
}
