using System;
using JetBrains.Annotations;
using StockageDecisionsAgissantPPDS.Model;
using StockageDecisionsAgissantPPDS.Model.Decision;
using StockageDecisionsAgissantPPDS.ViewModel.Decision;

namespace StockageDecisionsAgissantPPDS.Design
{
    /// <summary>
    /// Design version of <see cref="DecisionViewModel"/>
    /// </summary>
    [UsedImplicitly]
    public class DesignDecisionViewModel : DecisionViewModel
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        public DesignDecisionViewModel() : 
            base(new DecisionModel("TestDecision", "TestDescription", DateTime.Now, new Uri("http://localhost.dev"), new Domaine[0]))
        {
        }
    }
}
