using System;
using System.IO;
using System.Text;
using Normacode.IO.Adapters;
using Normacode.Patterns;
using StockageDecisionsAgissantPPDS.Configuration;
using StockageDecisionsAgissantPPDS.Persistence.DecisionPersister;
using StockageDecisionsAgissantPPDS.Persistence.DecisionPersister.RSS;

namespace StockageDecisionsAgissantPPDS.Persistence
{
    /// <summary>
    /// Crée une instance de IDecisionPersister
    /// </summary>
    public class DecisionPersisterFactory
    {
        private readonly IDecisionPersisterConfiguration _settingsProvider;
        private readonly IWarehouse<RSSPersister> _rssPersisterWarehouse;

        /// <summary>
        /// Constructeur
        /// </summary>
        public DecisionPersisterFactory(IDecisionPersisterConfiguration settingsProvider, IWarehouse<RSSPersister> rssPersisterWarehouse)
        {
            this._settingsProvider = settingsProvider;
            this._rssPersisterWarehouse = rssPersisterWarehouse;
        }

        /// <summary>
        /// Factory
        /// </summary>
        public IDecisionPersister Factory()
        {
            switch (this._settingsProvider.PersisterType)
            {
                case "RSS":
                    return this._rssPersisterWarehouse.Fetch();
                case "Csv":
                    var file = new FileInfoIFileAdapter(new FileInfo(this._settingsProvider.PersisterCsvPath),
                        Encoding.UTF8);
                    return new CsvPersister(file, this._settingsProvider.CsvSeparator);
                case "Memory":
                    return new MemoryDecisionPersister();
                default:
                    throw new NotSupportedException(this._settingsProvider.PersisterType + " is unknown DecisionPersister type");
            }
        }
    }
}
