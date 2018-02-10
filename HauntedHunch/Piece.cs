using System;
using System.Windows.Media.Imaging;

namespace HauntedHunch
{
    public abstract class Piece
    {
        protected static BitmapImage emptyImage = new BitmapImage(new Uri(@"Images/Transparent.png", UriKind.RelativeOrAbsolute));

        protected int row;
        public int Row { get { return row; } set { row = value; } }

        protected int column;
        public int Column { get { return column; } set { column = value; } }

        // if (player == true) the piece is white's, if (player == false) the piece is black's
        protected bool player;
        public bool Player { get { return player; } set { player = value; } }

        // Disguise mechanic
        protected bool revealed;
        public bool Revealed { get { return revealed; } set { revealed = value; } }

        // see: Freezer
        protected bool frozen;
        public bool Frozen { get { return frozen; } set { frozen = value; } }

        // Primary range of the pieces.
        // protected static int[,] a;

        public abstract void PossibleMoves(ref Square[,] table, int turnDup);

        public abstract void Move(ref Square[,] table, int to_row, int to_column, ref int turn);

        // Abilities should be overridden by the class which wants to utilize them

        public virtual void AbilityUno(ref Square[,] table, ref int turn)
        {
            Console.WriteLine("Error: Ability 1 disfunction");
        }

        public virtual Square AbilityWithInteracterStageOne(ref Square[,] table, ref Square sen)
        {
            Console.WriteLine("Error: Ability 2 disfunction");
            return null;
        }

        public virtual void AbilityWithInteracterStageTwo(ref Square[,] table, ref Square interacter, ref Square sen, ref int turn)
        {
            Console.WriteLine("Error: Ability 2 disfunction");
        }
    }
}
