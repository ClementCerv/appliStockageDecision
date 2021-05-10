using System.Windows;
using System.Windows.Input;

namespace StockageDecisionsAgissantPPDS.View.UserControl
{
    /// <summary>
    /// Interaction logic for DecisionViewUserControl.xaml
    /// </summary>
    public partial class DecisionViewUserControl 
    {
        /// <summary>
        /// Propriété rattacher à DeleteCommand
        /// </summary>
        public static readonly DependencyProperty DeleteCommandProperty = DependencyProperty.Register(
            "DeleteCommand", typeof(ICommand), typeof(DecisionViewUserControl), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// Propriété rattacher à EditCommand
        /// </summary>
        public static readonly DependencyProperty EditCommandProperty = DependencyProperty.Register(
            "EditCommand", typeof(ICommand), typeof(DecisionViewUserControl), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// DeleteCommand
        /// </summary>
        public ICommand DeleteCommand
        {
            get { return (ICommand) this.GetValue(DeleteCommandProperty); }
            set { this.SetValue(DeleteCommandProperty, value); }
        }

        /// <summary>
        /// EditCommand
        /// </summary>
        public ICommand EditCommand
        {
            get { return (ICommand) this.GetValue(EditCommandProperty); }
            set { this.SetValue(EditCommandProperty, value); }
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        public DecisionViewUserControl()
        {
            this.InitializeComponent();
        }
    }
}
