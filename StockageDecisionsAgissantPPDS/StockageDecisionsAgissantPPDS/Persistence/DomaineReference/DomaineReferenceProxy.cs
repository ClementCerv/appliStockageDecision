using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Normacode.Patterns;
using StockageDecisionsAgissantPPDS.Model;

namespace StockageDecisionsAgissantPPDS.Persistence.DomaineReference
{
    /// <summary>
    /// Proxy de <see cref="IDomaineReference"/>
    /// </summary>
    public class DomaineReferenceProxy : IDomaineReference, INotifyValueChanged<IDomaineReference>
    {
        private static readonly IEnumerable<Domaine> Empty = new Domaine[0];
        private IDomaineReference _proxied;

        /// <summary>
        /// Instance derrière le proxy
        /// </summary>
        public IDomaineReference Proxied
        {
            set
            {
                if(this._proxied != null) this._proxied.CollectionChanged -= this.OnCollectionChanged;
                this._proxied = value;
                this._proxied.CollectionChanged += this.OnCollectionChanged;

                this.ValueChanged?.Invoke(this, value);
                
            } 
        }

        /// <summary>
        /// ValueChangedNoArgs
        /// </summary>
        public event ValueChanged ValueChangedNoArgs;

        /// <summary>
        /// ValueChanged
        /// </summary>
        public event ValueChanged<IDomaineReference> ValueChanged;

        /// <summary>
        /// Constructeur
        /// </summary>
        public DomaineReferenceProxy()
        {
            this.ValueChanged += (sender, value) =>
            {
                this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                this.ValueChangedNoArgs?.Invoke();
            };
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            this.CollectionChanged?.Invoke(sender, args);
        }

        /// <summary>
        /// CollectionChanged
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        IEnumerator<Domaine> IEnumerable<Domaine>.GetEnumerator()
        {
            try
            {
                return this._proxied?.GetEnumerator() ?? Empty.GetEnumerator();
            }
            catch
            {
                return Empty.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            try
            {
                return this._proxied?.GetEnumerator() ?? Empty.GetEnumerator();
            }
            catch
            {
                return Empty.GetEnumerator();
            }
        }
    }
}
