using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockageDecisionsAgissantPPDS.Persistence
{
    /// <summary>
    /// Représentation générique d'un répertoire
    /// </summary>
    public interface IDirectoryAsync
    {
        /// <summary>
        /// Les fichiers du dossier
        /// </summary>
        Task<IEnumerable<IFileAsync>> EnumerateFilesAsync { get; }

        /// <summary>
        /// Créé et/ou renvoie un fichier
        /// </summary>
        Task<IFileAsync> CreateOrReturnAsync(string name);
    }
}
