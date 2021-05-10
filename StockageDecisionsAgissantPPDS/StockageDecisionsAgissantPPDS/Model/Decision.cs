using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace StockageDecisionsAgissantPPDS.Model
{
    [Serializable]
    public class Decision : IEquatable<Decision>
    {
        public const string Separator = "|";

        public string Titre { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public IEnumerable<Domaine> Domaines { get; set; }

        [CanBeNull]
        public Uri Lien { get; set; }

        public override string ToString()
        {
            return Serialize();
        }

        public Decision(string titre, string description, DateTime date, Uri lien)
        {
            if (titre.Contains(Separator))
                throw new FormatException(nameof(titre) + " contains forbidden character : " + Separator);
            if (description.Contains(Separator))
                throw new FormatException(nameof(description) + " contains forbidden character : " + Separator);
            Titre = titre;
            Description = description;
            Date = date;
            Lien = lien;
            Domaines = new List<Domaine>();
        }

        public Decision()
        {
            Titre = string.Empty;
            Description = string.Empty;
            Date = DateTime.Now;
            Lien = null;
            Domaines = new List<Domaine>();
        }

        public string Serialize()
        {
            string titre = Titre.Replace(Separator, string.Empty);
            string date = Date.ToString("d");
            string description = Description.Replace(Separator, string.Empty);
            string lien = string.Empty;

            if (Lien != null)
            {
                lien = Lien.ToString();
            }

            string domaineNom = string.Empty;

            foreach (Domaine domaine in Domaines)
            {
                domaineNom = domaineNom + Separator + domaine.Nom;
            }
            domaineNom = domaineNom.TrimStart(Separator[0]);

            return titre + Separator + date + Separator + description + Separator + lien + Separator + domaineNom;
        }

        public static Decision Deserialize(string serialized)
        {
            string value = serialized;
            string[] substrings = value.Split(new[] {Separator}, StringSplitOptions.None);

            var titre = substrings[0];
            var date = substrings[1];
            var description = substrings[2];
            var lien = substrings[3];

            var domaineList = new List<Domaine>();

            if (!string.IsNullOrEmpty(substrings[4]))
                for (int i = 4; i < substrings.Length; i++)
                {
                    var domaineNom = substrings[i];
                    var domaine = new Domaine(domaineNom);
                    domaineList.Add(domaine);
                }

            return new Decision(titre, description, DateTime.Parse(date),
                lien == string.Empty ? null : new Uri(lien))
            {
                Domaines = domaineList
            };
        }

        #region Equality

        public bool Equals(Decision other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Titre, other.Titre) && Date.ToString("d").Equals(other.Date.ToString("d")) &&
                   string.Equals(Description, other.Description) && Equals(Lien, other.Lien);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Decision) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Titre != null ? Titre.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Date.GetHashCode();
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Lien != null ? Lien.GetHashCode() : 0);
                return hashCode;
            }
        }

        #endregion
    }
}