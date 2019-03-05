using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HauntedHunch
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        const int R = G.r;
        const int C = G.c;

        public GamePage()
        {
            InitializeComponent();
            DataContext = new GamePageViewModel();

            // Create squareviews and images inside it
            for (int i = 1; i <= R; i++)
                for (int j = 1; j <= C; j++)
                {
                    var sq = new SquareView { DataContext = ((GamePageViewModel)DataContext).table[i, j] };
                    sq.MouseLeftButtonDown += LMDown;
                    GameBoardGrid.Children.Add(sq);
                    sq.SetValue(Grid.RowProperty, R - i);
                    sq.SetValue(Grid.ColumnProperty, j);
                }
        }

        void LMDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is DependencyObject))
                throw new ArgumentException();
            if (sender == null)
                throw new ArgumentNullException();

            var squareViewModel = ((DependencyObject)sender).GetValue(DataContextProperty);

            if (!(squareViewModel is SquareViewModel))
                throw new ArgumentException();

            ((GamePageViewModel)DataContext).LMDown((SquareViewModel)squareViewModel);
        }
    }
}
