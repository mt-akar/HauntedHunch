using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HauntedHunch.Pieces;

namespace HauntedHunch
{
    public class Square : INotifyPropertyChanged
    {
        int row;
        public int Row { get { return row; } }

        int column;
        public int Column { get { return column; } }

        SolidColorBrush backgroundColor;
        public SolidColorBrush BackgroundColor { get { return backgroundColor; } set { backgroundColor = value; OnPropertyChanged("BackgroundColor"); } }

        Piece piece;
        public Piece Piece { get { return piece; } set { piece = value; if (piece == null && psuedoPiece != null) { piece = psuedoPiece; Imagize(); } } }

        ImageSource image;
        public ImageSource Image { get { return image; } set { image = value; OnPropertyChanged("Image"); } }

        Piece psuedoPiece;
        public Piece PsuedoPiece { get { return psuedoPiece; } set { psuedoPiece = value; } }

        public Square(int r, int c, Piece p)
        {
            row = r;
            column = c;
            piece = p;
            Imagize();
            psuedoPiece = null;
            backgroundColor = new SolidColorBrush();
            Paint();
        }

        private void Imagize()
        {
            if (piece == null)
            {
                image = new BitmapImage(new Uri(@"Images/Transparent.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.GetType() == typeof(Guard))
            {
                image = new BitmapImage(new Uri(@"Images/Guard.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.GetType() == typeof(Runner))
            {
                image = new BitmapImage(new Uri(@"Images/Runner.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.GetType() == typeof(Ranger))
            {
                image = new BitmapImage(new Uri(@"Images/Ranger.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.GetType() == typeof(Jumper))
            {
                image = new BitmapImage(new Uri(@"Images/Jumper.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.GetType() == typeof(Freezer))
            {
                image = new BitmapImage(new Uri(@"Images/Freezer.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.GetType() == typeof(Converter))
            {
                image = new BitmapImage(new Uri(@"Images/Converter.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.GetType() == typeof(Courier))
            {
                image = new BitmapImage(new Uri(@"Images/Courier.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.GetType() == typeof(Boomer))
            {
                image = new BitmapImage(new Uri(@"Images/Boomer.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.GetType() == typeof(InnKeeper))
            {
                Image = new BitmapImage(new Uri(@"Images/InnKeeper.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.GetType() == typeof(Lotus))
            {
                image = new BitmapImage(new Uri(@"Images/Lotus.png", UriKind.RelativeOrAbsolute));
            }
            else
            {
                image = new BitmapImage(new Uri(@"Images/kl.png", UriKind.RelativeOrAbsolute));
            }
        }

        public void Paint()
        {
            if ((row == 3 || row == 5) && (column == 2 || column == 4))
            {
                backgroundColor.Color = Color.FromArgb(255, 255, 200, 200);
            }
            else
            {
                backgroundColor.Color = ((row + column) % 2 == 0) ? Color.FromArgb(255, 255, 255, 255) : Color.FromArgb(255, 240, 240, 240);
            }
        }

        // ---- INotifyPropertyChanged ----
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        // ---- INotifyPropertyChanged ----
    }
}
