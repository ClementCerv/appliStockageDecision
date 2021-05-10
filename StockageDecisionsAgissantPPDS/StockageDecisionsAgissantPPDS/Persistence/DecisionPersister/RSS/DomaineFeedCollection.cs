using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Normacode.IO;
using StockageDecisionsAgissantPPDS.Model;
using StockageDecisionsAgissantPPDS.Model.Decision;
using StockageDecisionsAgissantPPDS.Persistence.DomaineReference;

namespace StockageDecisionsAgissantPPDS.Persistence.DecisionPersister.RSS
{
    public class DomaineFeedCollection : IDomaineReference
    {
        private readonly IDirectory _directory;
        private readonly Dictionary<Domaine, DomaineRssFeed> _cache;
        private readonly Uri _baseUri;

        public DomaineFeedCollection(IDirectory directory, Uri baseUri)
        {
            this._directory = directory;
            this._baseUri = baseUri;

            this._cache = DomaineRssFeed.ReadDirectory(directory)
                .ToDictionary(feed => feed.Domaine, feed => feed);
        }

        public DomaineRssFeed Find(Domaine domaine)
        {
            if (!this._cache.ContainsKey(domaine))
            {
                this._cache[domaine] = new DomaineRssFeed(this._directory, domaine, this._baseUri);
            }

            return this._cache[domaine];
        }

        public void RemoveAllOccurences(DecisionModel model)
        {
            string hashCodeString = model.GetHashCode().ToString();

            foreach (DomaineRssFeed domaineRssFeed in this._cache.Values)
            {
                domaineRssFeed.DoSomething(feed =>
                {
                    if (feed.item == null) return;
                    feed.item = feed.item.Where(item => item.guid.Value != hashCodeString).ToArray();
                });
            }
        }

        public IEnumerator<Domaine> GetEnumerator() => this._cache.Keys.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
