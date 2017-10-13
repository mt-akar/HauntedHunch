using System;
using System.Windows.Media.Imaging;

namespace HauntedHunch
{
    public abstract class Piece
    {
        protected static BitmapImage emptyImage = new BitmapImage(new Uri(@"Images/Transparent.png", UriKind.RelativeOrAbsolute));
        private static int[,] adjacentRange = { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };

        protected int row;
        public int Row { get { return row; } set { row = value; } }

        protected int column;
        public int Column { get { return column; } set { column = value; } }

        protected bool player;
        public bool Player { get { return player; } set { player = value; } }

        // see: Freezer
        protected bool frozen;
        public bool Frozen { get { return frozen; } set { frozen = value; } }

        protected int[,] a;
        public int[,] A { get { return a; } set { a = value; } }

        public abstract int[,] PossibleMoves(Square[,] tableDup, int turnDup);

        public abstract void Move(ref Square[,] table, int to_row, int to_column, ref int turn);

        // Abilities should be overridden by the class which wants to utilize them

        public virtual void AbilityUno(ref Square[,] table, ref int turn)
        {
            Console.WriteLine("Error: Ability disfunction");
        }
        
        public virtual int AbilityWithInteracter(ref Square[,] table, ref Square interacter, ref Square sen, ref int turn)
        {
            Console.WriteLine("Error: Ability disfunction");
            return 0;
        }
    }
}
