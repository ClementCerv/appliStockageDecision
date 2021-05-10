using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using JetBrains.Annotations;
using Normacode.IO;
using Normacode.Patterns;
using Normacode.RSS;
using StockageDecisionsAgissantPPDS.Model;

namespace StockageDecisionsAgissantPPDS.Persistence.DecisionPersister.RSS
{
    public class DomaineRssFeed : IProxy<RssChannel>
    {
        private const string XmlExtension = ".xml";
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(Rss));

        public readonly Domaine Domaine;
        private readonly IFile _file;

        public static IEnumerable<DomaineRssFeed> ReadDirectory(IDirectory directory)
        {
            var files = directory.EnumerateFiles("*" + XmlExtension);
            foreach (IFile file in files) yield return new DomaineRssFeed(file);
        }

        private DomaineRssFeed(IFile file)
        {
            this._file = file;
            this.Domaine = new Domaine(this.GetSomething(rssChannel => rssChannel.title));
        }

        public DomaineRssFeed(IDirectory directory, Domaine domaine, Uri baseUri)
        {
            this.Domaine = domaine;
            this._file = directory.CreateOrReturnFile(FileName(domaine));

            RssChannel channel = this.ReadFile() ?? new RssChannel();

            var fileUri = new Uri(baseUri, FileName(domaine));
            channel.link = fileUri.ToString();
            channel.title = this.Domaine.Nom;

            this.WriteFile(channel);
        }

        private static string FileName(Domaine domaine) => domaine.Nom.ToLowerInvariant() + XmlExtension;

        public void DoSomething(Action<RssChannel> actionToExecute)
        {
            RssChannel channel = this.ReadFile();
            actionToExecute.Invoke(channel);
            this.WriteFile(channel);
        }

        public TReturn GetSomething<TReturn>(Func<RssChannel, TReturn> functionToExecute)
        {
            RssChannel channel = this.ReadFile();
            TReturn result = functionToExecute.Invoke(channel);
            this.WriteFile(channel);

            return result;
        }

        [CanBeNull]
        private RssChannel ReadFile()
        {
            Rss rssElement;

            try
            {
                using (StreamReader reader = this._file.CreateReader())
                {
                    rssElement = (Rss) Serializer.Deserialize(reader);
                }
            }
            catch(InvalidOperationException)
            {
                return null;
            }
            
            RssChannel channel = rssElement.channel.SingleOrDefault();
            return channel;
        }

        private void WriteFile(RssChannel channel)
        {
            var rss = new Rss { channel = new[] { channel } };
            
            using (StreamWriter writer = this._file.CreateWriter())
                Serializer.Serialize(writer, rss);
        }
    }
}
