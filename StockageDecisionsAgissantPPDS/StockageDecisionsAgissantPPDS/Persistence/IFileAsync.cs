using System.IO;
using System.Threading.Tasks;

namespace StockageDecisionsAgissantPPDS.Persistence
{
    /// <summary>
    /// Interface générique d'un fichier
    /// </summary>
    public interface IFileAsync
    {
        /// <summary>
        /// Nom du fichier
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Lit le fichier
        /// </summary>
        Task<StreamReader> ReaderAsync { get; }

        /// <summary>
        /// Écrit dans le fichier
        /// </summary>
        Task<StreamWriter> WriterAsync { get; }
    }
}
