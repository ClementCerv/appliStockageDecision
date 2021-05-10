using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace StockageDecisionsAgissantPPDS.Model.Decision
{
    /// <summary>
    /// DecisionBuilder
    /// </summary>
    public class DecisionBuilder
    {
        [CanBeNull] private Uri _lien;
        private DateTime _date = DateTime.Today;

        /// <summary>
        /// Titre
        /// </summary>
        public string Titre { get; set; } = string.Empty;

        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date
        {
            get => this._date;
            set => this._date = value.Date;
        }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// Domaines
        /// </summary>
        public ICollection<Domaine> Domaines { get; } = new HashSet<Domaine>();

        /// <summary>
        /// Vérifie si c'est une uri valide en plus de stocker
        /// </summary>
        public string Lien
        {
            get => this._lien?.ToString();
            set => this._lien = new Uri(value);
        }

        /// <summary>
        /// EstSupprimée
        /// </summary>
        public bool EstSupprimée { get; set; }

        /// <summary>
        /// Build
        /// </summary>
        /// <returns></returns>
        public DecisionModel Build()
        {
            return new DecisionModel(this.Titre, this.Description, this.Date, this._lien, this.Domaines) { EstSupprimée = this.EstSupprimée};
        }
    }
}