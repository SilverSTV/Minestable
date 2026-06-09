using System;
using Game.Scripts.Configs;
using Game.Scripts.Gameplay.Mine.Core;

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

        public void AddCaves(MineGrid mine, CaveGenerationSettings cgs)
        {
            var rng = new Random(_seed ^ 0x51C0FFEE);

            for (int i = 0; i < cgs.CaveCount; i++)
            {
                var cx = rng.Next(0, mine.Width);
                var cy = rng.Next(cgs.YMin,cgs.YMax);

                var rx = rng.Next(cgs.RadiusXMin, cgs.RadiusXMax + 1);
                var ry = rng.Next(cgs.RadiusYMin, cgs.RadiusYMax + 1);

                CarveEllipse(mine, cx, cy, rx, ry);
            }
        }

        private void CarveEllipse(MineGrid mine, int cx, int cy, int rx, int ry)
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

                        var airBlock = new CellState
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
