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
        public SolidColorBrush BackgroundColor { get { return backgroundColor; } set { backgroundColor = value; } }

        Piece piece;
        public Piece Piece
        {
            get { return piece; }
            set
            {
                piece = value;
                // If a piece moves away from on top of a psuedo piece, psuedo piece comes back.
                if (piece == null && psuedoPiece != null)
                {
                    piece = psuedoPiece;
                    SetImageAccordingToPiece();  // (*675)
                }
            }
        }

        ImageSource image;
        public ImageSource Image { get { return image; } set { image = value; OnPropertyChanged("Image"); } }

        // Some pieces such as innkeeper has the ability to be passive. Pieces that have this ability also occupies the psuedo piece slot.
        Piece psuedoPiece;
        public Piece PsuedoPiece { get { return psuedoPiece; } set { psuedoPiece = value; } }

        public Square(int r, int c, Piece p)
        {
            row = r;
            column = c;
            piece = p;
            SetImageAccordingToPiece();
            backgroundColor = new SolidColorBrush();
            Paint();
        }

        public void SetImageAccordingToPiece()
        {
            if (piece == null)
            {
                image = new BitmapImage(new Uri(@"Images/Transparent.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.GetType() == typeof(Guard))
            {
                if (piece.Player) image = new BitmapImage(new Uri(@"Images/GuardWhite.png", UriKind.RelativeOrAbsolute));
                else image = new BitmapImage(new Uri(@"Images/GuardBlack.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.GetType() == typeof(Runner))
            {
                if (piece.Player) image = new BitmapImage(new Uri(@"Images/RunnerWhite.png", UriKind.RelativeOrAbsolute));
                else image = new BitmapImage(new Uri(@"Images/RunnerBlack.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.GetType() == typeof(Ranger))
            {
                if (piece.Player) image = new BitmapImage(new Uri(@"Images/RangerWhite.png", UriKind.RelativeOrAbsolute));
                else image = new BitmapImage(new Uri(@"Images/RangerBlack.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.GetType() == typeof(Jumper))
            {
                if (piece.Player) image = new BitmapImage(new Uri(@"Images/JumperWhite.png", UriKind.RelativeOrAbsolute));
                else image = new BitmapImage(new Uri(@"Images/JumperBlack.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.GetType() == typeof(Freezer))
            {
                if (piece.Player) image = new BitmapImage(new Uri(@"Images/FreezerWhite.png", UriKind.RelativeOrAbsolute));
                else image = new BitmapImage(new Uri(@"Images/FreezerBlack.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.GetType() == typeof(Converter))
            {
                if (piece.Player) image = new BitmapImage(new Uri(@"Images/ConverterWhite.png", UriKind.RelativeOrAbsolute));
                else image = new BitmapImage(new Uri(@"Images/ConverterBlack.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.GetType() == typeof(Courier))
            {
                if (piece.Player) image = new BitmapImage(new Uri(@"Images/CourierWhite.png", UriKind.RelativeOrAbsolute));
                else image = new BitmapImage(new Uri(@"Images/CourierBlack.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.GetType() == typeof(Boomer))
            {
                if (piece.Player) image = new BitmapImage(new Uri(@"Images/BoomerWhite.png", UriKind.RelativeOrAbsolute));
                else image = new BitmapImage(new Uri(@"Images/BoomerBlack.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.GetType() == typeof(InnKeeper))
            {
                if (piece.Player) Image = new BitmapImage(new Uri(@"Images/InnKeeperWhite.png", UriKind.RelativeOrAbsolute));
                else Image = new BitmapImage(new Uri(@"Images/InnKeeperBlack.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.GetType() == typeof(Lotus))
            {
                if (piece.Player) image = new BitmapImage(new Uri(@"Images/LotusWhite.png", UriKind.RelativeOrAbsolute));
                else image = new BitmapImage(new Uri(@"Images/LotusBlack.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.GetType() == typeof(MindController))
            {
                if (piece.Player) image = new BitmapImage(new Uri(@"Images/MindControllerWhite.png", UriKind.RelativeOrAbsolute));
                else image = new BitmapImage(new Uri(@"Images/MindControllerBlack.png", UriKind.RelativeOrAbsolute));
            }
            else
            {
                Console.WriteLine("Error: Unknown piece type");
            }
            OnPropertyChanged("Image");
        }

        public void Paint()
        {
            if ((row == 3 || row == 5) && (column == 2 || column == 4)) // 4 trap squares are pink.
            {
                backgroundColor.Color = Color.FromArgb(255, 255, 200, 200);
            }
            else // Rest of the board is white/black
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
