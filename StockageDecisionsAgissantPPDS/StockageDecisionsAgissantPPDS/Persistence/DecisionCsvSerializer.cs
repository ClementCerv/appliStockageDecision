using System;
using System.Collections.Generic;
using StockageDecisionsAgissantPPDS.Model;
using StockageDecisionsAgissantPPDS.Model.Decision;

namespace StockageDecisionsAgissantPPDS.Persistence
{
    /// <summary>
    /// Décision sérialiser
    /// </summary>
    public class DecisionCsvSerializer
    {
        private readonly string _separator;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="separator"> Séparateur </param>
        public DecisionCsvSerializer(char separator)
        {
            this._separator = separator.ToString();
        }

        /// <summary>
        /// Sérialise
        /// </summary>
        /// <param name="decision"> Décision </param>
        public string Serialize(DecisionModel decision)
        {
            string titre = decision.Titre.Replace(this._separator, string.Empty);
            string date = decision.Date.ToString("d");
            string description = decision.Description.Replace(this._separator, string.Empty);
            string lien = decision.Lien.ToString();
            string estSupprimée = decision.EstSupprimée.ToString();
            
            string domaineNom = string.Empty;

            foreach (Domaine domaine in decision.Domaines)
                domaineNom = domaineNom + this._separator + domaine.Nom;
            domaineNom = domaineNom.TrimStart(this._separator[index: 0]);

            return titre + this._separator + date + this._separator + description + this._separator + lien + this._separator + estSupprimée + this._separator + domaineNom;
        }

        /// <summary>
        /// Désérialise
        /// </summary>
        /// <param name="serialized"> Sérialisé </param>
        public DecisionModel Deserialize(string serialized)
        {
            string value = serialized;
            var substrings = value.Split(new[] {this._separator }, StringSplitOptions.None);

            string titre = substrings[0];
            string date = substrings[1];
            string description = substrings[2];
            string lien = substrings[3];
            string estSupprimée = substrings[4];

            var domaineList = new List<Domaine>();

            if (string.IsNullOrEmpty(substrings[5]))
                return new DecisionModel(titre, description, DateTime.Parse(date),
                    lien == string.Empty ? null : new Uri(lien), domaineList) {EstSupprimée = bool.Parse(estSupprimée)};

            for (var i = 5; i < substrings.Length; i++)
            {
                string domaineNom = substrings[i];
                var domaine = new Domaine(domaineNom);
                domaineList.Add(domaine);
            }

            return new DecisionModel(titre, description, DateTime.Parse(date),
                lien == string.Empty ? null : new Uri(lien), domaineList) { EstSupprimée = bool.Parse(estSupprimée) };
        }
    }
}