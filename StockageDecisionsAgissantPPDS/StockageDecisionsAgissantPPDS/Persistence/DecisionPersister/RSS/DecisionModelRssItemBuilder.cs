using System;
using System.Collections.Generic;
using System.Linq;
using Normacode.RSS;
using StockageDecisionsAgissantPPDS.Model;
using StockageDecisionsAgissantPPDS.Model.Decision;

namespace StockageDecisionsAgissantPPDS.Persistence.DecisionPersister.RSS
{
    public class DecisionModelRssItemBuilder
    {
        public const string RemovedIndicator = "[SUPPRIMÉ] ";

        private string Titre { get; }
        
        private string Description { get; }

        private DateTime Date { get; }

        private Uri Lien { get; }

        private bool EstSupprimée { get; }

        public ICollection<Domaine> Domaines { get; }

        public DecisionModelRssItemBuilder(RssItem item)
        {
            this.Titre = item.title.Replace(RemovedIndicator, string.Empty);
            this.Description = item.description;
            this.Date = DateTime.Parse(item.pubDate).Date;
            this.Lien = new Uri(item.link);
            this.EstSupprimée = !this.Titre.Equals(item.title);
            this.Domaines = new HashSet<Domaine>();
        }
        
        public DecisionModel Build()
        {
            return new DecisionModel(Titre, Description, Date, Lien, Domaines?.ToArray() ?? new Domaine[0])
            {
                EstSupprimée = this.EstSupprimée
            };
        }

        public static bool DesignateSameDecision(DecisionModel decision, RssItem item)
        {
            return DecisionModel.IdentityEqualityComparer.Equals(decision, new DecisionModelRssItemBuilder(item).Build());
        }

        public bool DesignatesSameDecision(RssItem item)
        {
            return DesignateSameDecision(Build(), item);
        }
    }
}
