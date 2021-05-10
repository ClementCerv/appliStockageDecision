using System.Xml.Linq;
using JetBrains.Annotations;

namespace Normacode.RSS
{
    /// <summary>
    /// Représentation d'un flux RSS
    /// </summary>
    public class Feed
    {
        private readonly Channel _channel;

        /// <summary>
        /// Constructeur
        /// </summary>
        public Feed(Channel channel)
        {
            this._channel = channel;
        }

        /// <summary>
        /// Renvoie un élément XML représentant le flux
        /// </summary>
        /// <returns></returns>
        [PublicAPI]
        public XElement ToXml()
        {
            var rssElement = new XElement("rss");
            rssElement.SetAttributeValue("version", "2.0");
            rssElement.Add(this._channel.ToXml());
            return rssElement;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.ToXml().ToString();
        }
    }
}
