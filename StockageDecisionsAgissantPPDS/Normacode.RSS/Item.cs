using System;
using System.Xml;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Normacode.RSS
{
    /// <summary>
    /// Modèle d'un Item
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Constructeur qui récupère les valeurs depuis un xml
        /// </summary>
        /// <param name="itemElement"> Element de l'Item </param>
        public Item(XmlElement itemElement)
        {
            XmlElement titleElement = itemElement.GetSingleChildByTagName("title");
            this.Titre = titleElement.InnerText;

            XmlElement linkElement = itemElement.GetSingleChildByTagName("link");
            this.Lien = new Uri(linkElement.InnerText);

            XmlElement descriptionElement = itemElement.GetSingleChildByTagName("description");
            this.Description = descriptionElement.InnerText;

            XmlElement pubDateElement = itemElement.GetSingleChildByTagName("pubDate");
            this.PubDate = DateTime.Parse(pubDateElement.InnerText);
        }

        /// <summary>
        /// Constructeur qui construit à partir de valeur données
        /// </summary>
        public Item(string titre, DateTime pubDate, string description, Uri lien)
        {
            this.Titre = titre;
            this.PubDate = pubDate;
            this.Description = description;
            this.Lien = lien;
        }

        /// <summary>
        /// Titre
        /// </summary>
        [PublicAPI]
        public string Titre { get; }

        /// <summary>
        /// Date
        /// </summary>
        [PublicAPI]
        public DateTime PubDate { get; }

        /// <summary>
        /// Description
        /// </summary>
        [PublicAPI]
        public string Description { get; }

        /// <summary>
        /// Lien
        /// </summary>
        [PublicAPI]
        public Uri Lien { get; }

        /// <summary>
        /// Guid
        /// </summary>
        [PublicAPI]
        public Guid Guid { get; set; }

        /// <summary>
        /// Renvoie la représentation XML de l'Item
        /// </summary>
        /// <returns></returns>
        public XElement ToXml()
        {
            var itemElement = new XElement(nameof(Item).ToLower());

            var titreElement = new XElement("title", this.Titre);
            itemElement.Add(titreElement);

            var lienElement = new XElement("link", this.Lien);
            itemElement.Add(lienElement);

            var descriptionElement = new XElement("description", this.Description);
            itemElement.Add(descriptionElement);

            var pubDateElement = new XElement("pubDate", this.PubDate);
            itemElement.Add(pubDateElement);

            if (this.Guid != Guid.Empty)
            {
                var guidElement = new XElement("guid", this.Guid);
                itemElement.Add(guidElement);
            }

            return itemElement;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.ToXml().ToString();
        }
    }
}