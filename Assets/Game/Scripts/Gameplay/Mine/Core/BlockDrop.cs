using System;

namespace Game.Scripts.Gameplay
{
    [Serializable]
    public struct BlockDrop
    {
        public ResourceType ResourceType;
        public int MinAmount;
        public int MaxAmount;

        public BlockDrop(ResourceType resourceType, int minAmount, int maxAmount)
        {
            ResourceType = resourceType;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
        }
    }
}
