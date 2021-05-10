using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockageDecisionsAgissantPPDS.Common;
using StockageDecisionsAgissantPPDS.Model.Decision;

namespace StockageDecisionsAgissantPPDS.Persistence.DecisionPersister
{
    /// <summary>
    /// Décorateur permettant de loguer les requêtes et les exceptions d'un <see cref="IDecisionPersister"/>
    /// </summary>
    public class DecisionPersisterLogDecorator : IDecisionPersister
    {
        private readonly IDecisionPersister _decorated;
        private readonly Logger _log;

        public DecisionPersisterLogDecorator(IDecisionPersister decorated, Logger log)
        {
            this._decorated = decorated;
            this._log = log;
        }
        
        public async Task StoreDecisionAsync(DecisionModel decision)
        {
            try
            {
                this._log.Write($"Storing decision {decision} on persister {this._decorated}");
                await this._decorated.StoreDecisionAsync(decision);
                this._log.Write("Decision stored");
            }
            catch (Exception e)
            {
                this._log.Write($"Error while storing decision {decision} on persister {this._decorated}");
                this._log.Write(e.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<DecisionModel>> FetchAllAsync()
        {
            try
            {
                this._log.Write($"FetchAll decision on persister {this._decorated}");
                IEnumerable<DecisionModel> fetchAllDecision = await this._decorated.FetchAllAsync();
                this._log.Write("Decision FetchAll");
                return fetchAllDecision;
            }
            catch (Exception e)
            {
                this._log.Write($"Error while FetchAll decision on persister {this._decorated}");
                this._log.Write(e.ToString());
                throw;
            }
        }

        public async Task<bool> ExistsAsync(IDecisionModelIdentity decision)
        {
            try
            {
                this._log.Write($"Exist decision {decision} on persister {this._decorated}");
                bool existsDecision = await this._decorated.ExistsAsync(decision);
                this._log.Write("Decision Existed");
                return existsDecision;
            }
            catch (Exception e)
            {
                this._log.Write($"Error while existing decision {decision} on persister {this._decorated}");
                this._log.Write(e.ToString());
                throw;
            }
        }
    }
}
