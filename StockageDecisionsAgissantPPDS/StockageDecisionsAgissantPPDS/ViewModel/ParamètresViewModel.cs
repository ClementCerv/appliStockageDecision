using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Windows.Input;
using JetBrains.Annotations;
using Normacode.Command;
using Normacode.InstanceProperty;
using StockageDecisionsAgissantPPDS.Configuration;
using StockageDecisionsAgissantPPDS.Model;
using StockageDecisionsAgissantPPDS.Properties;

namespace StockageDecisionsAgissantPPDS.ViewModel
{
    /// <summary>
    /// ViewModel de paramètres
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
    public class ParamètresViewModel : InstancePropertyHolder, IDomaineReferenceConfiguration, IDecisionPersisterConfiguration
    {
        private readonly FileInfo _logFile;

        /// <summary>
        /// Is RSS persister used
        /// </summary>
        public CalculatedInstanceProperty<bool> IsRSSUsed { get; }

        /// <summary>
        /// Is Csv Persister used
        /// </summary>
        public CalculatedInstanceProperty<bool> IsCsvUsed { get; }

        public ICommand LogFileCommand { get; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public ParamètresViewModel(FileInfo logFile)
        {
            this._logFile = logFile;
            Settings.Default.PropertyChanged += (sender, args) =>
            {
                this.IsRSSUsed.Refresh();
                this.IsCsvUsed.Refresh();
                this.Changed?.Invoke(this, EventArgs.Empty);
            };

            this.IsCsvUsed = new CalculatedInstanceProperty<bool>(() => this.PersisterType == "Csv");
            this.IsRSSUsed = new CalculatedInstanceProperty<bool>(() =>
                this.PersisterType == "RSS" || this.PersisterDomainesSource == "RSS");

            this.Register(nameof(this.IsRSSUsed), this.IsRSSUsed);
            this.Register(nameof(this.IsCsvUsed), this.IsCsvUsed);

            this.LogFileCommand = new BasicCommand(this.LogFile);
        }

        /// <inheritdoc />
        public string PersisterType
        {
            get { return Settings.Default.Persister_Type; }
            set
            {
                Settings.Default.Persister_Type = value;
                Settings.Default.Save();
            }
        }


        /// <inheritdoc />
        public string PersisterCsvPath
        {
            get { return Settings.Default.Persister_Csv_Path; }
            set
            {
                Settings.Default.Persister_Csv_Path = value;
                Settings.Default.Save();
            }
        }

        /// <summary>
        /// Chemin du lien RSS
        /// </summary>
        [CanBeNull]
        public Uri PersisterRssUri
        {
            get
            {
                try
                {
                    return new Uri(Settings.Default.Persister_RSS_Uri);
                }
                catch(UriFormatException)
                {
                    return new Uri("http://localhost/");
                }
            }
            set
            {
                Settings.Default.Persister_RSS_Uri = value?.AbsoluteUri ?? string.Empty;
                Settings.Default.Save();
            }
        }

        /// <inheritdoc />
        public string PersisterDomainesSource
        {
            get { return Settings.Default.Persister_Domaines_Source; }
            set
            {
                Settings.Default.Persister_Domaines_Source = value;
                Settings.Default.Save();
            }
        }

        /// <inheritdoc />
        public char CsvSeparator
        {
            get { return Settings.Default.Separator; }
            set
            {
                Settings.Default.Separator = value;
                Settings.Default.Save();
            }
        }

        /// <inheritdoc />
        public IEnumerable<Domaine> PersisterDomainesList => Settings.Default.Persister_Domaines_List.Cast<string>()
            .Select(domainName => new Domaine(domainName));

        /// <summary>
        /// Valeur du type de persister
        /// </summary>
        public IEnumerable<string> PersisterTypeValues => Settings.Default.Persister_Type_Values.Cast<string>();

        /// <summary>
        /// Valeur du type de persister
        /// </summary>
        public IEnumerable<string> PersisterDomainesSourceValues =>
            Settings.Default.Persister_Domaines_Source_Values.Cast<string>();

        /// <summary>
        /// 
        /// </summary>
        public bool SettingsEnabled => Settings.Default.SettingsEnabled;

        /// <summary>
        /// Lancé quand un paramètre change
        /// </summary>
        public event EventHandler Changed;

        private void LogFile()
        {
            Process.Start(this._logFile.FullName);
        }
    }
}