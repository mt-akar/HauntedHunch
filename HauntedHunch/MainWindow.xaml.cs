using System.Windows;

// Always (row, column)
// Always modify image before piece because changing piece can change image. Look: Square.cs(*675)

// TODO: Disguise implementation
// TODO: Disguise graphics
// TODO: Freeze mechanic rework
// TODO: Mindscontroller & Freezer interaction

// TODO: No legal move / pass turn
// TODO: Undo

namespace HauntedHunch
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();

            Top = (SystemParameters.PrimaryScreenHeight - Height) / 4;
            Left = (SystemParameters.PrimaryScreenWidth - Width) / 10;
        }
    }
}
