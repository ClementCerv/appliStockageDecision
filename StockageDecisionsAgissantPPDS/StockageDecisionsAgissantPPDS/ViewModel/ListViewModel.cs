using System.Collections.Generic;
using System.Linq;
using StockageDecisionsAgissantPPDS.Properties;

namespace StockageDecisionsAgissantPPDS.ViewModel
{
    public class ListViewModel
    {
        public static ListViewModel Instance { get; } = new ListViewModel();

        private ListViewModel()
        {
            DecisionsList = Settings.Default.Domaines.Cast<string>();
        }

        public IEnumerable<string> DecisionsList { get; }
    }
}