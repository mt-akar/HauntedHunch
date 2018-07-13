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

        Color backgroundColor;
        public Color BackgroundColor
        {
            get => backgroundColor;
            set
            {
                backgroundColor = value;
                OnPropertyChanged(nameof(BackgroundColor));
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
                if (piece == null && PsuedoPiece != null)
                    piece = PsuedoPiece;

                SetImageAccordingToPiece();
            }
        }

        public Piece PsuedoPiece { get; set; }

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

        public Square(int r, int c, Piece p)
        {
            Row = r;
            Column = c;
            Piece = p;
            backgroundColor = BoardHelper.DefaultColor(Row, Column);
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

        public object Clone() => new Square(Row, Column, (Piece)Piece?.Clone());

        #endregion
    }
}