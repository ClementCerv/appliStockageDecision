using System.Collections.Generic;
using StockageDecisionsAgissantPPDS.Model;

namespace StockageDecisionsAgissantPPDS.Configuration
{
    /// <summary>
    /// Paramétrage relatif aux domaines
    /// </summary>
    public interface IDomaineReferenceConfiguration
    {
        /// <summary>
        /// Source de données fournissant les domaines
        /// </summary>
        string PersisterDomainesSource { get; }

        /// <summary>
        /// Liste prédéfinie de domaine
        /// </summary>
        IEnumerable<Domaine> PersisterDomainesList { get; }
    }
}
