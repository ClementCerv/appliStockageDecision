using System.Collections.Generic;
using System.Threading.Tasks;
using StockageDecisionsAgissantPPDS.Model.Decision;

namespace StockageDecisionsAgissantPPDS.Persistence.DecisionPersister
{
    /// <summary>
    /// Proxy d'un DecisionPersister
    /// </summary>
    public class DecisionPersisterProxy : IDecisionPersister
    {
        /// <summary>
        /// L'instance mise sous proxy
        /// </summary>
        public IDecisionPersister Proxied { private get; set; }
        
        public async Task StoreDecisionAsync(DecisionModel decision)
        {
            await this.Proxied.StoreDecisionAsync(decision);
        }

        public async Task<IEnumerable<DecisionModel>> FetchAllAsync()
        {
            return await this.Proxied.FetchAllAsync();
        }

        public async Task<bool> ExistsAsync(IDecisionModelIdentity decision)
        {
            return await this.Proxied.ExistsAsync(decision);
        }
    }
}