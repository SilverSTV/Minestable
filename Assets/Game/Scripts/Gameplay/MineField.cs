using System.IO.Hashing;
using UnityEngine;

namespace Game.Scripts.Gameplay
{
    public class MineField
    {
        private WorldBlock[,] _worldGrid;
        private int _width, _height;

        public MineField(int width, int height, WorldBlock fillerBlock)
        {
            _width = width;
            _height = height;
            _worldGrid = new WorldBlock[_width, _height];// Заполнение блоком-заполнителем
        }

        public WorldBlock GetBlock(int x, int y)
        {
            if (InBounds(x, y))
                return _worldGrid[x, y];

            return null;
        }

        public void SetBlock(int x, int y, WorldBlock block)
        {
            if (InBounds(x, y))
                _worldGrid[x, y] = block;
        }

        public bool InBounds(int x, int y)
        {
            return x >= 0 && x < _width &&
                   y >= 0 && y < _height;
        }

        public bool InBounds(Vector2Int pos)
        {
            return InBounds(pos.x, pos.y);
        }

        public void SwapBlocks(int x1, int y1, int x2, int y2)
        {
            if(InBounds(x1,y1) && InBounds(x2,y2))
            {
                var block1 = GetBlock(x1, y1);
                var block2 = GetBlock(x2, y2);
                (block1, block2) = (block2, block1);
            }
        }
    }
}
