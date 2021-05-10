using System;
using System.Collections.Generic;
using StockageDecisionsAgissantPPDS.Model;

namespace StockageDecisionsAgissantPPDS.Test
{
    public static class TestingDomaineGenerator
    {
        public static IEnumerable<Domaine> DifferentByName()
        {
            while(true) yield return new Domaine(Guid.NewGuid().ToString());
            // ReSharper disable once IteratorNeverReturns
        }
    }
}
