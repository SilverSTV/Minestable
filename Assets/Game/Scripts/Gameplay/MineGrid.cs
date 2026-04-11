using UnityEngine;

namespace Game.Scripts.Gameplay
{
    public class MineGrid
    {
        private CellState[,] _worldGrid;
        private int _width, _height;

        public MineGrid(int width, int height)
        {
            _width = width;
            _height = height;
            _worldGrid = new CellState[Width, Height];
        }

        public int Height => _height;

        public int Width => _width;

        public CellState GetBlock(int x, int y)
        {
            if (InBounds(x, y))
                return _worldGrid[x, y];
            
            return new CellState
            {
                BlockType = BlockType.Unknown
            };
        }

        public void SetBlock(int x, int y, CellState block)
        {
            if (InBounds(x, y))
                _worldGrid[x, y] = block;
        }

        public bool InBounds(int x, int y)
        {
            return x >= 0 && x < Width &&
                   y >= 0 && y < Height;
        }

        public bool InBounds(Vector2Int pos)
        {
            return InBounds(pos.x, pos.y);
        }

        public void SwapBlocks(int x1, int y1, int x2, int y2)
        {
            if (InBounds(x1, y1) && InBounds(x2, y2))
            {
                var block1 = GetBlock(x1, y1);
                var block2 = GetBlock(x2, y2);
                var tmpBlock = block1;
                _worldGrid[x1, y1] = block2;
                _worldGrid[x2, y2] = tmpBlock;
            }
        }
    }
}
