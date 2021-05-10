using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using StockageDecisionsAgissantPPDS.Model;
using StockageDecisionsAgissantPPDS.Model.Decision;
using StockageDecisionsAgissantPPDS.Persistence.DecisionPersister;
using StockageDecisionsAgissantPPDS.Persistence.DomaineReference;
using StockageDecisionsAgissantPPDS.Properties;

namespace StockageDecisionsAgissantPPDS.Design
{
    internal class DesignDecisionPersister : IDecisionPersister, IDomaineReference
    {
        private readonly List<DecisionModel> _models = new List<DecisionModel>
        {
            new DecisionModel("Test A", "Description A", DateTime.Today, new Uri("localhost"),
                new [] {new Domaine(Settings.Default.Persister_Domaines_List[0])}),
            new DecisionModel("Test B", "Description B", DateTime.Today, new Uri("localhost"),
                new [] {new Domaine(Settings.Default.Persister_Domaines_List[2])})
        };

        public void StoreDecision(DecisionModel decision)
        {
            this._models.Add(decision);
        }

        public IEnumerable<DecisionModel> FetchAll()
        {
            return this._models;
        }

        public bool Exists(IDecisionModelIdentity decision)
        {
            return this._models.Contains(decision);
        }

        public Task StoreDecisionAsync(DecisionModel decision)
        {
            return Task.Run(() => this.StoreDecision(decision));
        }

        public Task<IEnumerable<DecisionModel>> FetchAllAsync()
        {
            return Task.Run(() => this.FetchAll());
        }

        public Task<bool> ExistsAsync(IDecisionModelIdentity decision)
        {
            return Task.Run(() => this.Exists(decision));
        }

        private static IEnumerable<Domaine> Domaines => Settings.Default.Persister_Domaines_List.Cast<string>().Select(str => new Domaine(str));

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        IEnumerator<Domaine> IEnumerable<Domaine>.GetEnumerator() => Domaines.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Domaines.GetEnumerator();
    }
}
