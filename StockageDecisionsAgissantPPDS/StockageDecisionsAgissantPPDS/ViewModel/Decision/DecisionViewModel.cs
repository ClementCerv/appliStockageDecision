using Normacode.InstanceProperty;
using StockageDecisionsAgissantPPDS.Model.Decision;

namespace StockageDecisionsAgissantPPDS.ViewModel.Decision
{
    /// <summary>
    /// ViewModel d'une décision
    /// </summary>
    public class DecisionViewModel : InstancePropertyHolder
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        public DecisionViewModel(DecisionModel decision)
        {
            this.DecisionModel = decision;
            this.EstSupprimée = new InstanceProperty<bool> {Value = decision.EstSupprimée};
            this.EstSupprimée.ValueChanged += (sender, value) => this.DecisionModel.EstSupprimée = value;
            this.ToolTip = "Description : " + this.DecisionModel.Description;
        }

        /// <summary>
        /// EstSupprimée
        /// </summary>
        public InstanceProperty<bool> EstSupprimée { get; }

        /// <summary>
        /// Model représenté
        /// </summary>
        public DecisionModel DecisionModel { get; }

        public string ToolTip { get; }
    }
}
