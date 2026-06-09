using System.Collections.Generic;
using Game.Scripts.Configs;
using Game.Scripts.Gameplay.Mine.Core;
using UnityEngine;

namespace Game.Scripts.Gameplay.View
{
    public sealed class BlockParticleColorCache
    {
        private readonly BlockDatabase _blockDatabase;
        private readonly Dictionary<BlockType, List<Color32>> _colorsByBlock = new();

        public BlockParticleColorCache(BlockDatabase blockDatabase)
        {
            _blockDatabase = blockDatabase;
        }

        public Color32 GetRandomColor(BlockType blockType)
        {
            if (!_colorsByBlock.TryGetValue(blockType, out var colors))
            {
                var sprite = _blockDatabase.GetSettings(blockType).Sprite;
                colors = SpritePixelSampler.CollectOpaquePixelsFromAtlasSprite(sprite);
                _colorsByBlock[blockType] = colors;
            }

            if (colors == null || colors.Count == 0)
            {
                return Color.white;
            }

            return colors[Random.Range(0, colors.Count)];
        }
    }
}
