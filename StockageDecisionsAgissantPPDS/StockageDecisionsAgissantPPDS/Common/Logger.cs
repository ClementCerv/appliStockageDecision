using System;
using System.IO;
using System.Text;

namespace StockageDecisionsAgissantPPDS.Common
{
    /// <summary>
    /// Log
    /// </summary>
    public class Logger
    {
        private readonly FileInfo _logFile;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="logFile"> fichier log </param>
        public Logger(FileInfo logFile)
        {
            this._logFile = logFile;

            if (!logFile.Exists) logFile.Create().Close();
        }

        /// <summary>
        /// Ecrit le texte dans les logs
        /// </summary>
        /// <param name="text"></param>
        public void Write(string text)
        {
            DateTime dateTime = DateTime.Now;

            try
            {
                using (var sw = new StreamWriter(this._logFile.FullName, true, Encoding.UTF8))
                {
                    sw.Write(dateTime.ToString("u"));
                    sw.Write(value: ' ');
                    sw.WriteLine(text);
                }
            }
            catch (Exception)
            {
                Console.WriteLine(text + @" cannot be written in log file " + this._logFile.FullName);
            }
        }
    }
}