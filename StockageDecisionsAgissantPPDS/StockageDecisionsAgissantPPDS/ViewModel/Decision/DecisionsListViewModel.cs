using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Normacode.Command;
using Normacode.InstanceProperty;
using StockageDecisionsAgissantPPDS.Model;
using StockageDecisionsAgissantPPDS.Model.Decision;
using StockageDecisionsAgissantPPDS.Persistence.DecisionPersister;
using StockageDecisionsAgissantPPDS.Persistence.DomaineReference;
using StockageDecisionsAgissantPPDS.View;

namespace StockageDecisionsAgissantPPDS.ViewModel.Decision
{
    /// <summary>
    /// ViewModel de list de décisions
    /// </summary>
    public class DecisionsListViewModel : InstancePropertyHolder
    {
        private readonly IDecisionPersister _storage;
        private readonly IDomaineReference _domaineReference;

        /// <summary>
        /// Commande permettant d'éditer la décision
        /// </summary>
        public ICommand EditDecisionCommand { get; }

        /// <summary>
        /// Commande permettant de "supprimer" la décision
        /// </summary>
        public ICommand ToggleDeleteDecisionCommand { get; }

        /// <summary>
        /// Domaine en cours
        /// </summary>
        public InstanceProperty<Domaine> CurrentDomaine { get; }

        /// <summary>
        /// Indique qu'un travail est en cours
        /// </summary>
        public InstanceProperty<bool> WorkInProgress { get; }

        /// <summary>
        /// Liste de décision a afficher
        /// </summary>
        public ObservableCollection<DecisionViewModel> DecisionsList { get; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public DecisionsListViewModel(IDecisionPersister storage, IDomaineReference domaineReference)
        {
            this._storage = storage;
            this._domaineReference = domaineReference;

            this.WorkInProgress = new InstanceProperty<bool>();

            this.CurrentDomaine = new InstanceProperty<Domaine>();
            this.CurrentDomaine.ValueChanged += async (sender, value) => await this.RefreshDecisionList(value);

            this.DecisionsList = new ObservableCollection<DecisionViewModel>();
            this.ToggleDeleteDecisionCommand = new BasicCommand<DecisionViewModel>(async model => await this.ToggleDelete(model));
            this.EditDecisionCommand = new BasicCommand<DecisionViewModel>(this.EditDecision);

            this.Register(nameof(this.CurrentDomaine), this.CurrentDomaine);
        }

        public async Task RefreshCurrentDecisionList()
        {
            await this.RefreshDecisionList(this.CurrentDomaine.Value);
        }

        private async Task RefreshDecisionList(Domaine currentDomaine)
        {
            try
            {
                this.WorkInProgress.Value = true;
                this.DecisionsList.Clear();

                var decisionModels = await this._storage.FetchAllAsync();
                foreach (DecisionModel decision in decisionModels.Where(decision =>
                    decision.Domaines.Contains(currentDomaine)))
                {
                    var decisionViewModel = new DecisionViewModel(decision);
                    this.DecisionsList.Add(decisionViewModel);
                }
            }
            finally
            {
                this.WorkInProgress.Value = false;
            }
        }

        /// <summary>
        /// Crée une fenêtre pour l'édition
        /// </summary>
        private void EditDecision(DecisionViewModel decisionViewModel)
        {
            var viewModel = new DecisionEditViewModel(this._storage, this._domaineReference, decisionViewModel.DecisionModel);
            var newWindow = new DecisionEditWindow { DataContext = viewModel };

            viewModel.Committed += (sender, args) => newWindow.Close();
            newWindow.Show();
        }

        /// <summary>
        /// Permet de basculer une décision de "non supprimée" à "supprimée" et inversement
        /// </summary>
        private async Task ToggleDelete(DecisionViewModel decisionViewModel)
        {
            decisionViewModel.EstSupprimée.Value = !decisionViewModel.EstSupprimée.Value;

            await this._storage.StoreDecisionAsync(decisionViewModel.DecisionModel);

            MessageBox.Show(
                decisionViewModel.EstSupprimée.Value
                    ? "La décision a bien été supprimée"
                    : "La décision a bien été restaurée", "Confirmation");
        }
    }
}