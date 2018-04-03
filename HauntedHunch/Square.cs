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
        public SolidColorBrush BackgroundColor { get; }
        public Piece PsuedoPiece { get; set; }
        public ImageSource Image { get; private set; }

        private Piece piece;
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

        #endregion

        #region Constructor

        public Square(int r, int c, Piece p)
        {
            Row = r;
            Column = c;
            Piece = p;
            BackgroundColor = new SolidColorBrush(/*BackgroundColor.Color = */ BoardHelper.DefaultColor(Row, Column));
        }

        #endregion

        #region Helpers

        public void SetImageAccordingToPiece()
        {
            Image = new BitmapImage(new Uri(@"Images/" + (piece == null ? "Transparent.png" :
                (piece != null && piece.Revealed ? "" : "Hidden") + Piece.GetType().Name + Piece.Player.ToString() + ".png"), UriKind.Relative));
            OnPropertyChanged(nameof(Image));
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion

        #region IClonable

        public object Clone() => new Square(Row, Column, Piece == null ? null : (Piece)Piece.Clone());

        #endregion

    }
}