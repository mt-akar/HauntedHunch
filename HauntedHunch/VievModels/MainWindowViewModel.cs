namespace HauntedHunch
{
    public class MainWindowViewModel
    {
        #region Public Properties

        public ApplicationPageEnum CurrentPage { get; set; } = ApplicationPageEnum.GamePage;

        public double MinHeight => 720;

        public double MinWidth => 800;
        
        #endregion
    }
}
