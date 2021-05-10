using Normacode.IO;

namespace StockageDecisionsAgissantPPDS.Persistence.DecisionPersister
{
    public class CsvPersisterCreateFileDecorator : CsvPersister
    {
        public CsvPersisterCreateFileDecorator(IDirectory directory, string filename, char separator) 
            : base(directory.CreateOrReturnFile(filename), separator)
        {
        }
    }
}
