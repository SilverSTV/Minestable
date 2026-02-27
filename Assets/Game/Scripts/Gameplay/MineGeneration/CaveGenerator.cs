using System;
using Game.Scripts.Configs;

namespace Game.Scripts.Gameplay
{
    public class CaveGenerator
    {
        private int _seed;
        private BlockType _fillerBlock;
        private BlockDatabase _database;

        public CaveGenerator(int seed, BlockType fillerBlock, BlockDatabase database)
        {
            _seed = seed;
            _fillerBlock = fillerBlock;
            _database = database;
        }

        public void AddCaves(MineField mine, int caveCount, int radiusXMin, int radiusXMax, int radiusYMin,
            int radiusYMax, int yMin, int yMax)
        {
            var rng = new Random(_seed ^ 0x51C0FFEE);

            for (int i = 0; i < caveCount; i++)
            {
                var cx = rng.Next(0, mine.Width);
                var cy = rng.Next(yMin, yMax);

                var rx = rng.Next(radiusXMin, radiusXMax + 1);
                var ry = rng.Next(radiusYMin, radiusYMax + 1);

                CarveEllipse(mine, cx, cy, rx, ry);
            }
        }

        private void CarveEllipse(MineField mine, int cx, int cy, int rx, int ry)
        {
            for (int y = cy - ry; y <= cy + ry; y++)
            {
                for (int x = cx - rx; x <= cx + rx; x++)
                {
                    if (!mine.InBounds(x, y))
                        continue;

                    float nx = (x - cx) / (float) rx;
                    float ny = (y - cy) / (float) ry;

                    if (nx * nx + ny * ny <= 1f)
                    {
                        var existing = mine.GetBlock(x, y);
                        if (existing.BlockType != _fillerBlock)
                            continue;

                        var airBlock = new WorldBlock
                        {
                            BlockType = BlockType.Air,
                            Durability = 0
                        };
                        mine.SetBlock(x, y, airBlock);
                    }
                }
            }
        }
    }
}
