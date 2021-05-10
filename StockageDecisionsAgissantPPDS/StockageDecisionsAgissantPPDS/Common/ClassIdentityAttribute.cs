using System;

namespace StockageDecisionsAgissantPPDS.Common
{
    /// <summary>
    /// Indique que ce membre constitue l'identité de la classe
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ClassIdentityAttribute : ImmutableAttribute
    {
    }
}
