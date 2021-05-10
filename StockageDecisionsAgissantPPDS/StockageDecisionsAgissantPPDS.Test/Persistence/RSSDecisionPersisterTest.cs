using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Normacode.IO;
using Normacode.IO.Adapters;
using StockageDecisionsAgissantPPDS.Persistence.DecisionPersister;
using StockageDecisionsAgissantPPDS.Persistence.DecisionPersister.RSS;

namespace StockageDecisionsAgissantPPDS.Test.Persistence
{
    [TestClass]
    public class RSSDecisionPersisterTest : DecisionPersisterTestAbstract
    {
        private static readonly string TestingPath = Path.Combine(Path.GetTempPath(),
            Assembly.GetExecutingAssembly().FullName, nameof(RSSDecisionPersisterTest));

        private static readonly DirectoryInfo TestingDirectoryInfo = new DirectoryInfo(TestingPath);
        private static readonly IDirectory TestingDirectory = new DirectoryInfoIDirectoryAdapter(TestingDirectoryInfo);

        /// <inheritdoc />
        protected override IDecisionPersister FactoryPersister()
        {
            TestingDirectoryInfo.Create();
            return new RSSPersister(TestingDirectory, MockUri);
        }
        
        [TestCleanup]
        public void Cleanup()
        {
            TestingDirectoryInfo.Delete(true);
        }
    }
}
