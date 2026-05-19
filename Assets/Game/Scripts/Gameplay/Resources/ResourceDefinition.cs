using System;
using UnityEngine;

namespace Game.Scripts.Gameplay.PlayerResources
{
    [Serializable]
    public sealed class ResourceDefinition
    {
        public ResourceType ResourceType;
        public string DisplayName;
        public Sprite Icon;
    }
}
