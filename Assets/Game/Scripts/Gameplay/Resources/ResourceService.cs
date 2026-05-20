using System;
using System.Collections.Generic;

namespace Game.Scripts.Gameplay.PlayerResources
{
    public class ResourceService : IResourceService
    {
        private readonly ResourceStorage _storage;
        
        public event Action<ResourceType, int> ResourceChanged;
        public event Action ResourcesChanged;

        public ResourceService(ResourceStorage storage)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }
        
        public int GetAmount(ResourceType type)
        {
            return _storage.GetAmount(type);
        }

        public IReadOnlyDictionary<ResourceType, int> GetAll()
        {
            return _storage.GetAllAmounts();
        }

        public void Add(ResourceType type, int amount)
        {
            _storage.Add(type,amount);

            int newAmount = _storage.GetAmount(type);
            ResourceChanged?.Invoke(type, newAmount);
            ResourcesChanged?.Invoke();
        }

        public bool CanSpend(ResourceType type, int amount)
        {
            return _storage.Has(type, amount);
        }

        public bool TrySpend(ResourceType type, int amount)
        {
            bool success = _storage.TrySpend(type, amount);
            if (!success)
                return false;
            
            int newAmount = _storage.GetAmount(type);
            ResourceChanged?.Invoke(type, newAmount);
            ResourcesChanged?.Invoke();
            return true;
        }

        public bool CanAfford(IReadOnlyList<ResourceCost> costs)
        {
            if (costs is null)
                throw new ArgumentNullException(nameof(costs));
            
            foreach (var cost in costs)
            {
                if (!_storage.Has(cost.ResourceType, cost.Amount))
                    return false;
            }

            return true;
        }

        public bool TrySpend(IReadOnlyList<ResourceCost> costs)
        {
            if (costs is null)
                throw new ArgumentNullException(nameof(costs));

            var totals = new Dictionary<ResourceType, int>();

            foreach (var cost in costs)
            {
                if (cost.Amount <= 0)
                    throw new ArgumentException(nameof(costs));

                totals.TryGetValue(cost.ResourceType, out var current);
                totals[cost.ResourceType] = current + cost.Amount;
            }

            foreach (var total in totals)
            {
                if (!_storage.Has(total.Key, total.Value))
                    return false;
            }

            foreach (var total in totals)
            {
                if (!_storage.TrySpend(total.Key, total.Value))
                    throw new InvalidOperationException("Resource spending failed after affordability check.");
            }

            foreach (var total in totals)
            {
                ResourceChanged?.Invoke(total.Key, _storage.GetAmount(total.Key));
            }

            ResourcesChanged?.Invoke();
            return true;
        }
    }
}
