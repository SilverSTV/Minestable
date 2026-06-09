using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Gameplay.Mine.Core;
using Game.Scripts.Gameplay.Mine.State;
using Game.Scripts.Infrastructure.Pooling;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Gameplay.View
{
    public class BlockImpactVfx : MonoBehaviour
    {
        [SerializeField] private float _startSizeHit = 0.1f;
        [SerializeField] private float _startSizeDestroy;

        [SerializeField] private ParticleSystem _blockHitParticles;
        [SerializeField] private ParticleSystem _blockDestroyParticles;

        private BlockParticleColorCache _colorCache;
        private ComponentPool<ParticleSystem> _hitPool;
        private ComponentPool<ParticleSystem> _destroyPool;


        public void Initialize(BlockParticleColorCache colorCache)
        {
            _colorCache = colorCache;
            _hitPool = new ComponentPool<ParticleSystem>(_blockHitParticles, 8, transform);
            _destroyPool = new ComponentPool<ParticleSystem>(_blockDestroyParticles, 8, transform);
        }

        public void HandleMineChanges(List<MineCellVisualUpdate> changes)
        {
            foreach (var cellChange in changes)
            {
                var blockType = cellChange.ImpactBlockType;
                var pos = new Vector3(cellChange.X +0.5f, -cellChange.Y +0.5f);
                switch (cellChange.ImpactType)
                {
                    case BlockImpactType.None:
                        break;
                    case BlockImpactType.Hit:
                        PlayHit(blockType,pos);
                        break;
                    case BlockImpactType.Destroy:
                        PlayDestroy(blockType,pos);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void PlayHit(BlockType blockType, Vector3 worldPosition)
        {
            var hitPs = _hitPool.Acquire(ps =>
            {
                for (int i = 0; i < 6; i++)
                {
                    var emitParams = new ParticleSystem.EmitParams
                    {
                        position = worldPosition,
                        startColor = _colorCache.GetRandomColor(blockType),
                        startSize = _startSizeHit,
                        velocity = new Vector3(
                            Random.Range(-0.4f, 0.4f),
                            Random.Range(1.8f, 3.0f),
                            0)
                    };

                    ps.Emit(emitParams, 1);
                }
            });

            StartCoroutine(ReleaseWhenFinished(hitPs,_hitPool));
        }

        private void PlayDestroy(BlockType blockType, Vector3 worldPosition)
        {
            var destroyPs = _destroyPool.Acquire(ps =>
            {
                for (int i = 0; i < 6; i++)
                {
                    var emitParams = new ParticleSystem.EmitParams
                    {
                        position = worldPosition,
                        startColor = _colorCache.GetRandomColor(blockType),
                        startSize = _startSizeDestroy,
                        velocity = new Vector3(
                            Random.Range(-0.4f, 0.4f),
                            Random.Range(1.8f, 3.0f),
                            0)
                    };

                    ps.Emit(emitParams, 1);
                }
            });

            StartCoroutine(ReleaseWhenFinished(destroyPs,_destroyPool));
        }

        private IEnumerator ReleaseWhenFinished(ParticleSystem particleSystem, ComponentPool<ParticleSystem> pool)
        {
            yield return new WaitUntil(() => !particleSystem.IsAlive(true));
            pool.Release(particleSystem);
        }
    }
}
