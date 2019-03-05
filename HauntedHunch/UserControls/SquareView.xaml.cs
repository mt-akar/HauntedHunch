using System.Windows;
using System.Windows.Controls;

namespace HauntedHunch
{
    /// <summary>
    /// Interaction logic for SquareControl.xaml
    /// </summary>
    public partial class SquareView : Grid
    {
        public SquareView()
        {
            InitializeComponent();

            Border.Margin = new Thickness(GamePageViewModel.SquareWidth / 7);
        }
    }
}
