using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockageDecisionsAgissantPPDS.Model;

namespace StockageDecisionsAgissantPPDS.Test
{
    /// <summary>
    /// Tests pour <see cref="Domaine"/>
    /// </summary>
    [TestClass]
    public class DomaineTest
    {
        /// <summary>
        /// Vérifie que la construction avec un nom vide plante
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Construction_IfNameWhitespace_Throws()
        {
            var domaine = new Domaine(string.Empty);
            Assert.IsNotNull(domaine);
        }
    }
}
