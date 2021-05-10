using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Normacode.IO;
using Normacode.IO.Adapters;
using StockageDecisionsAgissantPPDS.Model;
using StockageDecisionsAgissantPPDS.Model.Decision;
using StockageDecisionsAgissantPPDS.Persistence.DecisionPersister;
using StockageDecisionsAgissantPPDS.Test.Persistence;

// ReSharper disable MissingXmlDoc

namespace StockageDecisionsAgissantPPDS.Test
{
    [TestClass]
    public class PersistenceTest
    {
        private static readonly string TestingPath = Path.Combine(Path.GetTempPath(),
            Assembly.GetExecutingAssembly().FullName, nameof(PersistenceTest));

        private static readonly DirectoryInfo TestingDirectoryInfo = new DirectoryInfo(TestingPath);
        private static readonly IDirectory TestingDirectory = new DirectoryInfoIDirectoryAdapter(TestingDirectoryInfo);

        private static readonly Domaine MockDomain = new Domaine("mock");
        private static readonly Uri MockUri = new Uri("http://localhost.dev/");
        private static readonly DecisionModel Prototype = new DecisionModel(string.Empty, string.Empty, DateTime.Today, MockUri, new [] { MockDomain });

        private RSSPersister _storage;
        private TestingDecisionModelGenerator _generators;

        [TestInitialize]
        public void Initialize()
        {
            TestingDirectoryInfo.Create();
            this._storage = new RSSPersister(TestingDirectory, MockUri);

            this._generators = new TestingDecisionModelGenerator(Prototype);
        }
        
        [TestCleanup]
        public void Cleanup()
        {
            this._generators = null;
            TestingDirectoryInfo.Delete(true);
        }

        [TestMethod]
        public void CanStoreAndFetch()
        {
            var decision = this._generators.DifferentByTitleGenerator().First();
            this._storage.StoreDecision(decision);

            var fetchedDecisions = this._storage.FetchAll();
            var fetchedDecision = fetchedDecisions.First();

            Assert.AreEqual(decision, fetchedDecision);
        }

        [TestMethod]
        public void Decision_DoesNotExists_Initially()
        {
            var decision = this._generators.DifferentByTitleGenerator().First();
            Assert.IsFalse(this._storage.Exists(decision));
        }

        [TestMethod]
        public void Decision_DoesNotExists_IfAnotherAdded()
        {
            var decisions = this._generators.DifferentByTitleGenerator().Take(2).ToArray();

            var decision = decisions.First();
            var anotherDecision = decisions.Last();

            this._storage.StoreDecision(anotherDecision);

            Assert.IsFalse(this._storage.Exists(decision));
        }

        [TestMethod]
        public void Decision_Exists_IfAdded()
        {
            var decision = this._generators.DifferentByTitleGenerator().First();
            this._storage.StoreDecision(decision);

            Assert.IsTrue(this._storage.Exists(decision));
        }

        [TestMethod]
        public void CanStoreAndFetchMultiple()
        {
            var decisions = this._generators.DifferentByTitleGenerator().Take(2).ToArray();

            this._storage.StoreDecision(decisions.First());
            this._storage.StoreDecision(decisions.Last());

            var fetchedDecisions = this._storage.FetchAll();
            
            CollectionAssert.AreEqual(decisions, fetchedDecisions.ToArray());
        }
    }
}
