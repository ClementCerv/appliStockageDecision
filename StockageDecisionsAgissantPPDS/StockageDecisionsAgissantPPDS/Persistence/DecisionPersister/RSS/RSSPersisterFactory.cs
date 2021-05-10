using System;
using Normacode.IO.WebDAV;

namespace StockageDecisionsAgissantPPDS.Persistence.DecisionPersister.RSS
{
    /// <summary>
    /// Factory de RSSPersister
    /// </summary>
    public static class RSSPersisterFactory
    {
        /// <summary>
        /// Factory
        /// </summary>
        public static RSSPersister Factory(Uri rssPersisterUri)
        {
            var webDavRoot = new WebDAVDirectory(rssPersisterUri);
            return new RSSPersister(webDavRoot, rssPersisterUri);
        }
    }
}
