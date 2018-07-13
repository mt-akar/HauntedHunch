using System;
using System.Windows;

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
        MainWindowViewModel d = new MainWindowViewModel(); // I want to remove this

        public MainWindow()
        {
            InitializeComponent();
            DataContext = d;

            #region DataContaxt Bindings

            sp11.DataContext = d.table[1, 1];
            sp12.DataContext = d.table[1, 2];
            sp13.DataContext = d.table[1, 3];
            sp14.DataContext = d.table[1, 4];
            sp15.DataContext = d.table[1, 5];
            sp16.DataContext = d.table[1, 6];
            sp21.DataContext = d.table[2, 1];
            sp22.DataContext = d.table[2, 2];
            sp23.DataContext = d.table[2, 3];
            sp24.DataContext = d.table[2, 4];
            sp25.DataContext = d.table[2, 5];
            sp26.DataContext = d.table[2, 6];
            sp31.DataContext = d.table[3, 1];
            sp32.DataContext = d.table[3, 2];
            sp33.DataContext = d.table[3, 3];
            sp34.DataContext = d.table[3, 4];
            sp35.DataContext = d.table[3, 5];
            sp36.DataContext = d.table[3, 6];
            sp41.DataContext = d.table[4, 1];
            sp42.DataContext = d.table[4, 2];
            sp43.DataContext = d.table[4, 3];
            sp44.DataContext = d.table[4, 4];
            sp45.DataContext = d.table[4, 5];
            sp46.DataContext = d.table[4, 6];
            sp51.DataContext = d.table[5, 1];
            sp52.DataContext = d.table[5, 2];
            sp53.DataContext = d.table[5, 3];
            sp54.DataContext = d.table[5, 4];
            sp55.DataContext = d.table[5, 5];
            sp56.DataContext = d.table[5, 6];
            sp61.DataContext = d.table[6, 1];
            sp62.DataContext = d.table[6, 2];
            sp63.DataContext = d.table[6, 3];
            sp64.DataContext = d.table[6, 4];
            sp65.DataContext = d.table[6, 5];
            sp66.DataContext = d.table[6, 6];
            sp71.DataContext = d.table[7, 1];
            sp72.DataContext = d.table[7, 2];
            sp73.DataContext = d.table[7, 3];
            sp74.DataContext = d.table[7, 4];
            sp75.DataContext = d.table[7, 5];
            sp76.DataContext = d.table[7, 6];

            #endregion

            Top = (SystemParameters.PrimaryScreenHeight - Height) / 4;
            Left = (SystemParameters.PrimaryScreenWidth - Width) / 10;
        }

        void LMDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (d.gameEnded) return;

            // Sender object (Square)
            Square sen = (Square)(sender as DependencyObject).GetValue(DataContextProperty);

            d.LMDown(sen);
        }


        void UndoClicked(object sender, RoutedEventArgs e)
        {
            d.UndoClicked();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////// Design time attribute, remove later /////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        void disableTurnConstraints(object sender, RoutedEventArgs e)
        {
            d.turnConstraintsEnabled = false;
            Console.WriteLine("Click");
        }
    }
}
