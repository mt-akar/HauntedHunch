using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HauntedHunch
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        #region Inner Classes

        public class SquareSelection
        {
            public bool selected;
            public SquareView selectedSquareView;
            public Point mousePos;

            public static SquareSelection Instance = new SquareSelection();

            public SquareSelection()
            {
                selected = false;
            }

            public void Select(SquareView selectedSquareView, Point mousePos)
            {
                selected = true;
                this.selectedSquareView = selectedSquareView;
                this.mousePos = mousePos;
                Panel.SetZIndex(this.selectedSquareView, 1);
            }

            public void Deselect()
            {
                selected = false;
                selectedSquareView.OuterBorder.Margin = new Thickness(0, 0, 0, 0);
                Panel.SetZIndex(selectedSquareView, 0);
            }
        }

        public class GameBoardCoordinate
        {
            public GameBoardCoordinate(int r, int c)
            {
                R = r;
                C = c;
            }
            public int R;
            public int C;
        }

        #endregion

        readonly SquareView[,] squareViews = new SquareView[G.r + 1, G.c + 1];

        readonly SquareSelection dragSelection = SquareSelection.Instance;

        readonly GamePageViewModel viewModel;

        public GamePage()
        {
            InitializeComponent();
            DataContext = viewModel = new GamePageViewModel();
            GameBoardGrid.MouseMove += Grid_MouseMove;
            GameBoardGrid.MouseLeave += Grid_MouseLeave;

            // Create squareViews and images inside it
            for (int i = 1; i <= G.r; i++)
                for (int j = 1; j <= G.c; j++)
                {
                    squareViews[i, j] = new SquareView { DataContext = viewModel.table[i, j] };
                    squareViews[i, j].MouseLeftButtonDown += LMDown;
                    squareViews[i, j].PreviewMouseLeftButtonUp += LMUp;
                    GameBoardGrid.Children.Add(squareViews[i, j]);
                    squareViews[i, j].SetValue(Grid.RowProperty, G.r - i);
                    squareViews[i, j].SetValue(Grid.ColumnProperty, j);
                }
        }

        /*
         void LMDown(object sender, MouseButtonEventArgs e)
        {
            // Exception cases that wont happen
            if (!(sender is SquareView)) throw new ArgumentException();
            if (sender == null) throw new ArgumentNullException();

            var squareViewModel = ((SquareView)sender).GetValue(DataContextProperty);

            // Exception cases that wont happen
            if (!(squareViewModel is Square)) throw new ArgumentException();

            if (!selection.selected)
            {
                if (((Square)((SquareView)sender).DataContext).Piece != null)
                    selection = new SquareSelection((SquareView)sender, Mouse.GetPosition(GameBoardGrid));
                else
                    selection.Deselect();
                // Call view model's LMDown method
                ((GamePageViewModel)DataContext).Select((Square)squareViewModel);
            }
            else
            {
                // Call view model's LMDown method
                ((GamePageViewModel)DataContext).Activate((Square)squareViewModel);
            }
        }

        void LMUp(object sender, MouseButtonEventArgs e)
        {
            // Problem cases
            if (!(sender is SquareView))
                throw new ArgumentException();
            if (sender == null)
                throw new ArgumentNullException();
            
            if ((SquareView)sender == selection.selectedSquare)
            {
                selection.selectedSquare.OuterBorder.Margin = new Thickness(0, 0, 0, 0);
                selection.Deselect();
                return;
            }

            var squareViewModel = ((SquareView)sender).GetValue(DataContextProperty);

            // Problem cases
            if (!(squareViewModel is Square))
                throw new ArgumentException();

            if(((GamePageViewModel)DataContext).Activate((Square)squareViewModel))
            {
                selection.Deselect();
                return;
            }
            selection.selectedSquare.OuterBorder.Margin = new Thickness(0, 0, 0, 0);
        }
        */
        
        #region Event handlers
            
        void LMDown(object sender, MouseButtonEventArgs e)
        {
            // Problem cases
            if (!(sender is SquareView)) throw new ArgumentException();
            if (sender == null) throw new ArgumentNullException();
            if (dragSelection.selected) throw new Exception($"Nothing can be selected at start od the {nameof(LMDown)}");

            if (((Square)((SquareView)sender).DataContext).Piece != null &&
                ((viewModel.turn - 2 * (int)((Square)((SquareView)sender).DataContext).Piece.Player + 1) % 4 == 0 ||
                (viewModel.turn - 2 * (int)((Square)((SquareView)sender).DataContext).Piece.Player + 1) % 4 == 1))
            {
                dragSelection.Select((SquareView)sender, Mouse.GetPosition(GameBoardGrid));
            }

            var sourceSquare = ((SquareView)sender).GetValue(DataContextProperty);

            // Problem cases
            if (!(sourceSquare is Square)) throw new ArgumentException();

            viewModel.Action((Square)sourceSquare);
        }

        void LMUp(object sender, MouseButtonEventArgs e)
        {
            // Problem cases
            if (!(sender is SquareView)) throw new ArgumentException();
            if (sender == null) throw new ArgumentNullException();

            var square = ((SquareView)sender).GetValue(DataContextProperty);

            // Problem cases
            if (!(square is Square)) throw new ArgumentException();

            // If nothing is selected, do nothing
            if (!dragSelection.selected)
                return;

            dragSelection.Deselect();

            // Get which square did player drag the piece to
            GameBoardCoordinate coords = GridPositionToGridCoord(Mouse.GetPosition(GameBoardGrid));

            // If piece is released outside the game board
            if (coords.R < 1 || coords.R > G.r || coords.C < 1 || coords.C > G.c)
                return;

            Square targetSquare = viewModel.table[coords.R, coords.C];

            // If piece is released in the same square
            if (targetSquare == ((SquareView)sender).DataContext)
                return;

            // If a selected piece is released in a another square
            viewModel.Action(targetSquare);
        }

        void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Released || !dragSelection.selected)
                return;

            var newMousePos = Mouse.GetPosition(GameBoardGrid);
            var offsetX = newMousePos.X - dragSelection.mousePos.X;
            var offsetY = newMousePos.Y - dragSelection.mousePos.Y;

            dragSelection.selectedSquareView.OuterBorder.Margin = new Thickness(
                dragSelection.selectedSquareView.OuterBorder.Margin.Left + offsetX,
                dragSelection.selectedSquareView.OuterBorder.Margin.Top + offsetY,
                dragSelection.selectedSquareView.OuterBorder.Margin.Right - offsetX,
                dragSelection.selectedSquareView.OuterBorder.Margin.Bottom - offsetY);

            dragSelection.mousePos = newMousePos;
        }

        void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            if (dragSelection.selected)
                dragSelection.Deselect();
        }

        #endregion

        #region Private Helpers

        GameBoardCoordinate GridPositionToGridCoord(Point p)
        {
            int coordX = (int)Math.Ceiling((GamePageViewModel.SquareWidth * G.r - p.Y) / GamePageViewModel.SquareWidth);
            int coordY = (int)Math.Ceiling((p.X - GamePageViewModel.SquareWidth / 3) / GamePageViewModel.SquareWidth);
            return new GameBoardCoordinate(coordX, coordY);
        }

        #endregion
    }
}
