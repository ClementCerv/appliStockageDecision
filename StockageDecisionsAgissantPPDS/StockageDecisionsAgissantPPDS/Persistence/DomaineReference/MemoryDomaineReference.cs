using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using StockageDecisionsAgissantPPDS.Model;

namespace StockageDecisionsAgissantPPDS.Persistence.DomaineReference
{
    /// <summary>
    /// DomaineReference stocké en mémoire
    /// </summary>
    public class MemoryDomaineReference : IDomaineReference
    {
        private readonly IEnumerable<Domaine> _domaines;

        /// <summary>
        /// Constructeur
        /// </summary>
        public MemoryDomaineReference()
        {
            this._domaines = new Domaine[0];
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        public MemoryDomaineReference(IEnumerable<Domaine> domaines)
        {
            this._domaines = domaines;
        }

        /// <summary>
        /// CollectionChanged
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        
        IEnumerator<Domaine> IEnumerable<Domaine>.GetEnumerator() => this._domaines.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this._domaines.GetEnumerator();
    }
}
