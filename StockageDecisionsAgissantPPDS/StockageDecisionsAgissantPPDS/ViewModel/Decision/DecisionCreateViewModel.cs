using System;
using System.Threading.Tasks;
using System.Windows;
using Normacode.InstanceProperty;
using StockageDecisionsAgissantPPDS.Model.Decision;
using StockageDecisionsAgissantPPDS.Persistence.DecisionPersister;
using StockageDecisionsAgissantPPDS.Persistence.DomaineReference;

namespace StockageDecisionsAgissantPPDS.ViewModel.Decision
{
    /// <summary>
    /// Créer la DecisionModel
    /// </summary>
    public class DecisionCreateViewModel : DecisionsViewModelAbstract
    {
        /// <summary>
        /// Builder des décisions en cours
        /// </summary>
        public InstanceProperty<DecisionBuilder> DecisionBuilder { get; }
        
        /// <summary>
        /// Constructeur
        /// </summary>
        public DecisionCreateViewModel(IDecisionPersister decisionsStorage, IDomaineReference domaineReference) 
            : base(decisionsStorage, domaineReference)
        {
            this.DecisionBuilder = new InstanceProperty<DecisionBuilder> { Value = new DecisionBuilder() };
            this.Register(nameof(this.DecisionBuilder), this.DecisionBuilder);
            this.Register(nameof(this.IsEnabled), this.IsEnabled);

            this.DomaineAdd += (sender, domaine) => this.DecisionBuilder.Value.Domaines.Add(domaine);
            this.DomaineRemoved += (sender, domaine) => this.DecisionBuilder.Value.Domaines.Remove(domaine);
        }
        
        /// <inheritdoc />
        protected override async Task Commit()
        {
            DecisionModel decision;

            try
            {
                decision = this.DecisionBuilder.Value.Build();
            }
            catch (UriFormatException uriEmpty)
            {
                MessageBox.Show(uriEmpty.Message, "Avertissement");
                return;
            }

            if (await this.DecisionsStorage.ExistsAsync(decision))
            {
                MessageBox.Show("La décision existe déjà.", "Avertissement");
            }
            else
            {
                try
                {
                    await this.DecisionsStorage.StoreDecisionAsync(decision);
                    this.Rollback();
                    MessageBox.Show("La décision a bien été créée.", "Confirmation");
                }
                catch (InvalidOperationException zeroDomaine)
                {
                    MessageBox.Show(zeroDomaine.Message, "Avertissement");
                }
            }
        }

        protected override string PreviewUri => this.DecisionBuilder.Value.Lien;

        public override string ComitMessage => "Création de la décision en cours ⌛ ";

        /// <inheritdoc />
        protected override void Rollback()
        {
            this.DecisionBuilder.Value = new DecisionBuilder();
            this.IncludedDomaineList.Clear();
        }
    }
}
