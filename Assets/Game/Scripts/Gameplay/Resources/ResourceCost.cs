using System;

namespace Game.Scripts.Gameplay.PlayerResources
{
    [Serializable]
    public struct ResourceCost
    {
        public ResourceType ResourceType;
        public int Amount;

        public ResourceCost(ResourceType resourceType, int amount)
        {
            ResourceType = resourceType;
            Amount = amount;
        }
    }
}
