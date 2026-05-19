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

            if (!CanAfford(costs))
                return false;

            foreach (var cost in costs)
            {
                if (!TrySpend(cost.ResourceType, cost.Amount))
                    throw new InvalidOperationException("Resource spending failed after CanAfford check.");
            }

            foreach (var cost in costs)
            {
                int newAmount = GetAmount(cost.ResourceType);
                ResourceChanged?.Invoke(cost.ResourceType,newAmount);
            }
            ResourcesChanged?.Invoke();

            return true;
        }
    }
}
