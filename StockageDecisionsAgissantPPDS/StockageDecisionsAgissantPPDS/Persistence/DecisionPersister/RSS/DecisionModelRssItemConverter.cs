using System;
using System.Collections.Generic;
using Normacode.RSS;
using StockageDecisionsAgissantPPDS.Model;
using StockageDecisionsAgissantPPDS.Model.Decision;
using Guid = Normacode.RSS.Guid;

namespace StockageDecisionsAgissantPPDS.Persistence.DecisionPersister.RSS
{
    public class DecisionModelRssItemConverter
    {
        public const string RemovedIndicator = "[SUPPRIMÉ] ";

        private readonly DecisionBuilder _builder = new DecisionBuilder();

        public ICollection<Domaine> Domaines => this._builder.Domaines;

        private int ModelHashCode => this._builder.Build().GetHashCode();

        public DecisionModelRssItemConverter(DecisionModel model)
        {
            this._builder.Titre = model.Titre;
            this._builder.Description = model.Description;
            this._builder.Date = model.Date;
            this._builder.Lien = model.Lien.ToString();
            this._builder.EstSupprimée = model.EstSupprimée;

            foreach (Domaine domaine in this.Domaines)
            {
                this._builder.Domaines.Add(domaine);
            }
        }

        public DecisionModelRssItemConverter(RssItem item)
        {
            this._builder.Titre = item.title.Replace(RemovedIndicator, string.Empty);
            this._builder.Description = item.description;
            this._builder.Date = DateTime.Parse(item.pubDate).Date;
            this._builder.Lien = item.link;
            this._builder.EstSupprimée = !this._builder.Titre.Equals(item.title);

            // todo corriger cette merde.
            //if(this.ModelHashCode != int.Parse(item.guid.Value)) throw new InvalidOperationException("Le GUID ne correspond pas aux données");
        }

        public RssItem BuildItem()
        {
            var item = new RssItem
            {
                title = this._builder.EstSupprimée ? RemovedIndicator + this._builder.Titre : this._builder.Titre,
                description = this._builder.Description,
                link = this._builder.Lien,
                guid = new Guid { isPermaLink = false, Value = this.ModelHashCode.ToString() }
            };

            item.SetPubDate(this._builder.Date);

            return item;
        }

        public DecisionModel BuildModel() => this._builder.Build();

        private static bool DesignateSameDecision(IDecisionModelIdentity decision, RssItem item)
        {
            return DecisionModel.IdentityEqualityComparer.Equals(decision, new DecisionModelRssItemConverter(item).BuildModel());
        }

        public bool DesignatesSameDecision(RssItem item)
        {
            return DesignateSameDecision(this.BuildModel(), item);
        }
    }
}
