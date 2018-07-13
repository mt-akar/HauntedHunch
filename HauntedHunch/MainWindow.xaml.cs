using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

// Always (row, column)
// Always modify image before piece because changing piece can change image. Look: Square.cs(*675)

// TODO: Disguise implementation
// TODO: Disguise graphics
// TODO: Freeze mechanic rework
// TODO: Mindcontroller & Freezer interaction

// TODO: No legal move / pass turn
// TODO: Undo

namespace HauntedHunch
{
    public partial class MainWindow : Window
    {
        const int nr = 7;
        const int nc = 6;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();

            // Create stackpanels and images inside it
            for (int i = 1; i <= nr; i++)
            {
                for (int j = 1; j <= nc; j++)
                {
                    var bo = new Border { DataContext = ((MainWindowViewModel)DataContext).table[i, j] };
                    var im = new Image();
                    bo.MouseLeftButtonDown += LMDown;
                    bo.Child = im;
                    GameBoardGrid.Children.Add(bo);
                    bo.SetValue(Grid.RowProperty, nr - i);
                    bo.SetValue(Grid.ColumnProperty, j);
                }
            }

            Top = (SystemParameters.PrimaryScreenHeight - Height) / 4;
            Left = (SystemParameters.PrimaryScreenWidth - Width) / 10;
        }

        void LMDown(object sender, MouseButtonEventArgs e)
        {
            ((MainWindowViewModel)DataContext).LMDown((Square)(sender as DependencyObject).GetValue(DataContextProperty));
        }
    }
}
