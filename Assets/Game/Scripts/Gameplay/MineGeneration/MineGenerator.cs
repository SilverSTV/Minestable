using Game.Scripts.Configs;

namespace Game.Scripts.Gameplay
{
    public class MineGenerator
    {
        private BlockDatabase _database;

        public MineGenerator(BlockDatabase db)
        {
            _database = db;
        }

        public MineGrid GenerateMine(MineGrid mine, int seed, BlockType fillerBlockType)
        {
            var fillerBlockSettings = _database.GetSettings(fillerBlockType);
            var fillerBlock = new CellState
            {
                BlockType = fillerBlockType,
                Durability = fillerBlockSettings.MaxDurability
            };

            FillMine(mine, fillerBlock);

            CreateVeins(mine, seed, fillerBlockType);
            CreateCaves(mine, seed, fillerBlockType);

            return mine;
        }

        private void CreateCaves(MineGrid mine, int seed, BlockType fillerBlockType)
        {
            var caveGenerator = new CaveGenerator(seed, fillerBlockType, _database);
            caveGenerator.AddCaves(mine, 3, 1, 3, 1, 3, 2, 10);
        }

        private void CreateVeins(MineGrid mine, int seed, BlockType fillerBlockType)
        {
            var veinGenerator = new VeinGenerator(seed, fillerBlockType, _database);
            var coalSettings = _database.GetSettings(BlockType.Coal);
            veinGenerator.AddVeins(mine, BlockType.Coal, 3, 2, 4, 3, coalSettings.SpawnHeightMin,
                coalSettings.SpawnHeightMax);

            var ironSettings = _database.GetSettings(BlockType.IronOre);
            veinGenerator.AddVeins(mine, BlockType.Coal, 3, 2, 4, 3, ironSettings.SpawnHeightMin,
                ironSettings.SpawnHeightMax);
        }

        private void FillMine(MineGrid mine, CellState fillerBlock)
        {
            for (int y = 0; y < mine.Height; y++)
            {
                for (int x = 0; x < mine.Width; x++)
                {
                    mine.SetBlock(x, y, fillerBlock);
                }
            }
        }
    }
}
