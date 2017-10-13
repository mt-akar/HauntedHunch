using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HauntedHunch.Pieces;

namespace HauntedHunch
{
    public partial class MainWindow : Window
    {
        static BitmapImage emptyImage = new BitmapImage(new Uri(@"Images/Transparent.png", UriKind.RelativeOrAbsolute));
        private static int[,] adjacentRange = { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };

        Square[,] table;
        Square[,,] history;
        Square cur;
        Square interacter;
        int turn; // 0 and 1 module 4 are white's turns, 2 and 3 module 4 are black's turns
        bool gameEnded;

        public MainWindow()
        {
            InitializeComponent();
            table = new Square[8, 6];
            for (int i = 1; i <= 7; i++)
            {
                for (int j = 1; j <= 5; j++)
                {
                    if (i == 1 && j == 1)
                    {
                        table[i, j] = new Square(i, j, new Guard(i, j, true));
                    }
                    else if (i == 1 && j == 2)
                    {
                        table[i, j] = new Square(i, j, new Runner(i, j, true));
                    }
                    else if (i == 1 && j == 3)
                    {
                        table[i, j] = new Square(i, j, new Ranger(i, j, true));
                    }
                    else if (i == 1 && j == 4)
                    {
                        table[i, j] = new Square(i, j, new Jumper(i, j, true));
                    }
                    else if (i == 1 && j == 5)
                    {
                        table[i, j] = new Square(i, j, new Freezer(i, j, true));
                    }
                    else if (i == 2 && j == 1)
                    {
                        table[i, j] = new Square(i, j, new Converter(i, j, true));
                    }
                    else if (i == 2 && j == 2)
                    {
                        table[i, j] = new Square(i, j, new Courier(i, j, true));
                    }
                    else if (i == 2 && j == 3)
                    {
                        table[i, j] = new Square(i, j, new Boomer(i, j, true));
                    }
                    else if (i == 2 && j == 4)
                    {
                        table[i, j] = new Square(i, j, new InnKeeper(i, j, true));
                        table[i, j].PsuedoPiece = table[i, j].Piece;
                    }
                    else if (i == 2 && j == 5)
                    {
                        table[i, j] = new Square(i, j, new Lotus(i, j, true));
                    }
                    else if (i == 7 && j == 1)
                    {
                        table[i, j] = new Square(i, j, new Guard(i, j, false));
                    }
                    else if (i == 7 && j == 2)
                    {
                        table[i, j] = new Square(i, j, new Runner(i, j, false));
                    }
                    else if (i == 7 && j == 3)
                    {
                        table[i, j] = new Square(i, j, new Ranger(i, j, false));
                    }
                    else if (i == 7 && j == 4)
                    {
                        table[i, j] = new Square(i, j, new Jumper(i, j, false));
                    }
                    else if (i == 7 && j == 5)
                    {
                        table[i, j] = new Square(i, j, new Freezer(i, j, false));
                    }
                    else if (i == 6 && j == 1)
                    {
                        table[i, j] = new Square(i, j, new Converter(i, j, false));
                    }
                    else if (i == 6 && j == 2)
                    {
                        table[i, j] = new Square(i, j, new Courier(i, j, false));
                    }
                    else if (i == 6 && j == 3)
                    {
                        table[i, j] = new Square(i, j, new Boomer(i, j, false));
                    }
                    else if (i == 6 && j == 4)
                    {
                        table[i, j] = new Square(i, j, new InnKeeper(i, j, false));
                        table[i, j].PsuedoPiece = table[i, j].Piece;
                    }
                    else if (i == 6 && j == 5)
                    {
                        table[i, j] = new Square(i, j, new Lotus(i, j, false));
                    }
                    else
                    {
                        table[i, j] = new Square(i, j, null);
                    }
                }
            }
            history = new Square[8, 6, 1000];
            cur = null;
            interacter = null;
            turn = 0;

            Icon = BitmapFrame.Create(new Uri("pack://application:,,,/kl.ico", UriKind.RelativeOrAbsolute));

            #region DataContaxt Bindings

            sp11.DataContext = table[1, 1];
            sp12.DataContext = table[1, 2];
            sp13.DataContext = table[1, 3];
            sp14.DataContext = table[1, 4];
            sp15.DataContext = table[1, 5];
            sp21.DataContext = table[2, 1];
            sp22.DataContext = table[2, 2];
            sp23.DataContext = table[2, 3];
            sp24.DataContext = table[2, 4];
            sp25.DataContext = table[2, 5];
            sp31.DataContext = table[3, 1];
            sp32.DataContext = table[3, 2];
            sp33.DataContext = table[3, 3];
            sp34.DataContext = table[3, 4];
            sp35.DataContext = table[3, 5];
            sp41.DataContext = table[4, 1];
            sp42.DataContext = table[4, 2];
            sp43.DataContext = table[4, 3];
            sp44.DataContext = table[4, 4];
            sp45.DataContext = table[4, 5];
            sp51.DataContext = table[5, 1];
            sp52.DataContext = table[5, 2];
            sp53.DataContext = table[5, 3];
            sp54.DataContext = table[5, 4];
            sp55.DataContext = table[5, 5];
            sp61.DataContext = table[6, 1];
            sp62.DataContext = table[6, 2];
            sp63.DataContext = table[6, 3];
            sp64.DataContext = table[6, 4];
            sp65.DataContext = table[6, 5];
            sp71.DataContext = table[7, 1];
            sp72.DataContext = table[7, 2];
            sp73.DataContext = table[7, 3];
            sp74.DataContext = table[7, 4];
            sp75.DataContext = table[7, 5];

            im11.DataContext = table[1, 1];
            im12.DataContext = table[1, 2];
            im13.DataContext = table[1, 3];
            im14.DataContext = table[1, 4];
            im15.DataContext = table[1, 5];
            im21.DataContext = table[2, 1];
            im22.DataContext = table[2, 2];
            im23.DataContext = table[2, 3];
            im24.DataContext = table[2, 4];
            im25.DataContext = table[2, 5];
            im31.DataContext = table[3, 1];
            im32.DataContext = table[3, 2];
            im33.DataContext = table[3, 3];
            im34.DataContext = table[3, 4];
            im35.DataContext = table[3, 5];
            im41.DataContext = table[4, 1];
            im42.DataContext = table[4, 2];
            im43.DataContext = table[4, 3];
            im44.DataContext = table[4, 4];
            im45.DataContext = table[4, 5];
            im51.DataContext = table[5, 1];
            im52.DataContext = table[5, 2];
            im53.DataContext = table[5, 3];
            im54.DataContext = table[5, 4];
            im55.DataContext = table[5, 5];
            im61.DataContext = table[6, 1];
            im62.DataContext = table[6, 2];
            im63.DataContext = table[6, 3];
            im64.DataContext = table[6, 4];
            im65.DataContext = table[6, 5];
            im71.DataContext = table[7, 1];
            im72.DataContext = table[7, 2];
            im73.DataContext = table[7, 3];
            im74.DataContext = table[7, 4];
            im75.DataContext = table[7, 5];

            #endregion

        }

        // Most complicated methos of the project. Activated when clicked on a square.
        // Does different things depending on the input square and the state of the variables turn, cur and interacter.
        private void LMDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (gameEnded)
            {
                return;
            }
            // Sender object (Square)
            Square sen = ((Square)((sender as DependencyObject).GetValue(FrameworkElement.DataContextProperty)));

            tunnel1:

            if (cur == null)
            {
                if (sen.Piece != null && ((sen.Piece.Player && (turn % 4 == 0 || turn % 4 == 3)) || (!sen.Piece.Player && (turn % 4 == 1 || turn % 4 == 2))))
                {
                    sen.BackgroundColor.Color = Color.FromArgb(255, 204, 255, 220);

                    int[,] possible = sen.Piece.PossibleMoves(table, turn);
                    for (int i = 0; i < possible.Length / 3; i++)
                    {
                        if (possible[i, 2] == 1)
                        {
                            sen.BackgroundColor.Color = Color.FromArgb(255, 255, 0, 0);
                        }
                        else if (possible[i, 2] == 2)
                        {
                            table[possible[i, 0], possible[i, 1]].BackgroundColor.Color = Color.FromArgb(255, 255, 100, 100);
                        }
                        else if (possible[i, 0] > 0)
                        {
                            table[possible[i, 0], possible[i, 1]].BackgroundColor.Color = Color.FromArgb(255, 204, 255, 220);
                        }
                    }
                    cur = sen;
                }
                else
                {
                    // Do nothing
                }
            }
            else
            {
                if (sen.BackgroundColor.Color == Color.FromArgb(255, 255, 0, 0))
                {
                    cur.Piece.AbilityUno(ref table, ref turn);

                    UpdatePits();
                    UpdateFrozenity();
                    cur = null;
                    Repaint();
                }
                else if (sen.BackgroundColor.Color == Color.FromArgb(255, 255, 100, 100))
                {
                    Repaint();
                    int stage = cur.Piece.AbilityWithInteracter(ref table, ref interacter, ref sen, ref turn);
                    if (stage == 1)
                    {
                        interacter = sen;
                    }
                    else if (stage == 2)
                    {
                        UpdatePits();
                        UpdateFrozenity();
                        cur = null;
                        interacter = null;
                    }
                }
                else if (sen.BackgroundColor.Color == Color.FromArgb(255, 204, 255, 220) && cur != sen)
                {
                    cur.Piece.Move(ref table, sen.Row, sen.Column, ref turn);

                    UpdatePits();
                    UpdateFrozenity();
                    cur = null;
                    Repaint();
                }
                else if (cur != sen && sen.Piece != null && cur.Piece.Player == sen.Piece.Player)
                {
                    cur = null;
                    Repaint();
                    goto tunnel1;
                }
                else
                {
                    cur = null;
                    Repaint();
                }
            }

            // gameEnded check
            int lotusCount = 0;
            for (int i = 1; i <= 7; i++)
            {
                for (int j = 1; j <= 5; j++)
                {
                    if (table[i, j].Piece != null && table[i, j].Piece.GetType() == typeof(Lotus))
                    {
                        if ((table[i, j].Piece.Player && i == 7) || (!table[i, j].Piece.Player && i == 1))
                        {
                            gameEnded = true;
                        }
                        lotusCount++;
                    }
                }
            }
            if (lotusCount < 2)
            {
                gameEnded = true;
            }
        }

        #region Tool methods

        private void UpdatePits()
        {
            int[,] pits = { { 3, 2 }, { 3, 4 }, { 5, 2 }, { 5, 4 } };
            for (int i = 0; i < 4; i++)
            {
                if (table[pits[i, 0], pits[i, 1]].Piece != null &&
                   (table[pits[i, 0] + 1, pits[i, 1]].Piece == null || table[pits[i, 0] + 1, pits[i, 1]].Piece.Player != table[pits[i, 0], pits[i, 1]].Piece.Player) &&
                   (table[pits[i, 0], pits[i, 1] + 1].Piece == null || table[pits[i, 0], pits[i, 1] + 1].Piece.Player != table[pits[i, 0], pits[i, 1]].Piece.Player) &&
                   (table[pits[i, 0] - 1, pits[i, 1]].Piece == null || table[pits[i, 0] - 1, pits[i, 1]].Piece.Player != table[pits[i, 0], pits[i, 1]].Piece.Player) &&
                   (table[pits[i, 0], pits[i, 1] - 1].Piece == null || table[pits[i, 0], pits[i, 1] - 1].Piece.Player != table[pits[i, 0], pits[i, 1]].Piece.Player))
                {
                    table[pits[i, 0], pits[i, 1]].Image = emptyImage;
                    table[pits[i, 0], pits[i, 1]].Piece = null;
                }
                if (table[pits[i, 0], pits[i, 1]].PsuedoPiece != null &&
                   (table[pits[i, 0] + 1, pits[i, 1]].Piece == null || table[pits[i, 0] + 1, pits[i, 1]].Piece.Player != table[pits[i, 0], pits[i, 1]].PsuedoPiece.Player) &&
                   (table[pits[i, 0], pits[i, 1] + 1].Piece == null || table[pits[i, 0], pits[i, 1] + 1].Piece.Player != table[pits[i, 0], pits[i, 1]].PsuedoPiece.Player) &&
                   (table[pits[i, 0] - 1, pits[i, 1]].Piece == null || table[pits[i, 0] - 1, pits[i, 1]].Piece.Player != table[pits[i, 0], pits[i, 1]].PsuedoPiece.Player) &&
                   (table[pits[i, 0], pits[i, 1] - 1].Piece == null || table[pits[i, 0], pits[i, 1] - 1].Piece.Player != table[pits[i, 0], pits[i, 1]].PsuedoPiece.Player))
                {
                    if (table[pits[i, 0], pits[i, 1]].Piece == table[pits[i, 0], pits[i, 1]].PsuedoPiece)
                    {
                        table[pits[i, 0], pits[i, 1]].PsuedoPiece = null;
                        table[pits[i, 0], pits[i, 1]].Image = emptyImage;
                        table[pits[i, 0], pits[i, 1]].Piece = null;
                    }
                    else
                    {
                        table[pits[i, 0], pits[i, 1]].PsuedoPiece = null;
                    }
                }
            }
        }

        private void UpdateFrozenity()
        {
            for (int i = 1; i <= 7; i++)
            {
                for (int j = 1; j <= 5; j++)
                {
                    if (table[i, j].Piece != null)
                    {
                        table[i, j].Piece.Frozen = false;
                        for (int k = 0; k < 4; k++)
                        {
                            if (i + adjacentRange[k, 0] <= 7 && i + adjacentRange[k, 0] >= 1 && j + adjacentRange[k, 1] <= 5 && j + adjacentRange[k, 1] >= 1 &&
                                table[i + adjacentRange[k, 0], j + adjacentRange[k, 1]].Piece != null &&
                                table[i + adjacentRange[k, 0], j + adjacentRange[k, 1]].Piece.GetType() == typeof(Freezer) &&
                                table[i + adjacentRange[k, 0], j + adjacentRange[k, 1]].Piece.Player != table[i, j].Piece.Player)
                            {
                                table[i, j].Piece.Frozen = true;
                            }
                        }
                    }
                }
            }
        }

        private void Repaint()
        {
            for (int i = 1; i <= 7; i++)
            {
                for (int j = 1; j <= 5; j++)
                {
                    if ((i == 3 || i == 5) && (j == 2 || j == 4))
                    {
                        table[i, j].BackgroundColor.Color = Color.FromArgb(255, 255, 200, 200);
                    }
                    else
                    {
                        table[i, j].BackgroundColor.Color = ((i + j) % 2 == 0) ? Color.FromArgb(255, 255, 255, 255) : Color.FromArgb(255, 244, 244, 244);
                    }
                }
            }
        }

        #endregion

        private void UndoClicked(object sender, RoutedEventArgs e) { }

        private void CreditsClicked(object sender, RoutedEventArgs e) { }
    }
}
