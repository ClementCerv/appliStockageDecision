using System;
using Normacode.Patterns;
using StockageDecisionsAgissantPPDS.Configuration;
using StockageDecisionsAgissantPPDS.Persistence.DecisionPersister.RSS;
using StockageDecisionsAgissantPPDS.Persistence.DomaineReference;

namespace StockageDecisionsAgissantPPDS.Persistence
{
    /// <summary>
    /// Factory produisant un DomaineReference
    /// </summary>
    public class DomaineReferenceFactory
    {
        private readonly IDomaineReferenceConfiguration _settingsProvider;
        private readonly IWarehouse<RSSPersister> _persisterWarehouse;

        /// <summary>
        /// Constructeur
        /// </summary>
        public DomaineReferenceFactory(IDomaineReferenceConfiguration settingsProvider, IWarehouse<RSSPersister> persisterWarehouse)
        {
            this._settingsProvider = settingsProvider;
            this._persisterWarehouse = persisterWarehouse;
        }

        /// <summary>
        /// Factory
        /// </summary>
        public IDomaineReference Factory()
        {
            switch (this._settingsProvider.PersisterDomainesSource)
            {
                case "RSS":
                    return this._persisterWarehouse.Fetch();
                case "List":
                    return new MemoryDomaineReference(this._settingsProvider.PersisterDomainesList);
                default:
                    throw new NotSupportedException(this._settingsProvider.PersisterDomainesSource + " is unknown DomaineReference type");
            }
        }
    }
}
