using System.Collections.Generic;
using System.Linq;
using StockageDecisionsAgissantPPDS.Model;
using StockageDecisionsAgissantPPDS.Model.Decision;

namespace StockageDecisionsAgissantPPDS.Test
{
    public class TestingDecisionModelGenerator
    {
        private readonly DecisionModel _prototype;

        public DecisionModel Prototype => 
            new DecisionModel(
                this._prototype.Titre, 
                this._prototype.Description, 
                this._prototype.Date, 
                this._prototype.Lien, 
                this._prototype.Domaines.ToArray())
            { EstSupprimée = this._prototype.EstSupprimée };

        public TestingDecisionModelGenerator(DecisionModel prototypeValues)
        {
            this._prototype = prototypeValues;
        }

        public IEnumerable<DecisionModel> DifferentByTitleGenerator()
        {
            for (var letterIndex = 'a'; letterIndex <= 'z'; letterIndex++)
            {
                yield return new DecisionModel(
                    letterIndex.ToString(), 
                    this._prototype.Description, 
                    this._prototype.Date, 
                    this._prototype.Lien, 
                    this._prototype.Domaines.ToArray())
                { EstSupprimée = this._prototype.EstSupprimée };
            }
        }

        public IEnumerable<DecisionModel> WithSpecificDomainesGenerator(IEnumerable<Domaine[]> domaineGenerator)
        {
            return domaineGenerator.Select(domaines => new DecisionModel(
                    this._prototype.Titre, 
                    this._prototype.Description, 
                    this._prototype.Date, 
                    this._prototype.Lien, 
                    domaines
                )
                { EstSupprimée = this._prototype.EstSupprimée });
        }
    }
}
