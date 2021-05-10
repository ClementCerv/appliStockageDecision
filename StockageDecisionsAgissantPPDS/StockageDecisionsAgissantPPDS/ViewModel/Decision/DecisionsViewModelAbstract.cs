using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Normacode.Command;
using Normacode.InstanceProperty;
using StockageDecisionsAgissantPPDS.Model;
using StockageDecisionsAgissantPPDS.Persistence.DecisionPersister;
using StockageDecisionsAgissantPPDS.Persistence.DomaineReference;

namespace StockageDecisionsAgissantPPDS.ViewModel.Decision
{
    /// <summary>
    /// ViewModel de décisions
    /// </summary>
    public abstract class DecisionsViewModelAbstract : InstancePropertyHolder
    {
        /// <summary>
        /// Stockage de décision
        /// </summary>
        protected readonly IDecisionPersister DecisionsStorage;

        private readonly IDomaineReference _domaineReference;

        /// <summary>
        /// Domaines exclus du modèle
        /// </summary>
        public ObservableCollection<Domaine> ExcludedDomaineList { get; }

        /// <summary>
        /// Domaines inclus dans le modèle
        /// </summary>
        public ObservableCollection<Domaine> IncludedDomaineList { get; }

        /// <summary>
        /// Ajouter les domaines sélectionnés
        /// </summary>
        public IEnumerable<Domaine> AddSelectedDomaineList { get; set; }

        /// <summary>
        /// Enlever les domaines sélectionnés
        /// </summary>
        public IEnumerable<Domaine> RemoveSelectedDomaineList { get; set; }

        /// <summary>
        /// Commande permettant d'ajouter les domaines selectionné
        /// </summary>
        public ICommand AjouterSelectedDomainesCommand { get; }

        /// <summary>
        /// Commande permettant d'enlever les domaines selectionné
        /// </summary>
        public ICommand EnleverSelectedDomainesCommand { get; }

        /// <summary>
        /// Commande permettant de valider la décision
        /// </summary>
        public ICommand CommitCommand { get; }

        /// <summary>
        /// Réinitialisation des commandes
        /// </summary>
        public ICommand RollbackCommand { get; }

        /// <summary>
        ///  
        /// </summary>
        public ICommand ClickCommand { get; }

        /// <summary>
        /// Indique si la vue est activée
        /// </summary>
        public InstanceProperty<bool> IsEnabled { get; }

        /// <summary>
        /// Indique qu'un travail est en cours
        /// </summary>
        public InstanceProperty<bool> WorkInProgress { get; }

        public abstract string ComitMessage { get; }

        /// <summary>
        /// Constructeur
        /// </summary>
        protected DecisionsViewModelAbstract(
            IDecisionPersister decisionsStorage, 
            IDomaineReference domaineReference)
        {
            this.DecisionsStorage = decisionsStorage;
            
            this.IsEnabled = new InstanceProperty<bool> { Value = true };

            this.AddSelectedDomaineList = new List<Domaine>();
            this.RemoveSelectedDomaineList = new List<Domaine>();

            this.IncludedDomaineList = new ObservableCollection<Domaine>();
            this.ExcludedDomaineList = new ObservableCollection<Domaine>();

            this._domaineReference = domaineReference;
            this._domaineReference.CollectionChanged += (value, args) => this.ResetDomaineList();

            this.AjouterSelectedDomainesCommand = new BasicCommand(this.AjouterSelectedDomaines);
            this.EnleverSelectedDomainesCommand = new BasicCommand(this.EnleverSelectedDomaines);
            
            this.CommitCommand = new BasicCommand(async () =>
            {
                bool oldValueOfIsEnabled = this.IsEnabled.Value;

                try
                {
                    this.WorkInProgress.Value = true;
                    this.IsEnabled.Value = false;
                    await this.Commit();
                }
                finally
                {
                    this.WorkInProgress.Value = false;
                    this.IsEnabled.Value = oldValueOfIsEnabled;
                }
            });

            this.RollbackCommand = new BasicCommand(this.Rollback);

            this.ResetDomaineList();
            this.ClickCommand = new BasicCommand(this.Clicked);

            this.WorkInProgress = new InstanceProperty<bool>();
        }

        private void ResetDomaineList()
        {
            this.IncludedDomaineList.Clear();
            this.ExcludedDomaineList.Clear();

            try
            {
                foreach (Domaine domaine in this._domaineReference)
                    this.ExcludedDomaineList.Add(domaine);

                this.IsEnabled.Value = true;
            }
            catch
            {
                this.IsEnabled.Value = false;
            }
        }

        /// <summary>
        /// Remet la vue dans son état initial
        /// </summary>
        protected abstract void Rollback();

        /// <summary>
        /// Valide la décision avant de la stocker
        /// </summary>
        protected abstract Task Commit();

        /// <summary>
        /// Remet la vue dans son état initial
        /// </summary>
        protected event EventHandler<Domaine> DomaineAdd;

        /// <summary>
        /// Valide la décision avant de la stocker
        /// </summary>
        protected event EventHandler<Domaine> DomaineRemoved;

        /// <summary>
        /// Ajoute une liste de domaines à la liste
        /// </summary>
        protected void AjouterDomaines(IEnumerable<Domaine> domaines)
        {
            var hashTable = new HashSet<Domaine>(this.IncludedDomaineList);

            foreach (Domaine addSelectedDomaine in domaines)
                hashTable.Add(addSelectedDomaine);

            this.IncludedDomaineList.Clear();

            foreach (Domaine domaine in hashTable)
            {
                this.IncludedDomaineList.Add(domaine);
                this.DomaineAdd?.Invoke(this, domaine);
            }
        }

        /// <summary>
        /// Retire les domaines
        /// </summary>
        /// <param name="domaines"></param>
        protected void RetirerDomaines(IEnumerable<Domaine> domaines)
        {
            var bufferCollection = new HashSet<Domaine>(domaines);

            foreach (Domaine domaine in bufferCollection)
            {
                this.IncludedDomaineList.Remove(domaine);
                this.DomaineRemoved?.Invoke(this, domaine);
            }
        }

        private void AjouterSelectedDomaines()
        {
            this.AjouterDomaines(this.AddSelectedDomaineList);
        }
        
        private void EnleverSelectedDomaines()
        {
            this.RetirerDomaines(this.RemoveSelectedDomaineList);
        }

        protected abstract string PreviewUri { get; }

        private void Clicked()
        {
            var process = new Process { StartInfo = { FileName = this.PreviewUri } };
            process.Start();
        }
    }
}