using System.Collections.Generic;
using Game.Scripts.Gameplay.PlayerResources;
using Game.Scripts.Gameplay.Resources;
using UnityEngine;
using ResourceType = Game.Scripts.Gameplay.ResourceType;

namespace Game.Scripts.Configs
{
    [CreateAssetMenu(menuName = "Game/Resources/Resource Database")]
    public class ResourceDatabase : ScriptableObject
    {
        [SerializeField] private List<ResourceDefinition> _resources = new();

        public IReadOnlyList<ResourceDefinition> Resources => _resources;

        private Dictionary<ResourceType, ResourceDefinition> _cache;

        public ResourceDefinition Get(ResourceType resourceType)
        {
            EnsureCache();

            if (_cache.TryGetValue(resourceType, out var definition))
                return definition;

            throw new KeyNotFoundException($"Resource definition not found for {resourceType}");
        }

        private void EnsureCache()
        {
            if(_cache is not null)
                return;

            _cache = new Dictionary<ResourceType, ResourceDefinition>(_resources.Count);

            foreach (var resource in _resources)
            {
                _cache[resource.ResourceType] = resource;
            }
        }
    }
}
