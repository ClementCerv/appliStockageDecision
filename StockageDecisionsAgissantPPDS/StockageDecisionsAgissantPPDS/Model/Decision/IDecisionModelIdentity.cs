using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Normacode.DDD;

namespace StockageDecisionsAgissantPPDS.Model.Decision
{
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface IDecisionModelIdentity : IIdentity
    {
        /// <summary>
        /// Titre de la décision
        /// </summary>
        string Titre { get; }

        /// <summary>
        /// Date de la création de la décision
        /// </summary>
        DateTime Date { get; }

        /// <summary>
        /// Description de la décision
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Lien de la décision
        /// </summary>
        [CanBeNull]
        Uri Lien { get; }
    }
}
