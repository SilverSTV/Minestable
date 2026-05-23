using System;
using UnityEngine;

namespace Game.Scripts.Gameplay.PlayerResources
{
    [Serializable]
    public sealed class ResourceDefinition
    {
        private ResourceType _resourceType;
        private string _displayName;
        private Sprite _icon;

        public ResourceType ResourceType => _resourceType;

        public string DisplayName => _displayName;

        public Sprite Icon => _icon;
    }
}
