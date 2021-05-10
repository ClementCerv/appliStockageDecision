using Normacode.Patterns;
using StockageDecisionsAgissantPPDS.Model;

namespace StockageDecisionsAgissantPPDS.Persistence.DomaineReference
{
    /// <summary>
    /// Fournit un accès à une liste de tous les domaines existants
    /// </summary>
    public interface IDomaineReference : IObservableEnumerable<Domaine>
    {
    }
}
