using System;
using Normacode.Patterns;

namespace StockageDecisionsAgissantPPDS.Persistence.DecisionPersister.RSS
{
    /// <summary>
    /// Entrepôt fainéant de RSSPersister
    /// </summary>
    public class RSSPersisterLazyWarehouse : IWarehouse<RSSPersister>
    {
        private Lazy<RSSPersister> _persister;

        /// <inheritdoc />
        public RSSPersister Fetch()
        {
            return this._persister.Value;
        }

        /// <summary>
        /// Stocke un persister
        /// </summary>
        public void ReplaceInstance(Lazy<RSSPersister> persister)
        {
            this._persister = persister;
        }
    }
}
