using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using StockageDecisionsAgissantPPDS.Model;
using StockageDecisionsAgissantPPDS.Model.Decision;
using StockageDecisionsAgissantPPDS.Persistence.DecisionPersister;
using StockageDecisionsAgissantPPDS.Persistence.DomaineReference;

namespace StockageDecisionsAgissantPPDS.ViewModel.Decision
{
    /// <summary>
    /// Editer la DecisionModel
    /// </summary>
    public class DecisionEditViewModel : DecisionsViewModelAbstract
    {
        private readonly Domaine[] _initialState;

        /// <summary>
        /// La décision en cours d'édition
        /// </summary>
        public DecisionModel EditedDecision { get; }

        /// <summary>
        /// Émis lors de la validation d'une modification
        /// </summary>
        public event EventHandler Committed;

        /// <summary>
        /// Constructeur
        /// </summary>
        public DecisionEditViewModel(IDecisionPersister decisionsStorage, IDomaineReference domaineReference, DecisionModel toEdit) 
            : base(decisionsStorage, domaineReference)
        {
            this.EditedDecision = toEdit;
            this.PreviewUri = toEdit.Lien.AbsoluteUri;

            this._initialState = toEdit.Domaines.ToArray();
            this.AjouterDomaines(this._initialState);

            this.DomaineAdd += (sender, domaine) => toEdit.Domaines.Add(domaine);
            this.DomaineRemoved += (sender, domaine) => toEdit.Domaines.Remove(domaine);

            this.Register(nameof(this.IsEnabled), this.IsEnabled);
        }

        public override string ComitMessage => "Modification de la décision en cours ⌛ ";

        /// <inheritdoc />
        protected override void Rollback()
        {
            this.RetirerDomaines(this.EditedDecision.Domaines);
            this.AjouterDomaines(this._initialState);
        }

        /// <inheritdoc />
        protected override async Task Commit()
        {
            if (!this.EditedDecision.Domaines.Any())
            {
                MessageBox.Show("Il faut au moins un domaine pour créer une décision.", "Erreur");
            }
            else
            {
                await this.DecisionsStorage.StoreDecisionAsync(this.EditedDecision);
                MessageBox.Show("La décision a bien été mise à jour.", "Confirmation");
                this.Committed?.Invoke(this, EventArgs.Empty);
            }
        }

        protected override string PreviewUri { get; }
    }
}
