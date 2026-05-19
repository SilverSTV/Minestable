using System;
using System.Collections.Generic;

namespace Game.Scripts.Gameplay.PlayerResources
{
    public class ResourceStorage
    {
        private readonly Dictionary<ResourceType, int> _amounts = new();

        public int GetAmount(ResourceType type)
        {
            return _amounts.TryGetValue(type, out int value) ? value : 0;
        }

        public IReadOnlyDictionary<ResourceType, int> GetAllAmounts()
        {
            return _amounts;
        }

        public void SetAmount(ResourceType type, int amount)
        {
            if (amount < 0)
                throw new ArgumentException(nameof(amount));

            if (amount == 0)
                _amounts.Remove(type);
            else
                _amounts[type] = amount;
        }

        public void Add(ResourceType type, int amount)
        {
            if (amount <= 0)
                throw new ArgumentException(nameof(amount));

            int current = GetAmount(type);
            _amounts[type] = current + amount;
        }

        public bool Has(ResourceType type, int amount)
        {
            if (amount <= 0)
                return false;

            return GetAmount(type) >= amount;
        }

        public bool TrySpend(ResourceType type, int amount)
        {
            if (!Has(type, amount))
                return false;

            int current = GetAmount(type);
            if (amount == current)
                _amounts.Remove(type);
            else
            {
                _amounts[type] = current - amount;
            }

            return true;
        }
    }
}
