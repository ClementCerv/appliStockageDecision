using System;
using JetBrains.Annotations;
using Normacode.DDD;

namespace StockageDecisionsAgissantPPDS.Model
{
    /// <summary>
    /// Domaine
    /// </summary>
    public class Domaine : ValueObject<Domaine>
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        public Domaine(string nom)
        {
            if (string.IsNullOrWhiteSpace(nom))
                throw new FormatException(nameof(nom) + " is null or whitespace");
            this.Nom = nom;
        }

        /// <summary>
        /// Nom
        /// </summary>
        public string Nom { get; }

        /// <inheritdoc />
        [Pure]
        public override string ToString() => this.Nom;

        [Pure]
        public override object Clone()
        {
            return new Domaine(this.Nom);
        }
    }
}