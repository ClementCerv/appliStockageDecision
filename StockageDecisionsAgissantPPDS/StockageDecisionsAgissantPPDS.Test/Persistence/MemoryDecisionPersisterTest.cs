using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockageDecisionsAgissantPPDS.Persistence.DecisionPersister;

namespace StockageDecisionsAgissantPPDS.Test.Persistence
{
    /// <summary>
    /// Tests pour <see cref="MemoryDecisionPersister"/>
    /// </summary>
    [TestClass]
    public class MemoryDecisionPersisterTest : DecisionPersisterTestAbstract
    {
        protected override IDecisionPersister FactoryPersister()
        {
            return new MemoryDecisionPersister();
        }
    }
}
