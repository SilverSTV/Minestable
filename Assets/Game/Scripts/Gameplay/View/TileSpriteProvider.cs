using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Scripts.Configs;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.Scripts.Gameplay
{
    public sealed class TileSpriteProvider : IDisposable
    {
        private readonly Dictionary<string, Sprite> _spriteCache = new();
        private readonly Dictionary<string, Task<Sprite>> _loadingTasks = new();
        private readonly Dictionary<string, AsyncOperationHandle<Sprite>> _handles = new();

        public async Task Preload(IEnumerable<BlockSettings> blocks)
        {
            foreach (var block in blocks)
            {
                await GetSpriteAsync(block.SpriteReference);
            }
        }
        
        public async Task<Sprite> GetSpriteAsync(AssetReferenceSprite spriteReference)
        {
            if (spriteReference == null || !spriteReference.RuntimeKeyIsValid())
            {
                return null;
            }

            string spriteKey = GetCacheKey(spriteReference);

            if (_spriteCache.TryGetValue(spriteKey, out var cachedSprite) && cachedSprite != null)
            {
                return cachedSprite;
            }

            if (_loadingTasks.TryGetValue(spriteKey, out var loadingTask))
            {
                return await loadingTask;
            }

            var task = LoadSpriteAsync(spriteReference, spriteKey);
            _loadingTasks[spriteKey] = task;

            try
            {
                return await task;
            }
            finally
            {
                _loadingTasks.Remove(spriteKey);
            }
        }

        public void Dispose()
        {
            foreach (var handle in _handles.Values)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }

            _handles.Clear();
            _loadingTasks.Clear();
            _spriteCache.Clear();
        }

        private async Task<Sprite> LoadSpriteAsync(AssetReferenceSprite spriteReference, string spriteKey)
        {
            var handle = spriteReference.LoadAssetAsync();

            try
            {
                var sprite = await handle.Task;
                if (handle.Status != AsyncOperationStatus.Succeeded || sprite == null)
                {
                    if (handle.IsValid())
                    {
                        Addressables.Release(handle);
                    }

                    return null;
                }

                _handles[spriteKey] = handle;
                _spriteCache[spriteKey] = sprite;
                return sprite;
            }
            catch
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }

                throw;
            }
        }

        private static string GetCacheKey(AssetReferenceSprite spriteReference)
        {
            return string.IsNullOrWhiteSpace(spriteReference.AssetGUID)
                ? spriteReference.RuntimeKey.ToString()
                : spriteReference.AssetGUID;
        }
    }
}
