using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Normacode.RSS;
using StockageDecisionsAgissantPPDS.Model.Decision;
using StockageDecisionsAgissantPPDS.Persistence.DecisionPersister.RSS;

namespace StockageDecisionsAgissantPPDS.Test.Persistence.RSS
{
    /// <summary>
    /// Tests pour <see cref="DecisionModelRssItemBuilder"/>
    /// </summary>
    [TestClass]
    public class DecisionModelRssItemBuilderTest
    {
        private static RssItem Prototype()
        {
            var item = new RssItem
            {
                title = string.Empty,
                description = string.Empty,
                link = new Uri("http://localhost.dev/").ToString()
            };
            item.SetPubDate(DateTime.Today);
            return item;
        }

        [TestMethod]
        public void BuildsDecisionTitre_FromItemTitle_WithoutRemoveIndicator()
        {
            RssItem itemNotRemoved = Prototype();

            RssItem itemRemoved = Prototype();
            itemRemoved.title = DecisionModelRssItemBuilder.RemovedIndicator;

            DecisionModel notRemovedDecision = new DecisionModelRssItemBuilder(itemNotRemoved).Build();
            DecisionModel removedDecision = new DecisionModelRssItemBuilder(itemRemoved).Build();

            Assert.AreEqual(string.Empty, notRemovedDecision.Titre);
            Assert.AreEqual(string.Empty, removedDecision.Titre);
        }

        [TestMethod]
        public void BuildsDecisionDate_FromItemPubDate_WithoutTimePart()
        {
            RssItem badDateItem = Prototype();
            badDateItem.SetPubDate(DateTime.Now);

            DecisionModel builtDecision = new DecisionModelRssItemBuilder(badDateItem).Build();

            Assert.AreEqual(builtDecision.Date.Date, builtDecision.Date);
        }

        [TestMethod]
        public void BuildsDecisionLien_FromItemLink()
        {
            RssItem badDateItem = Prototype();
            DecisionModel builtDecision = new DecisionModelRssItemBuilder(badDateItem).Build();

            Assert.AreEqual(badDateItem.link, builtDecision.Lien?.ToString());
        }

        [TestMethod]
        public void BuildsDecisionEstSupprimé_FromItemTitle_WhenRemoveIndicatorPresent()
        {
            RssItem itemNotRemoved = Prototype();

            RssItem itemRemoved = Prototype();
            itemRemoved.title = DecisionModelRssItemBuilder.RemovedIndicator;

            DecisionModel notRemovedDecision = new DecisionModelRssItemBuilder(itemNotRemoved).Build();
            DecisionModel removedDecision = new DecisionModelRssItemBuilder(itemRemoved).Build();

            Assert.IsFalse(notRemovedDecision.EstSupprimée);
            Assert.IsTrue(removedDecision.EstSupprimée);
        }
    }
}
