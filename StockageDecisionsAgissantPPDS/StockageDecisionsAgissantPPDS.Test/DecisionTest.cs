using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockageDecisionsAgissantPPDS.Model;
using StockageDecisionsAgissantPPDS.Model.Decision;

namespace StockageDecisionsAgissantPPDS.Test
{
    /// <summary>
    /// Tests pour la classe <see cref="DecisionModel"/>
    /// </summary>
    [TestClass]
    public class DecisionModelTest
    {
        private static readonly Domaine MockDomain = new Domaine("mock");
        private static readonly Uri MockUri = new Uri("http://localhost.dev/");
        private static readonly DecisionModel Prototype = new DecisionModel(string.Empty, string.Empty, DateTime.Today, MockUri, new[] { MockDomain });

        private TestingDecisionModelGenerator Generators { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            this.Generators = new TestingDecisionModelGenerator(Prototype);
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.Generators = null;
        }

        /// <summary>
        /// Vérifie que la construction avec un séparateur dans le titre plante
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Construction_WithSeparatorInTitle_Throws()
        {
            var decision = new DecisionModel(DecisionModel.Separator.ToString(), string.Empty, DateTime.Today, new Uri("http://localhost"), new Domaine[1]);
            Assert.IsNotNull(decision);
        }

        /// <summary>
        /// Vérifie que la construction avec un séparateur dans la description plante
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Construction_WithSeparatorInDescription_Throws()
        {
            var decision = new DecisionModel(string.Empty, DecisionModel.Separator.ToString(), DateTime.Today, new Uri("http://localhost"), new Domaine[1]);
            Assert.IsNotNull(decision);
        }

        /// <summary>
        /// Vérifie qu'une décision et elle-même supprimée sont égales
        /// </summary>
        [TestMethod]
        public void Decision_AndTheSameDeleted_AreEqual()
        {
            DecisionModel notDeleted = this.Generators.Prototype;

            DecisionModel deleted = this.Generators.Prototype;
            deleted.EstSupprimée = true;

            Assert.AreEqual(notDeleted, deleted);
        }
    }
}