using System;
using UnityEngine;

namespace Game.Scripts.Gameplay.Resources
{
    [Serializable]
    public sealed class ResourceDefinition
    {
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private string _displayName;
        [SerializeField] private Sprite _icon;

        public ResourceType ResourceType => _resourceType;
        public string DisplayName => _displayName;
        public Sprite Icon => _icon;
    }
}
