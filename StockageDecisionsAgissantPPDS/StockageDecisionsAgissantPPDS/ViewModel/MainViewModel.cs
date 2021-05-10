using System;
using System.IO;
using System.Windows.Input;
using Normacode.Accounting;
using Normacode.Command;
using StockageDecisionsAgissantPPDS.Common;
using StockageDecisionsAgissantPPDS.Persistence;
using StockageDecisionsAgissantPPDS.Persistence.DecisionPersister;
using StockageDecisionsAgissantPPDS.Persistence.DecisionPersister.RSS;
using StockageDecisionsAgissantPPDS.Persistence.DomaineReference;
using StockageDecisionsAgissantPPDS.ViewModel.Decision;

namespace StockageDecisionsAgissantPPDS.ViewModel
{
    /// <summary>
    /// ViewModel principale
    /// </summary>
    public class MainViewModel
    {
        static MainViewModel()
        {
            if (!Directory.Exists(LogFolderPath)) Directory.CreateDirectory(LogFolderPath);
            Log = new Logger(new FileInfo(LogFilePath));
        }

        private static readonly string LogFolderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "StockageDecisionsAgissantPPDS");

        private static readonly string LogFilePath = Path.Combine(LogFolderPath,
            DateTime.Today.ToString("yyyy-MM-dd") + ".log");

        private static readonly Logger Log;

        private readonly DecisionPersisterProxy _storage;
        private readonly DomaineReferenceProxy _domaineReference;
        private readonly DomaineReferenceFactory _domaineReferenceFactory;
        private readonly DecisionPersisterFactory _decisionPersisterFactory;
        private readonly RSSPersisterLazyWarehouse _rssPersisterWarehouse;

        /// <summary>
        /// ListeViewModel
        /// </summary>
        public ListeViewModel ListeViewModel { get; }

        /// <summary>
        /// DecisionCreateViewModel
        /// </summary>
        public DecisionCreateViewModel DecisionCreateViewModel { get; }

        /// <summary>
        /// ParamètresViewModel
        /// </summary>
        public ParamètresViewModel ParamètresViewModel { get; }

        /// <summary>
        /// Commande exécuté au chargement
        /// </summary>
        public ICommand OnLoadCommand { get; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public MainViewModel()
        {
            this._storage = new DecisionPersisterProxy();
            this._domaineReference = new DomaineReferenceProxy();

            this.ParamètresViewModel = new ParamètresViewModel(new FileInfo(LogFolderPath));
            this.ParamètresViewModel.Changed += this.ParamètresViewModelChanged;

            this._rssPersisterWarehouse = new RSSPersisterLazyWarehouse();
            this._domaineReferenceFactory = new DomaineReferenceFactory(this.ParamètresViewModel, this._rssPersisterWarehouse);
            this._decisionPersisterFactory = new DecisionPersisterFactory(this.ParamètresViewModel, this._rssPersisterWarehouse);

            this.FactoryDecisionPersister();

            this.ListeViewModel = new ListeViewModel(this._storage, this._domaineReference);
            this.DecisionCreateViewModel = new DecisionCreateViewModel(this._storage, this._domaineReference);

            this.OnLoadCommand = new BasicCommand(OnLoad);
        }

        private void ParamètresViewModelChanged(object sender, EventArgs args)
        {
            this.FactoryDecisionPersister();
        }

        private void FactoryDecisionPersister()
        {
            try
            {
                this._rssPersisterWarehouse.ReplaceInstance(new Lazy<RSSPersister>(() => RSSPersisterFactory.Factory(this.ParamètresViewModel.PersisterRssUri)));

                this._storage.Proxied = new DecisionPersisterLogDecorator(this._decisionPersisterFactory.Factory(), Log);
                this._domaineReference.Proxied = this._domaineReferenceFactory.Factory();
            }
            catch(Exception e)
            {
                Log.Write("Erreur d'initialisation " + e.Message);
                Log.Write(e.StackTrace);

                this._storage.Proxied = new MemoryDecisionPersister();
                this._domaineReference.Proxied = new MemoryDomaineReference();
            }
        }

        private static void OnLoad()
        {
            Log.Write("Application demarrée par " + Account.CurrentAccount.UserName);
        }
    }
}