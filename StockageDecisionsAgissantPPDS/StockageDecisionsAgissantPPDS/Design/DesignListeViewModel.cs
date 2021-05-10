using JetBrains.Annotations;
using StockageDecisionsAgissantPPDS.ViewModel;

namespace StockageDecisionsAgissantPPDS.Design
{
    [UsedImplicitly]
    internal class DesignListeViewModel : ListeViewModel
    {
        private static readonly DesignDecisionPersister DesignDecisionPersister = new DesignDecisionPersister();

        public DesignListeViewModel() : base(DesignDecisionPersister, DesignDecisionPersister)
        {
        }
    }
}
