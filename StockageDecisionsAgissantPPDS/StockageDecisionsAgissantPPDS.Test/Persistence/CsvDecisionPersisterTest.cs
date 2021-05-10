using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Normacode.IO;
using Normacode.IO.Adapters;
using StockageDecisionsAgissantPPDS.Model.Decision;
using StockageDecisionsAgissantPPDS.Persistence.DecisionPersister;

namespace StockageDecisionsAgissantPPDS.Test.Persistence
{
    [TestClass]
    public class CsvDecisionPersisterTest : DecisionPersisterTestAbstract
    {
        private static readonly string TestingDirectoryPath = Path.Combine(Path.GetTempPath(),
            Assembly.GetExecutingAssembly().FullName);

        private const string CsvFileName = nameof(CsvDecisionPersisterTest) + ".csv";
        private static readonly DirectoryInfo TestingDirectoryInfo = new DirectoryInfo(TestingDirectoryPath);
        private static readonly FileInfo TestingFileInfo = new FileInfo(Path.Combine(TestingDirectoryInfo.FullName, CsvFileName));
        private static readonly IDirectory TestingDirectory = new DirectoryInfoIDirectoryAdapter(TestingDirectoryInfo);
        private static readonly IFile TestingFile = new FileInfoIFileAdapter(TestingFileInfo, Encoding.UTF8);

        protected override IDecisionPersister FactoryPersister()
        {
            TestingFileInfo.Delete();
            TestingFileInfo.Create().Close();
            return new CsvPersister(TestingFile, ';');
        }

        [TestMethod]
        [TestCategory("Defects")]
        public async void Export_NameFile_FileNotFound()
        {
            DecisionModel decision = this.Prototype;
            var csvPersister = new CsvPersisterCreateFileDecorator(TestingDirectory, CsvFileName, DecisionModel.Separator);

            // Bloc 2 : Exporter tout
            {
                var decisionList = new List<DecisionModel> { decision };

                foreach (DecisionModel decisionModel in decisionList)
                {
                    await csvPersister.StoreDecisionAsync(decisionModel);
                }
            }

            // Bloc final : vérification que le fichier csv existe
            {
                DecisionModel csv = (await csvPersister.FetchAllAsync()).Single();
                Assert.AreEqual(decision, csv);
            }
        }
    }
}
