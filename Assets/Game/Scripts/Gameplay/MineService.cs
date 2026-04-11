using Game.Scripts.Configs;

namespace Game.Scripts.Gameplay
{
    public class MineService
    {
        private MineGrid _world;
        private BlockDatabase _blockSettings;
        private BlockType _emptyBlockType;

        public MineService(MineGrid world, BlockDatabase blockSettings, BlockType emptyBlockType)
        {
            _world = world;
            _blockSettings = blockSettings;
            _emptyBlockType = emptyBlockType;
        }

        private void CreateBlock(int x, int y, BlockType blockType)
        {
            var fillerBlockSettings = _blockSettings.GetSettings(blockType);
            var fillerBlock = new CellState
            {
                BlockType = blockType,
                Durability = fillerBlockSettings.MaxDurability
            };
            _world.SetBlock(x, y, fillerBlock);
        }

        private void HitBlock(int x, int y, int damage)
        {
            var block = _world.GetBlock(x, y);
            if (block.BlockType != BlockType.Unknown)
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
            var durability = _blockSettings.GetSettings(_emptyBlockType).MaxDurability;
            var emptyBlock = new CellState
            {
                BlockType = _emptyBlockType,
                Durability = durability
            };

            _world.SetBlock(x, y, emptyBlock);
        }
    }
}
