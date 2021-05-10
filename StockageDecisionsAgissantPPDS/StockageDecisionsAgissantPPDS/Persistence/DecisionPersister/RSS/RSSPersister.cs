using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Normacode.IO;
using Normacode.RSS;
using StockageDecisionsAgissantPPDS.Model;
using StockageDecisionsAgissantPPDS.Model.Decision;
using StockageDecisionsAgissantPPDS.Persistence.DomaineReference;

namespace StockageDecisionsAgissantPPDS.Persistence.DecisionPersister.RSS
{
    /// <summary>
    /// Stocke les décisions au format rss
    /// </summary>
    public class RSSPersister : IDecisionPersister, IDomaineReference
    {
        private readonly DomaineFeedCollection _channels;
        private readonly string _stringRepresentation;

        /// <summary>
        /// Constructeur
        /// </summary>
        public RSSPersister(IDirectory feedsStorageDirectory, Uri baseUri)
        {
            this._stringRepresentation = nameof(RSSPersister) + " at " + feedsStorageDirectory; 
            this._channels = new DomaineFeedCollection(feedsStorageDirectory, baseUri);
            this._channels.CollectionChanged += (sender, args) => this.CollectionChanged?.Invoke(sender, args);
        }
        
        private void StoreDecision(DecisionModel decision)
        {
            var converter = new DecisionModelRssItemConverter(decision);
            RssItem item = converter.BuildItem();

            this._channels.RemoveAllOccurences(decision);

            foreach (Domaine decisionDomaine in decision.Domaines)
            {
                DomaineRssFeed feed = this._channels.Find(decisionDomaine);
                feed.DoSomething(channel =>
                {
                    channel.Add(item);
                });
            }
        }
        
        private IEnumerable<DecisionModel> FetchAll()
        {
            var decisionBuilders = new List<DecisionModelRssItemConverter>();

            foreach (Domaine domaine in this._channels)
            {
                DomaineRssFeed feed = this._channels.Find(domaine);

                foreach (RssItem item in feed.GetSomething(channel => channel.Items()))
                {
                    DecisionModelRssItemConverter builder = decisionBuilders
                        .SingleOrDefault(decisionBuilder => decisionBuilder.DesignatesSameDecision(item));

                    if (builder == null)
                    {
                        builder = new DecisionModelRssItemConverter(item);
                        decisionBuilders.Add(builder);
                    }

                    builder.Domaines.Add(domaine);  
                }
            }

            return decisionBuilders.Select(builder => builder.BuildModel());
        }

        public async Task StoreDecisionAsync(DecisionModel decision)
        {
            await Task.Run(() => this.StoreDecision(decision));
        }

        public async Task<IEnumerable<DecisionModel>> FetchAllAsync()
        {
            return await Task.Run(() => this.FetchAll());
        }

        public async Task<bool> ExistsAsync(IDecisionModelIdentity decision)
        {
            return (await this.FetchAllAsync()).Any(decision.Equals);
        }

        /// <inheritdoc />
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        IEnumerator<Domaine> IEnumerable<Domaine>.GetEnumerator() => this._channels.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this._channels.GetEnumerator();

        public override string ToString()
        {
            return this._stringRepresentation;
        }
    }
}