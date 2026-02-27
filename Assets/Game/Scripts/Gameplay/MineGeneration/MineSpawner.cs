using Game.Scripts.Configs;
using UnityEngine;

namespace Game.Scripts.Gameplay
{
    public class MineSpawner : MonoBehaviour
    {
        [SerializeField] private BlockDatabase _database;
        [SerializeField] private int _mineWidth, _mineHeight;
        [SerializeField] private BlockType _fillerBlockType;

        private MineField _mine;

        public MineField GenerateMine(int seed)
        {
            var fillerBlockSettings = _database.GetSettings(_fillerBlockType);
            var fillerBlock = new WorldBlock
            {
                BlockType = _fillerBlockType,
                Durability = fillerBlockSettings!._maxDurability
            };

            _mine = new MineField(_mineWidth, _mineHeight);

            FillMine(fillerBlock);

            CreateVeins(seed);

            CreateCaves(seed);

            return _mine;
        }

        private void CreateCaves(int seed)
        {
            var caveGenerator = new CaveGenerator(seed, _fillerBlockType, _database);
            caveGenerator.AddCaves(_mine, 3, 1, 3, 1, 3, 2, 10);
        }

        private void CreateVeins(int seed)
        {
            var veinGenerator = new VeinGenerator(seed, _fillerBlockType, _database);
            var coalSettings = _database.GetSettings(BlockType.Coal);
            veinGenerator.AddVeins(_mine, BlockType.Coal, 3, 2, 4, 3, coalSettings._spawnHeightMin,
                coalSettings._spawnHeightMax);

            var ironSettings = _database.GetSettings(BlockType.IronOre);
            veinGenerator.AddVeins(_mine, BlockType.Coal, 3, 2, 4, 3, ironSettings._spawnHeightMin,
                ironSettings._spawnHeightMax);
        }

        private void FillMine(WorldBlock fillerBlock)
        {
            for (int y = 0; y < _mineHeight; y++)
            {
                for (int x = 0; x < _mineWidth; x++)
                {
                    _mine.SetBlock(x, y, fillerBlock);
                }
            }
        }
    }
}
