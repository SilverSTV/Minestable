using System;
using System.Collections.Generic;
using Game.Scripts.Configs;
using Game.Scripts.Gameplay.PlayerResources;
using UnityEngine;

namespace Game.Scripts.Gameplay.View.UI
{
    public class ResourcePanelView : MonoBehaviour
    {
        [SerializeField] private ResourceView _resourceViewPrefab;
        [SerializeField] private ResourceDatabase _database;
        [SerializeField] private Transform _contentRoot;

        private IResourceService _resourceService;
        private Dictionary<ResourceType, ResourceView> _views = new();

        public void Init(IResourceService service)
        {
            _resourceService = service;

            BuildViews();
            RefreshAll();

            _resourceService.ResourceChanged += OnResourceChanged;
        }

        public void OnDestroy()
        {
            if (_resourceService is not null)
                _resourceService.ResourceChanged -= OnResourceChanged;
        }

        private void BuildViews()
        {
            _views.Clear();

            var resources = _database.Resources;

            foreach (var resource in resources)
            {
                var view = Instantiate(_resourceViewPrefab, _contentRoot);
                view.Init(resource);

                _views.Add(resource.ResourceType, view);
            }
        }

        private void RefreshAll()
        {
            var resources = _database.Resources;

            foreach (var resource in resources)
            {
                var resourceType = resource.ResourceType;
                var amount = _resourceService.GetAmount(resourceType);

                if (_views.TryGetValue(resourceType, out var view))
                    view.SetAmount(amount);
            }
        }

        private void OnResourceChanged(ResourceType type, int amount)
        {
            if (_views.TryGetValue(type, out var view))
                view.SetAmount(amount);
        }
    }
}
