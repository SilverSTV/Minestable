using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Pooling
{
    public sealed class ComponentPool<T> where T : Component
    {
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly Queue<T> _available = new();
        private readonly HashSet<T> _inUse = new();

        public int CountAll { get; private set; }
        public int CountAvailable => _available.Count;
        public int CountInUse => _inUse.Count;

        public ComponentPool(T prefab, int initialSize, Transform parent = null)
        {
            if (prefab == null)
            {
                throw new ArgumentNullException(nameof(prefab));
            }

            if (initialSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initialSize), "Initial size must be greater than zero.");
            }

            _prefab = prefab;
            _parent = parent;

            Expand(initialSize);
        }

        public T Acquire()
        {
            if (_available.Count == 0)
            {
                Expand(Mathf.Max(1, CountAll));
            }

            var instance = _available.Dequeue();
            _inUse.Add(instance);
            instance.gameObject.SetActive(true);
            return instance;
        }

        public T Acquire(Action<T> onAcquire)
        {
            var instance = Acquire();
            onAcquire?.Invoke(instance);
            return instance;
        }

        public void Release(T instance)
        {
            if (instance == null)
            {
                return;
            }

            if (!_inUse.Remove(instance))
            {
                return;
            }

            instance.gameObject.SetActive(false);
            _available.Enqueue(instance);
        }

        public void ReleaseAll()
        {
            if (_inUse.Count == 0)
            {
                return;
            }

            var activeInstances = new List<T>(_inUse);
            foreach (var instance in activeInstances)
            {
                Release(instance);
            }
        }

        private void Expand(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                var instance = UnityEngine.Object.Instantiate(_prefab, _parent);
                instance.gameObject.SetActive(false);
                _available.Enqueue(instance);
            }

            CountAll += amount;
        }
    }
}