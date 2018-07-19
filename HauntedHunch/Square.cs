using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HauntedHunch
{
    public class Square : INotifyPropertyChanged, ICloneable
    {
        #region Public Properties

        public int Row { get; }

        public int Column { get; }

        /// <summary>
        /// Defines visual look of the squre in game
        /// </summary>
        SquareState state;
        public SquareState State
        {
            get => state;
            set
            {
                if (value == SquareState.None)
                    // Trap squares are redish, rest of the board is white/black
                    state = (Row == 3 || Row == 5) && (Column == 2 || Column == 5) ? SquareState.Trap : (Row + Column) % 2 == 0 ? SquareState.White : SquareState.Black;
                else
                    state = value;
                OnPropertyChanged(nameof(State));
            }
        }

        Piece piece;
        public Piece Piece
        {
            get => piece;
            set
            {
                piece = value;

                // If a piece moves away from on top of a psuedo piece, psuedo piece comes back.
                // If you want to null both piece and psuedo piece, null psuedo piece first since you cannot null the piece that has a psuedo piece below it.
                if (piece == null && PsuedoPiece != null)
                    piece = PsuedoPiece;

                // If piece is psuedo enabled, set its psuedo piece to its piece
                else if (piece is InnKeeper)
                    PsuedoPiece = piece;

                SetImageAccordingToPiece();
            }
        }

        /// <summary>
        /// The piece that can be under other pieces
        /// </summary>
        public Piece PsuedoPiece { get; set; }

        /// <summary>
        /// Image source of the image of the active piece
        /// </summary>
        ImageSource image;
        public ImageSource Image
        {
            get => image;
            private set
            {
                image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        #endregion

        #region Constructor

        public Square(int r, int c, Piece p = null)
        {
            Row = r;
            Column = c;
            Piece = p;
            State = SquareState.None;
        }

        #endregion

        #region Helpers

        public void SetImageAccordingToPiece()
        {
            Image = new BitmapImage(new Uri(@"Images/" + (piece == null ? "Transparent.png" :
                (piece.Revealed ? "" : "Hidden") + Piece.GetType().Name + Piece.Player + ".png"), UriKind.Relative));
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        #endregion

        #region IClonable

        public object Clone() => new Square(0, 0, (Piece)Piece?.Clone()) { PsuedoPiece = (Piece)PsuedoPiece?.Clone() };

        #endregion
    }
}