using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StockageDecisionsAgissantPPDS.Common
{
    /// <summary>
    /// Comparateur d'égalité selon les propriétés marquées <see cref="ClassIdentityAttribute"/>
    /// </summary>
    public class ClassIdentityEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Type _reflectedClassIdentityAttribute = typeof(ClassIdentityAttribute);
        private readonly Type _reflectedIdentity = typeof(T);

        private readonly IEnumerable<PropertyInfo> _classIdentityProperties;

        /// <summary>
        /// Constructeur
        /// </summary>
        public ClassIdentityEqualityComparer()
        {
            this._classIdentityProperties = this._reflectedIdentity.GetProperties().Where(prop => Attribute.IsDefined(prop, this._reflectedClassIdentityAttribute)).ToArray();
        }

        /// <summary>
        /// Renvoie une version curryfiée de Equals, a utiliser dans les objets immuables
        /// </summary>
        public Func<T, bool> CurryfiedEquals(T x)
        {
            var curryfiedValues = new Dictionary<string, object>();

            foreach (PropertyInfo classIdentityProperty in this._classIdentityProperties)
            {
                object valueX = classIdentityProperty.GetValue(x);
                curryfiedValues[classIdentityProperty.Name] = valueX;
            }

            return y =>
            {
                foreach (PropertyInfo classIdentityProperty in this._classIdentityProperties)
                {
                    object valueX = curryfiedValues[classIdentityProperty.Name];
                    object valueY = classIdentityProperty.GetValue(y);
                    if (!Equals(valueX, valueY)) return false;
                }

                return true;
            };
        }

        /// <summary>
        /// Vérifie l'égalité complète de toutes le propriétés marquées <see cref="ClassIdentityAttribute"/>
        /// </summary>
        public bool Equals(T x, T y)
        {
            foreach (PropertyInfo classIdentityProperty in this._classIdentityProperties)
            {
                object valueX = classIdentityProperty.GetValue(x);
                object valueY = classIdentityProperty.GetValue(y);
                if (!Equals(valueX, valueY)) return false;
            }

            return true;
        }

        /// <summary>
        /// Calcule le hashCode à partir des propriétés marquées <see cref="ClassIdentityAttribute"/> d'un objet
        /// </summary>
        public int GetHashCode(T obj)
        {
            var hashCode = new HashCode();

            foreach (PropertyInfo identityMember in this._classIdentityProperties)
            {
                object value = identityMember.GetValue(obj);
                hashCode.Hash(value);
            }

            return hashCode.Value;
        }
    }
}
