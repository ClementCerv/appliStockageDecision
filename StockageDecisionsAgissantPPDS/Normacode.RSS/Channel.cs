using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Normacode.RSS
{
    /// <summary>
    /// Modèle d'un channel
    /// </summary>
    public class Channel
    {
        private readonly List<Item> _items;

        /// <summary>
        /// Constructeur qui récupère les valeurs depuis un xml
        /// </summary>
        /// <param name="channelElement"> Element du channel </param>
        public Channel(XmlElement channelElement)
        {
            XmlElement titleElement = channelElement.GetSingleChildByTagName("title");
            this.Titre = titleElement.InnerText;

            XmlElement linkElement = channelElement.GetSingleChildByTagName("link");
            this.Lien = new Uri(linkElement.InnerText);

            XmlElement descriptionElement = channelElement.GetSingleChildByTagName("description");
            this.Description = descriptionElement.InnerText;
            
            var itemElements = channelElement.GetChildrenByTagName("item");
            this._items = itemElements.Select(element => new Item(element)).ToList();
        }

        /// <summary>
        /// Constructeur qui construit à partir de valeur données
        /// </summary>
        public Channel(string titre,Uri lien)
        {
            this.Titre = titre;
            this.Lien = lien;
            this.Description = string.Empty;
            this._items = new List<Item>();
        }

        /// <summary>
        /// Titre du Channel
        /// </summary>
        [PublicAPI]
        public string Titre { get; }

        /// <summary>
        /// Lien du Channel
        /// </summary>
        [PublicAPI]
        public Uri Lien { get; }
        /// <summary>
        /// Description du channel
        /// </summary>
        [PublicAPI]
        public string Description { get; }

        /// <summary>
        /// Items du channel
        /// </summary>
        [PublicAPI]
        public ICollection<Item> Items => this._items;

        /// <summary>
        /// Renvoie la représentation XML de l'Item
        /// </summary>
        /// <returns></returns>
        public XElement ToXml()
        {
            var channelElement = new XElement(nameof(Channel).ToLower());

            var titreElement = new XElement("title", this.Titre);
            channelElement.Add(titreElement);

            var lienElement = new XElement("link", this.Lien);
            channelElement.Add(lienElement);

            var descriptionElement = new XElement("description", this.Description);
            channelElement.Add(descriptionElement);

            foreach (Item itemElement in this.Items)
            {
                channelElement.Add(itemElement.ToXml());
            }

            return channelElement;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.ToXml().ToString();
        }
    }
}
