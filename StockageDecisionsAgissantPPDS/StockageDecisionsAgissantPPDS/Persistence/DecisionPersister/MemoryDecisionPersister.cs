using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockageDecisionsAgissantPPDS.Model.Decision;

namespace StockageDecisionsAgissantPPDS.Persistence.DecisionPersister
{
    /// <summary>
    /// Mémoire de L'IDecisionPersister
    /// </summary>
    public class MemoryDecisionPersister : IDecisionPersister
    {
        private readonly HashSet<DecisionModel> _decisionList;

        /// <summary>
        /// Constructeur
        /// </summary>
        public MemoryDecisionPersister()
        {
            this._decisionList = new HashSet<DecisionModel>();
        }
        
        private void StoreDecision(DecisionModel decision)
        {
            if (!decision.Domaines.Any())
            {
                throw new InvalidOperationException("Il faut au moins un domaine pour créer une décision.");
            }

            this._decisionList.Add(decision);
        }

        public async Task StoreDecisionAsync(DecisionModel decision)
        {
            await Task.Run(() => this.StoreDecision(decision));
        }

        public async Task<IEnumerable<DecisionModel>> FetchAllAsync()
        {
            return await Task.Run(() => this._decisionList);
        }

        public async Task<bool> ExistsAsync(IDecisionModelIdentity decision)
        {
            return await Task.Run(() => this._decisionList.Any(element => element.Equals(decision)));
        }
    }
}
