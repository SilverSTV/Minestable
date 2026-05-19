using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Game.Scripts.Gameplay.PlayerResources
{
    public interface IResourceService
    {
        event Action<ResourceType, int> ResourceChanged;
        event Action ResourcesChanged;

        int GetAmount(ResourceType type);
        IReadOnlyDictionary<ResourceType, int> GetAll();

        void Add(ResourceType type, int amount);
        bool CanSpend(ResourceType type, int amount);
        bool TrySpend(ResourceType type, int amount);

        bool CanAfford(IReadOnlyList<ResourceCost> costs);
        bool TrySpend(IReadOnlyList<ResourceCost> costs);
    }
}
