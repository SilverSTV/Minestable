using Game.Scripts.Configs;

namespace Game.Scripts.Gameplay
{
    public class MineGenerator
    {
        private readonly BlockDatabase _database;
        private readonly CaveGenerationSettings _caveGeneration;
        private readonly VeinSettingsDatabase _veinSettingsDatabase;


        public MineGenerator(BlockDatabase db, CaveGenerationSettings caveGeneration,
            VeinSettingsDatabase veinSettingsDatabase)
        {
            _database = db;
            _caveGeneration = caveGeneration;
            _veinSettingsDatabase = veinSettingsDatabase;
        }

        public MineGrid GenerateMine(MineGrid mine, int seed, BlockType fillerBlockType, BlockType surfaceBlockType)
        {
            CreateSurface(mine, surfaceBlockType);


            FillMine(mine, fillerBlockType);

            CreateVeins(mine, seed, fillerBlockType);
            CreateCaves(mine, seed, fillerBlockType);

            return mine;
        }

        private void CreateCaves(MineGrid mine, int seed, BlockType fillerBlockType)
        {
            var caveGenerator = new CaveGenerator(seed, fillerBlockType, _database);
            caveGenerator.AddCaves(mine, _caveGeneration);
        }

        private void CreateVeins(MineGrid mine, int seed, BlockType fillerBlockType)
        {
            var veinGenerator = new VeinGenerator(seed, fillerBlockType, _database);

            veinGenerator.AddVeins(mine, _veinSettingsDatabase.Get(BlockType.Coal));

            veinGenerator.AddVeins(mine, _veinSettingsDatabase.Get(BlockType.IronOre));
        }

        private void CreateSurface(MineGrid mine, BlockType surfaceBlockType)
        {
            var surfaceBlockSettings = _database.GetSettings(surfaceBlockType);
            var surfaceBlock = new CellState
            {
                BlockType = surfaceBlockType,
                Durability = surfaceBlockSettings.MaxDurability
            };
            for (int x = 0; x < mine.Width; x++)
            {
                mine.SetBlock(x, 0, surfaceBlock);
            }
        }

        private void FillMine(MineGrid mine, BlockType fillerBlockType)
        {
            var fillerBlockSettings = _database.GetSettings(fillerBlockType);
            var fillerBlock = new CellState
            {
                BlockType = fillerBlockType,
                Durability = fillerBlockSettings.MaxDurability
            };

            for (int y = 1; y < mine.Height; y++)
            {
                for (int x = 0; x < mine.Width; x++)
                {
                    mine.SetBlock(x, y, fillerBlock);
                }
            }
        }
    }
}
