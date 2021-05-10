using System.Collections.Generic;
using System.Threading.Tasks;
using StockageDecisionsAgissantPPDS.Model.Decision;

namespace StockageDecisionsAgissantPPDS.Persistence.DecisionPersister
{
    /// <summary>
    /// Interface d'accès en lecture écriture à un stockage de décision
    /// </summary>
    public interface IDecisionPersister
    {
        /// <summary>
        /// Stocke une décision dans le persister
        /// </summary>
        Task StoreDecisionAsync(DecisionModel decision);

        /// <summary>
        /// Récupère l'intégralité des décisions du persister
        /// </summary>
        Task<IEnumerable<DecisionModel>> FetchAllAsync();

        /// <summary>
        /// Vérifie l'existence d'un DecisionModel dans le persister.
        /// </summary>
        Task<bool> ExistsAsync(IDecisionModelIdentity decision);
    }
}
