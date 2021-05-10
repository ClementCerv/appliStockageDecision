using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Normacode.RSS;
using StockageDecisionsAgissantPPDS.Model.Decision;
using StockageDecisionsAgissantPPDS.Persistence.DecisionPersister.RSS;

namespace StockageDecisionsAgissantPPDS.Test.Persistence.RSS
{
    /// <summary>
    /// Tests pour <see cref="DecisionModelRssItemConverter"/>
    /// </summary>
    [TestClass]
    public class DecisionModelRssItemConverterTest
    {
        private static readonly Uri MockUri = new Uri("http://localhost.dev/");

        private static RssItem PrototypeItem()
        {
            DecisionModel decision = PrototypeDecision();
            var item = new RssItem
            {
                title = decision.Titre,
                description = decision.Description,
                link = decision.Lien.ToString(),
                guid = new Normacode.RSS.Guid { isPermaLink = false, Value = decision.GetHashCode().ToString() }
            };
            item.SetPubDate(decision.Date);
            return item;
        }

        private static DecisionModel PrototypeDecision()
        {
            return new DecisionModel(string.Empty, string.Empty, DateTime.Now, MockUri, TestingDomaineGenerator.DifferentByName().Take(1).ToArray());
        }

        [TestMethod]
        public void BuildsItemTitle_FromDecisionTitre_WithRemoveIndicatorIfRemoved()
        {
            DecisionModel notRemovedDecision = PrototypeDecision();
            DecisionModel removedDecision = PrototypeDecision();
            removedDecision.EstSupprimée = true;

            RssItem itemNotRemoved = new DecisionModelRssItemConverter(notRemovedDecision).BuildItem();
            RssItem itemRemoved = new DecisionModelRssItemConverter(removedDecision).BuildItem();

            Assert.AreEqual(string.Empty, itemNotRemoved.title);
            Assert.AreEqual(DecisionModelRssItemConverter.RemovedIndicator, itemRemoved.title);
        }

        [TestMethod]
        public void BuildsDecisionTitre_FromItemTitle_WithoutRemoveIndicator()
        {
            RssItem itemNotRemoved = PrototypeItem();

            RssItem itemRemoved = PrototypeItem();
            itemRemoved.title = DecisionModelRssItemConverter.RemovedIndicator;

            DecisionModel notRemovedDecision = new DecisionModelRssItemConverter(itemNotRemoved).BuildModel();
            DecisionModel removedDecision = new DecisionModelRssItemConverter(itemRemoved).BuildModel();

            Assert.AreEqual(string.Empty, notRemovedDecision.Titre);
            Assert.AreEqual(string.Empty, removedDecision.Titre);
        }

        [TestMethod]
        public void BuildsDecisionDescription_FromItemDescription()
        {
            RssItem item = PrototypeItem();
            DecisionModel decision = new DecisionModelRssItemConverter(item).BuildModel();

            Assert.AreEqual(item.description, decision.Description);
        }

        [TestMethod]
        public void BuildsDecisionDate_FromItemPubDate_WithoutTimePart()
        {
            RssItem badDateItem = PrototypeItem();
            badDateItem.SetPubDate(DateTime.Now);

            DecisionModel builtDecision = new DecisionModelRssItemConverter(badDateItem).BuildModel();

            Assert.AreEqual(builtDecision.Date.Date, builtDecision.Date);
        }

        [TestMethod]
        public void BuildsDecisionLien_FromItemLink()
        {
            RssItem badDateItem = PrototypeItem();
            DecisionModel builtDecision = new DecisionModelRssItemConverter(badDateItem).BuildModel();

            Assert.AreEqual(badDateItem.link, builtDecision.Lien.ToString());
        }

        [TestMethod]
        public void BuildsDecisionEstSupprimé_FromItemTitle_WhenRemoveIndicatorPresent()
        {
            RssItem itemNotRemoved = PrototypeItem();

            RssItem itemRemoved = PrototypeItem();
            itemRemoved.title = DecisionModelRssItemConverter.RemovedIndicator;

            DecisionModel notRemovedDecision = new DecisionModelRssItemConverter(itemNotRemoved).BuildModel();
            DecisionModel removedDecision = new DecisionModelRssItemConverter(itemRemoved).BuildModel();

            Assert.IsFalse(notRemovedDecision.EstSupprimée);
            Assert.IsTrue(removedDecision.EstSupprimée);
        }
    }
}
