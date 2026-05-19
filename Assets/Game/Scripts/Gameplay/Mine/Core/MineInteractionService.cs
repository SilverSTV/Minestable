using System.Collections.Generic;
using Game.Scripts.Configs;
using Game.Scripts.Gameplay.PlayerResources;
using UnityEngine;

namespace Game.Scripts.Gameplay
{
    public class MineInteractionService
    {
        private readonly MineGrid _world;
        private readonly BlockDatabase _blocksSettings;
        private readonly BlockType _emptyBlockType;
        private readonly List<CellChange> _changes = new();
        private readonly IResourceService _resourceService;

        public MineInteractionService(MineGrid world, BlockDatabase blocksSettings, BlockType emptyBlockType,
            IResourceService resourceService)
        {
            _world = world;
            _blocksSettings = blocksSettings;
            _emptyBlockType = emptyBlockType;
            _resourceService = resourceService;
        }

        public List<CellChange> Changes => _changes;

        public void TryDamageBlock(int x, int y, int damage)
        {
            if (!_world.InBounds(x, y) || damage <= 0)
            {
                return;
            }

            var block = _world.GetBlock(x, y);
            if (block.BlockType == BlockType.Air || block.BlockType == BlockType.Unknown)
            {
                return;
            }

            block.Durability -= damage;
            if (block.Durability <= 0)
            {
                var destroyedCell = DestroyBlock(x, y, block.BlockType);
                return;
            }

            _world.SetBlock(x, y, block);
            Changes.Add(new CellChange(x, y, block));
        }

        public void ClearChanges()
        {
            _changes.Clear();
        }

        private void CreateBlock(int x, int y, BlockType blockType)
        {
            var fillerBlockSettings = _blocksSettings.GetSettings(blockType);
            var fillerBlock = new CellState
            {
                BlockType = blockType,
                Durability = fillerBlockSettings.MaxDurability
            };
            _world.SetBlock(x, y, fillerBlock);
        }

        private CellState DestroyBlock(int x, int y, BlockType destroyedBlockType)
        {
            var durability = _blocksSettings.GetSettings(_emptyBlockType).MaxDurability;
            var emptyBlock = new CellState
            {
                BlockType = _emptyBlockType,
                Durability = durability
            };

            var destroyedBlockSettings = _blocksSettings.GetSettings(destroyedBlockType);
            foreach (var dropResource in destroyedBlockSettings.DropResources)
            {
                var dropAmount = Random.Range(dropResource.MinAmount, dropResource.MaxAmount + 1);
                _resourceService.Add(dropResource.ResourceType, dropAmount);
            }

            _world.SetBlock(x, y, emptyBlock);
            Changes.Add(new CellChange(x, y, emptyBlock));
            return emptyBlock;
        }
    }
}
