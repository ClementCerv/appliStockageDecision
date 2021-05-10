using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Normacode.IO;
using StockageDecisionsAgissantPPDS.Model.Decision;

namespace StockageDecisionsAgissantPPDS.Persistence.DecisionPersister
{
    /// <summary>
    /// Stocke les décisions au format csv
    /// </summary>
    public class CsvPersister : IDecisionPersister, IDisposable
    {
        private readonly IFile _csvFile;
        private readonly ReaderWriterLockSlim _csvFileAccessLock = new ReaderWriterLockSlim();
        private readonly DecisionCsvSerializer _serialiser;
        private readonly string _stringRepresentation;

        /// <summary>
        /// Constructeur
        /// </summary>
        public CsvPersister(IFile file, char separator)
        {
            this._stringRepresentation = nameof(CsvPersister) + " at " + file;
            this._csvFile = file;
            this._serialiser = new DecisionCsvSerializer(separator);
        }
        
        private void StoreDecision(DecisionModel decision)
        {
            var previousState = new HashSet<DecisionModel>(this.FetchAll());
            previousState.Remove(decision);
            previousState.Add(decision);

            this._csvFileAccessLock.EnterWriteLock();
            try
            {
                using (StreamWriter sw = this._csvFile.CreateWriter())
                {
                    foreach (DecisionModel decisionModel in previousState)
                    {
                        sw.WriteLine(this._serialiser.Serialize(decisionModel));
                    }
                }
            }
            finally
            {
                this._csvFileAccessLock.ExitWriteLock();
            }
        }
        
        private IEnumerable<DecisionModel> FetchAll()
        {
            this._csvFileAccessLock.EnterReadLock();

            StreamReader sr = null;

            try
            {

                try
                {
                    sr = this._csvFile.CreateReader();
                    if(sr == null) yield break;
                }
                catch (IOException)
                {
                    MessageBox.Show("Impossible d'accéder au fichier, car il est en cours d'utilisation / déjà ouvert",
                        "Avertissement");
                    yield break;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Erreur");
                    yield break;
                }

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    yield return this._serialiser.Deserialize(line);
                }
            }
            finally
            {
                this._csvFileAccessLock.ExitReadLock();
                sr?.Dispose();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this._csvFileAccessLock?.Dispose();
        }

        public async Task StoreDecisionAsync(DecisionModel decision)
        {
            await Task.Run(() => this.StoreDecision(decision));
        }

        public async  Task<IEnumerable<DecisionModel>> FetchAllAsync()
        {
            return await Task.Run(() => this.FetchAll());
        }

        public async Task<bool> ExistsAsync(IDecisionModelIdentity decision)
        {
            return (await this.FetchAllAsync()).Any(decision.Equals);
        }

        public override string ToString()
        {
            return this._stringRepresentation;
        }
    }
}