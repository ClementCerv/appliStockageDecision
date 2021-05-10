using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockageDecisionsAgissantPPDS.Model;
using StockageDecisionsAgissantPPDS.Model.Decision;
using StockageDecisionsAgissantPPDS.Persistence.DecisionPersister;

namespace StockageDecisionsAgissantPPDS.Test.Persistence
{
    [TestClass]
    public abstract class DecisionPersisterTestAbstract
    {
        protected static readonly Uri MockUri = new Uri("http://localhost.dev/");

        protected DecisionModel Prototype => new DecisionModel(
            string.Empty,
            string.Empty,
            DateTime.Today,
            MockUri,
            TestingDomaineGenerator.DifferentByName().Take(1).ToArray());

        private IDecisionPersister Persister { get; set; }

        protected abstract IDecisionPersister FactoryPersister();

        private TestingDecisionModelGenerator Generators { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            this.Persister = this.FactoryPersister();
            this.Generators = new TestingDecisionModelGenerator(this.Prototype);
        }

        /// <summary>
        /// Rien n'existe initialement
        /// </summary>
        [TestMethod]
        public async Task FetchAll_ReturnsNothing_Initially()
        {
            Assert.IsFalse((await this.Persister.FetchAllAsync()).Any());
        }

        /// <summary>
        /// Après un store, FetchAll renvoie le seul contenu
        /// </summary>
        [TestMethod]
        public async Task FetchAll_ReturnsStored_AfterStore()
        {
            DecisionModel decision = this.Prototype;
            await this.Persister.StoreDecisionAsync(decision);
            Assert.AreEqual(decision, (await this.Persister.FetchAllAsync()).Single());
        }

        /// <summary>
        /// Si on stocke un doublon, il n'apparaît pas, seul le premier est renvoyé
        /// </summary>
        [TestMethod]
        public async Task FetchAll_ReturnsOneStored_AfterTwoIdenticalStore()
        {
            DecisionModel decision = this.Prototype;
            await this.Persister.StoreDecisionAsync(decision);
            await this.Persister.StoreDecisionAsync(decision);

            Assert.AreEqual(this.Prototype, (await this.Persister.FetchAllAsync()).Single());
        }

        /// <summary>
        /// Si on stocke deux instances différences, elles sont renvoyées par FetchAll
        /// </summary>
        [TestMethod]
        public async Task FetchAll_ReturnsStored_AfterTwoDifferentStore()
        {
            var instances = this.Generators.DifferentByTitleGenerator().Take(2).ToArray();

            await this.Persister.StoreDecisionAsync(instances.First());
            await this.Persister.StoreDecisionAsync(instances.Last());

            CollectionAssert.AreEquivalent(instances, (await this.Persister.FetchAllAsync()).ToArray());
        }
        
        /// <summary>
        /// Quand rien n'a été stocké, exists renvoie faux
        /// </summary>
        [TestMethod]
        public async Task Exists_ReturnsFalse_Initially()
        {
            Assert.IsFalse(await this.Persister.ExistsAsync(this.Prototype));
        }

        /// <summary>
        /// Exists renvoie true lorsqu'une décision a été ajoutée
        /// </summary>
        [TestMethod]
        public async Task Exists_ReturnsTrue_IfAdded()
        {
            await this.Persister.StoreDecisionAsync(this.Prototype);
            Assert.IsTrue(await this.Persister.ExistsAsync(this.Prototype));
        }

        /// <summary>
        /// Exists renvoie false lorsqu'une décision différente a été ajoutée
        /// </summary>
        [TestMethod]
        public async Task Exists_ReturnsFalse_IfAnotherAdded()
        {
            var decisions = this.Generators.DifferentByTitleGenerator().Take(2).ToArray();

            DecisionModel decision = decisions.First();
            DecisionModel anotherDecision = decisions.Last();

            await this.Persister.StoreDecisionAsync(decision);

            Assert.IsFalse(await this.Persister.ExistsAsync(anotherDecision));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task CannotStoreDecision_WithZeroDomain()
        {
            DecisionModel decision = this.Generators.WithSpecificDomainesGenerator(new Domaine[0][]).Take(1).Single();
            
            await this.Persister.StoreDecisionAsync(decision);

            Assert.Fail("Exception non-obtenue");
        }

        [TestMethod]
        [TestCategory("Defect")]
        public async Task DeleteDecision_WithMoreThanOneDomain_ThenFetch_DoesNotFail()
        {
            DecisionModel currentPrototype = this.Prototype;
            Domaine firstDomain = currentPrototype.Domaines.Single();
            Domaine secondDomain = TestingDomaineGenerator.DifferentByName().Take(1).Single();

            // Création initiale de la décision avec un domaine
            {
                DecisionModel decision = currentPrototype;
                Assert.AreEqual(1, decision.Domaines.Count);
                await this.Persister.StoreDecisionAsync(decision);
            }
            
            // Bloc 2 : Ajout d'un second domaine
            {
                DecisionModel decision = (await this.Persister.FetchAllAsync()).Single();
                decision.Domaines.Add(secondDomain);
                await this.Persister.StoreDecisionAsync(decision);
            }

            // Bloc 3 : Suppresion de la décision
            {
                DecisionModel decision = (await this.Persister.FetchAllAsync()).Single();
                decision.EstSupprimée = true;
                await this.Persister.StoreDecisionAsync(decision);
            }

            // Bloc final : vérification que la décision est bien supprimée et dans deux domaines
            {
                DecisionModel decision = (await this.Persister.FetchAllAsync()).Single();

                Assert.IsTrue(decision.EstSupprimée);
                Assert.IsTrue(decision.Domaines.Contains(firstDomain));
                Assert.IsTrue(decision.Domaines.Contains(secondDomain));
            }
        }

        [TestMethod]
        [TestCategory("Defect")]
        public async Task RemoveDomaine_RemovesDomainInStorage()
        {
            DecisionModel currentPrototype = this.Prototype;
            Domaine firstDomain = currentPrototype.Domaines.Single();
            Domaine secondDomain = TestingDomaineGenerator.DifferentByName().Take(1).Single();

            // Bloc 1 : création initiale de la décision
            {
                DecisionModel decision = currentPrototype;
                decision.Domaines.Add(secondDomain);

                Assert.AreEqual(2, decision.Domaines.Count);
                await this.Persister.StoreDecisionAsync(decision);
            }

            // Bloc 2 : Enlever un domaine à la décision
            {
                DecisionModel decision = (await this.Persister.FetchAllAsync()).Single();
                decision.Domaines.Remove(secondDomain);
                await this.Persister.StoreDecisionAsync(decision);
            }

            // Bloc final : vérification que la décision ne contient qu'un seul domaine
            {
                DecisionModel decision = (await this.Persister.FetchAllAsync()).Single();
                Assert.IsTrue(decision.Domaines.Contains(firstDomain));
                Assert.IsFalse(decision.Domaines.Contains(secondDomain));
            }
        }

        [TestMethod]
        [TestCategory("Defect")]
        public async Task DeleteLastDecision_NotDelete()
        {
            var testedDecisions = this.Generators.DifferentByTitleGenerator().Take(2).ToArray();
            await this.Persister.StoreDecisionAsync(testedDecisions.First());
            await this.Persister.StoreDecisionAsync(testedDecisions.Last());

            {
                var decisions = await this.Persister.FetchAllAsync();
                foreach (DecisionModel decision in decisions)
                {
                    decision.EstSupprimée = true;
                    await this.Persister.StoreDecisionAsync(decision);
                }
            }

            {
                var decisions = await this.Persister.FetchAllAsync();
                Assert.IsTrue(decisions.All(decision => decision.EstSupprimée));
            }
        }
    }
}
