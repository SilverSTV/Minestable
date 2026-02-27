using System;
using System.Collections.Generic;
using System.IO.Hashing;
using System.Linq;
using Game.Scripts.Configs;
using UnityEngine;

namespace Game.Scripts.Gameplay
{
    public class MineService : MonoBehaviour
    {
        private MineField _world;

        [SerializeField] private List<BlockSettings> _blockSettings;
        [SerializeField] private BlockType _fillerBlockType;

        private int _fillerBlockDurability;

        private void Start()
        {
            _fillerBlockDurability = _blockSettings.FirstOrDefault(bs => bs._blockTypeId == _fillerBlockType)!
                ._maxDurability;
        }

        private void CreateBlock(int x, int y, BlockType blockType)
        {
            var fillerBlockSettings = _blockSettings.FirstOrDefault(bs => bs._blockTypeId == blockType);
            var fillerBlock = new WorldBlock
            {
                BlockType = blockType,
                Durability = fillerBlockSettings!._maxDurability
            };
            _world.SetBlock(x, y, fillerBlock);
        }

        private void HitBlock(int x, int y, int damage)
        {
            var block = _world.GetBlock(x, y);
            if (block != null)
            {
                block.Durability -= damage;
                if (block.Durability <= 0)
                {
                    DestroyBlock(x, y);
                }
            }
        }

        private void DestroyBlock(int x, int y)
        {
            var fillerBlock = new WorldBlock
            {
                BlockType = _fillerBlockType,
                Durability = _fillerBlockDurability
            };

            _world.SetBlock(x, y, fillerBlock);
        }
    }
}
