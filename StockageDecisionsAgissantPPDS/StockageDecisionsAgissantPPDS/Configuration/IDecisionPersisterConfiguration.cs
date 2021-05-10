namespace StockageDecisionsAgissantPPDS.Configuration
{
    /// <summary>
    /// Configuration du DecisionPersister
    /// </summary>
    public interface IDecisionPersisterConfiguration
    {
        /// <summary>
        /// Type de persister
        /// </summary>
        string PersisterType { get; }

        /// <summary>
        /// Chemin CSV
        /// </summary>
        string PersisterCsvPath { get; }

        /// <summary>
        /// Séparateur Csv
        /// </summary>
        char CsvSeparator { get; }
    }
}
