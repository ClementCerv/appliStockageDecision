using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockageDecisionsAgissantPPDS.Model.Decision;

namespace StockageDecisionsAgissantPPDS.Test
{
    [TestClass]
    public class Defects
    {
        /// <summary>
        /// Souci apparu précédemment, permettant de construire une mauvaise Uri par binding, 
        /// puis de la stocker à cause d'une conversion implicite bizarre.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(UriFormatException))]
        public void CannotBuildBadUri()
        {
            const string badUriString = "efeulfhe";
            Assert.IsNotNull(new DecisionBuilder {Lien = badUriString});
        }

        [TestMethod]
        [ExpectedException(typeof(UriFormatException))]
        public void CannotBuildEmptyUri()
        {
            const string emptyUriString = "";
            DecisionBuilder decisionBuilder = new DecisionBuilder();

            try
            {
                decisionBuilder.Lien = emptyUriString;
            }
            catch (UriFormatException)
            {
                Assert.IsNotNull(decisionBuilder.Build());
            }
        }
    }
}