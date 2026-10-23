using Game.Scripts.Gameplay.Mine.Core;
using UnityEngine;

namespace Game.Scripts.Gameplay.Mine.Collapse
{
    public class CollapseRuleEvaluator
    {
        private readonly MineGrid _mineGrid;

        public CollapseRuleEvaluator(MineGrid mineGrid)
        {
            _mineGrid = mineGrid;
        }

        public CollapseDirection EvaluateBlockCollapseDirection(Vector2Int blockPos)
        {
            var isBottomEmpty = _mineGrid.GetBlock(blockPos.x, blockPos.y - 1).BlockType == BlockType.Air;
            if (isBottomEmpty)
                return CollapseDirection.FallDown;

            var isLeftEmpty = _mineGrid.GetBlock(blockPos.x - 1, blockPos.y).BlockType == BlockType.Air;
            var isRightEmpty = _mineGrid.GetBlock(blockPos.x + 1, blockPos.y).BlockType == BlockType.Air;

            if (!isLeftEmpty && !isRightEmpty)
                return CollapseDirection.None;

            if (isLeftEmpty && !isRightEmpty)
                return CollapseDirection.SlideLeft;
            if (!isLeftEmpty)
                return CollapseDirection.SlideRight;

            var direction = Random.Range(0, 2);
            if (direction == 0)
                return CollapseDirection.SlideRight;

            return CollapseDirection.SlideLeft;
        }
    }
}
