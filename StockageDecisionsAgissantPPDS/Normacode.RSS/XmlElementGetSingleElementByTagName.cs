using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Normacode.RSS
{
    /// <summary>
    /// Extensions de la classe <see cref="XmlElement"/>
    /// </summary>
    public static class XmlElementGetSingleElementByTagName
    {

        /// <summary>
        /// GetElementByTagName suivi d'un Single
        /// </summary>
        public static XmlElement GetSingleElementByTagName(this XmlElement parent, string tagName)
        {
            return parent.GetElementsByTagName(tagName)
                .OfType<XmlElement>().Single();
        }

        /// <summary>
        /// GetElementByTagName sur les enfants directs suivi d'un Single
        /// </summary>
        public static XmlElement GetSingleChildByTagName(this XmlElement parent, string tagName)
        {
            return parent.ChildNodes.OfType<XmlElement>().Single(element => element.Name == tagName);
        }

        /// <summary>
        /// GetElementByTagName sur les enfants directs
        /// </summary>
        public static IEnumerable<XmlElement> GetChildrenByTagName(this XmlElement parent, string tagName)
        {
            return parent.ChildNodes.OfType<XmlElement>().Where(element => element.Name == tagName);
        }
    }
}
