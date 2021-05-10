using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using JetBrains.Annotations;
using Normacode.IO;
using Normacode.RSS;
using StockageDecisionsAgissantPPDS.Model;
using StockageDecisionsAgissantPPDS.Model.Decision;
using StockageDecisionsAgissantPPDS.Persistence.DomaineReference;
using Guid = Normacode.RSS.Guid;

namespace StockageDecisionsAgissantPPDS.Persistence.DecisionPersister
{
    /// <summary>
    /// Stocke les décisions au format rss
    /// </summary>
    public class RSSPersister : IDecisionPersister, IDomaineReference
    {
        private const string RemovedIndicator = "[SUPPRIMÉ] ";
        private static readonly Func<IFile, bool> XmlFileFilter = file => file.Name.EndsWith(".xml"); 
        private readonly IDirectory _feedsStorageDirectory;
        private readonly Uri _baseUri;

        /// <inheritdoc />
        public void StoreDecision(DecisionModel decision)
        {
            this.RemoveAllOccurences(decision);

            string titre = decision.EstSupprimée ? RemovedIndicator + decision.Titre : decision.Titre;
            var item = new RssItem
            {
                title = titre,
                pubDate = decision.Date.ToString("ddd, dd MMM yyyy HH:mm:ss"),
                description = decision.Description,
                link = decision.Lien?.ToString(),
                guid = new Guid { isPermaLink = false, Value = decision.GetHashCode().ToString() }
            };

            foreach (Domaine decisionDomaine in decision.Domaines)
            {
                RssChannel channel = this.OpenOrCreateChannelForDomaine(decisionDomaine);
                channel.Add(item);

                var rss = new Rss { channel = new [] { channel }};
                var serializer = new XmlSerializer(typeof(Rss));

                IFile file = this._feedsStorageDirectory.CreateOrReturn(FileNameForDomaine(decisionDomaine));

                using (StreamWriter writer = file.CreateWriter())
                    serializer.Serialize(writer, rss);
            }
        }

        private void RemoveAllOccurences(DecisionModel decision)
        {
            foreach (RssChannel channel in this.Channels)
            {
                var occurences = channel.Items()
                    .Where(element => ItemsReferesToDecision(element, decision))
                    .ToArray();

                foreach (RssItem channelItem in occurences)
                {
                    channel.Remove(channelItem);
                }
            }
        }

        private static bool ItemsReferesToDecision(RssItem item, DecisionModel decision)
        {
            DecisionModel itemDecision = FromItem(item).Build();
            return itemDecision.Equals(decision);
        }

        private RssChannel OpenOrCreateChannelForDomaine(Domaine domaine)
        {
            return this.GetChannelMatchingDomaine(domaine) ?? this.FactoryNewChannel(domaine);
        }

        [CanBeNull]
        private RssChannel GetChannelMatchingDomaine(Domaine domaine)
        {
            return this.Channels.FirstOrDefault(channel => channel.title == domaine.Nom);
        }

        private RssChannel FactoryNewChannel(Domaine domaine)
        {
            return new RssChannel
            {
                title = domaine.Nom,
                link = new Uri(this._baseUri, FileNameForDomaine(domaine)).ToString()
            };
        }

        private static bool BuilderContainsItem(DecisionBuilder builder, RssItem item)
        {
            return builder.Titre == item.title
                   && builder.Description == item.description
                   && builder.Lien == item.link;
        }

        private static DecisionBuilder FromItem(RssItem item)
        {
            string titre = item.title;
            titre = titre.Replace(RemovedIndicator, string.Empty);

            return new DecisionBuilder
            {
                Titre = titre,
                Description = item.description,
                Date = DateTime.Parse(item.pubDate),
                Lien = item.link,
                EstSupprimée = item.title != titre
            };
        }

        /// <inheritdoc />
        public IEnumerable<DecisionModel> FetchAll()
        {
            var decisionBuilders = new List<DecisionBuilder>();

            foreach (RssChannel channel in this.Channels)
            {
                var currentDomaine = new Domaine(channel.title);

                foreach (RssItem item in channel.Items())
                {
                    DecisionBuilder builder = decisionBuilders
                        .SingleOrDefault(element => BuilderContainsItem(element, item));

                    if (builder == null)
                    {
                        builder = FromItem(item);
                        decisionBuilders.Add(builder);
                    }

                    builder.Domaines.Add(currentDomaine);
                }
            }

            return decisionBuilders.Select(builder => builder.Build());
        }

        /// <inheritdoc />
        public bool Exists(DecisionModel decision)
        {
            return this.FetchAll().Any(decision.Equals);
        }
        
        private IEnumerable<Domaine> Domaines => this.Channels.Select(channel => new Domaine(channel.title));

        /// <summary>
        /// Constructeur
        /// </summary>
        public RSSPersister(IDirectory feedsStorageDirectory, Uri baseUri)
        {
            this._feedsStorageDirectory = feedsStorageDirectory;
            this._baseUri = baseUri;
        }

        private static string FileNameForDomaine(Domaine domaine)
        {
            return domaine.Nom.ToLowerInvariant() + ".xml";
        }

        private static RssChannel ParseFile(IFile file)
        {
            Rss rssElement;

            using (StreamReader reader = file.CreateReader())
            {
                var serializer = new XmlSerializer(typeof(Rss));
                rssElement = (Rss) serializer.Deserialize(reader);
            }

            return rssElement.channel.Single();
        }
        
        private IEnumerable<RssChannel> Channels
        {
            get
            {
                var files = this._feedsStorageDirectory.EnumerateFiles.Where(XmlFileFilter);
                return files.Select(ParseFile);
            }
        }

        /// <summary>
        /// CollectionChanged
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        IEnumerator<Domaine> IEnumerable<Domaine>.GetEnumerator() => this.Domaines.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.Domaines.GetEnumerator();
    }
}