using System;
using System.Collections.Generic;
using Game.Scripts.Gameplay;
using UnityEngine;

namespace Game.Scripts.Configs
{
    public abstract class ScriptableObjectDatabase<TKey, TItem> : ScriptableObject
        where TKey : struct, Enum
        where TItem : ScriptableObject, IEnumKey<TKey>
    {
        [SerializeField] private List<TItem> _items = new();

        public IReadOnlyList<TItem> Items => _items;

        private Dictionary<TKey, TItem> _itemsById;

        protected virtual void OnEnable()
        {
            BuildDictionary();
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            BuildDictionary();
        }
#endif

        private void BuildDictionary()
        {
            _itemsById = new Dictionary<TKey, TItem>();

            foreach (var item in _items)
            {
                if (item == null)
                    continue;

                var id = item.Id;

                if (_itemsById.ContainsKey(id))
                {
                    Debug.LogError(
                        $"Duplicate key '{id}' in {name}. Item '{item.name}' was skipped.",
                        this
                    );

                    continue;
                }

                _itemsById.Add(id, item);
            }
        }
        
        public bool TryGet(TKey id, out TItem item)
        {
            EnsureDictionary();
            return _itemsById.TryGetValue(id, out item);
        }

        public TItem Get(TKey id)
        {
            EnsureDictionary();

            if (_itemsById.TryGetValue(id, out var item))
                return item;

            throw new KeyNotFoundException(
                $"Item with key '{id}' was not found in database '{name}'."
            );
        }

        private void EnsureDictionary()
        {
            if (_itemsById == null)
                BuildDictionary();
        }
    }
}
