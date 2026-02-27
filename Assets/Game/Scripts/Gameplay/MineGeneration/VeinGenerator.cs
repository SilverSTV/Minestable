using Game.Scripts.Configs;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;


namespace Game.Scripts.Gameplay
{
    public class VeinGenerator
    {
        private int _seed;
        private BlockType _fillerBlock;
        private BlockDatabase _database;

        public VeinGenerator(int seed, BlockType fillerBlock, BlockDatabase database)
        {
            _seed = seed;
            _fillerBlock = fillerBlock;
            _database = database;
        }

        public void AddVeins(MineField mine, BlockType oreType, int veinCount, int stepsMin, int stepsMax,
            int thicknessMax, int minY, int maxY)
        {
            var rng = new Random(_seed ^ (int) oreType * 10007);

            for (int v = 0; v < veinCount; v++)
            {
                int x = rng.Next(0, mine.Width);
                int y = rng.Next(minY, maxY);

                int steps = rng.Next(stepsMin, stepsMax + 1);
                int thickness = rng.Next(1, thicknessMax + 1);

                CarveRandomWalk(mine, oreType, x, y, steps, thickness, rng);
            }
        }

        private void CarveRandomWalk(MineField mine, BlockType oreType, int x, int y, int steps, int thickness,
            Random rng)
        {
            var oreCfg = _database.GetSettings(oreType);

            for (int i = 0; i < steps; i++)
            {
                PainDisc(mine, oreType, x, y, thickness, oreCfg._maxDurability);

                int dir = rng.Next(0, 6);
                switch (dir)
                {
                    case 0:
                        x += 1;
                        break;
                    case 1:
                        x -= 1;
                        break;
                    case 2:
                        y += 1;
                        break;
                    case 3:
                        y -= 1;
                        break;
                    case 4:
                        x += 1;
                        y += 1;
                        break;
                    case 5:
                        x -= 1;
                        y -= 1;
                        break;
                }

                x = Mathf.Clamp(x, 0, mine.Width - 1);
                y = Mathf.Clamp(y, 0, mine.Height - 1);

                if (rng.NextDouble() < 0.15)
                    thickness = Mathf.Clamp(thickness + (rng.Next(0, 2) == 0 ? -1 : 1), 1, 4);
            }
        }

        private void PainDisc(MineField mine, BlockType oreType, int cx, int cy, int radius, int durability)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                for (int dx = 0; dx <= radius; dx++)
                {
                    if (dx * dx + dy * dy > radius * radius)
                        continue;

                    int x = cx + dx;
                    int y = cy + dy;
                    if (!mine.InBounds(x, y))
                        continue;

                    var existing = mine.GetBlock(x, y);
                    if (existing.BlockType != _fillerBlock)
                        continue;

                    var block = new WorldBlock
                    {
                        BlockType = oreType,
                        Durability = durability
                    };
                    mine.SetBlock(x, y, block);
                }
            }
        }
    }
}
