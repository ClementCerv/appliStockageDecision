using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Normacode.DDD;

namespace StockageDecisionsAgissantPPDS.Model.Decision
{
    /// <summary>
    /// Modèle d'une décision
    /// </summary>
    [Serializable]
    public class DecisionModel : EntityObject<IDecisionModelIdentity>, IDecisionModelIdentity
    {
        /// <summary>
        /// Séparateur
        /// </summary>
        public const char Separator = '|';

        /// <inheritdoc />
        public string Titre { get; }

        /// <inheritdoc />
        public DateTime Date { get; }

        /// <inheritdoc />
        public string Description { get; }

        /// <inheritdoc />
        [NotNull]
        public Uri Lien { get; }

        /// <summary>
        /// Domaines à laquelle la décision est rattachée
        /// </summary>
        public ICollection<Domaine> Domaines { get; }

        /// <summary>
        /// EstSupprimée
        /// </summary>
        public bool EstSupprimée { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public DecisionModel(string titre, string description, DateTime date, Uri lien, IEnumerable<Domaine> domaines)
        {
            if (titre.Contains(Separator))
                throw new FormatException(nameof(titre) + " contains forbidden character : " + Separator);
            if (description.Contains(Separator))
                throw new FormatException(nameof(description) + " contains forbidden character : " + Separator);

            this.Titre = titre;
            this.Date = date.Date;
            this.Description = description;
            this.Lien = lien ?? throw new UriFormatException("Un lien ne peut pas être vide");
            this.Domaines = new HashSet<Domaine>(domaines);
        }
        
        /// <inheritdoc />
        public override string ToString()
        {
            return this.Titre + " du " + this.Date.ToString("d");
        }
    }
}