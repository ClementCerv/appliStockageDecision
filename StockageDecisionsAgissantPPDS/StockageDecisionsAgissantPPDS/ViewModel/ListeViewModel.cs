using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Normacode.Command;
using Normacode.IO.Adapters;
using Normacode.Patterns;
using StockageDecisionsAgissantPPDS.Model;
using StockageDecisionsAgissantPPDS.Model.Decision;
using StockageDecisionsAgissantPPDS.Persistence.DecisionPersister;
using StockageDecisionsAgissantPPDS.Persistence.DomaineReference;
using StockageDecisionsAgissantPPDS.ViewModel.Decision;

namespace StockageDecisionsAgissantPPDS.ViewModel
{
    /// <summary>
    /// ViewModel de liste
    /// </summary>
    public class ListeViewModel
    {
        private readonly IDecisionPersister _decisionSource;

        /// <summary>
        /// ViewModel de liste de décisions
        /// </summary>
        public DecisionsListViewModel DecisionsListeViewModel { get; }

        /// <summary>
        /// DomaineList
        /// </summary>
        public IObservableEnumerable<Domaine> DomaineList { get; }

        /// <summary>
        /// Domaine en cours
        /// </summary>
        public Domaine CurrentDomaine { get; set; }

        /// <summary>
        /// Commande qui signale que les domaines sélectionnés ont changés
        /// </summary>
        public ICommand SelectedDomaineChangedCommand { get; }

        /// <summary>
        /// Commande permettant d'exporter toutes les décisions de tous les domaines
        /// </summary>
        public ICommand ExportCommand { get; }

        /// <summary>
        /// Commande permettant d'actualiser les décisions d'un domaine
        /// </summary>
        public ICommand RefreshCommand { get; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public ListeViewModel(IDecisionPersister decisionsSource, IDomaineReference domaineReference)
        {
            this._decisionSource = decisionsSource;
            this.DomaineList = domaineReference;

            this.DecisionsListeViewModel = new DecisionsListViewModel(decisionsSource, domaineReference);
            this.SelectedDomaineChangedCommand = new BasicCommand(this.SelectedDomainChanged);
            this.ExportCommand = new BasicCommand(this.Export);
            this.RefreshCommand = new BasicCommand(this.Refresh);
        }

        private void SelectedDomainChanged()
        {
            this.DecisionsListeViewModel.CurrentDomaine.Value = this.CurrentDomaine;
        }

        private async void Export()
        {
            var saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = "C:\\",
                Filter = "csv files (*.csv)|*.csv",
                RestoreDirectory = true
            };

            var result = saveFileDialog.ShowDialog();
            if (result != true) return;

            var fileInfo = new FileInfo(saveFileDialog.FileName);
            var directory = new DirectoryInfoIDirectoryAdapter(new DirectoryInfo(fileInfo.DirectoryName ?? throw new InvalidOperationException()));
            var csvPersister = new CsvPersisterCreateFileDecorator(directory, fileInfo.Name, DecisionModel.Separator);
            
            
            
            try
            {
                foreach (DecisionModel decisionModel in await this._decisionSource.FetchAllAsync())
                    {
                        await csvPersister.StoreDecisionAsync(decisionModel);
                    }
                MessageBox.Show("L'export a bien été effectué.", "Confirmation");
            }

            catch (IOException)
            {
                MessageBox.Show("Impossible d'accéder au fichier, car il est en cours d'utilisation / déjà ouvert",
                    "Avertissement");
            }
        }

        private async void Refresh()
        {
            await this.DecisionsListeViewModel.RefreshCurrentDecisionList();
        }
    }
}