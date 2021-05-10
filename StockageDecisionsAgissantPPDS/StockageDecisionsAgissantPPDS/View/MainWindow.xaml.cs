using StockageDecisionsAgissantPPDS.ViewModel;

namespace StockageDecisionsAgissantPPDS.View
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        public MainWindow()
        {
            this.DataContext = new MainViewModel();

            this.InitializeComponent();
        }
    }
}