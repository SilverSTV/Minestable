using Game.Scripts.Gameplay.Mine.Core;
using UnityEngine;

namespace Game.Scripts.Gameplay.Mine.State
{
    public class MineVisualUpdateFactory
    {
        public MineCellVisualUpdate CreateHit(int x, int y, BlockType blockType, float durabilityPercent)
        {
            return new MineCellVisualUpdate(
                x,
                y,
                blockType,
                ResolveDamageState(durabilityPercent),
                BlockImpactType.Hit,
                blockType
            );
        }

        public MineCellVisualUpdate CreateDestroy(int x, int y, BlockType destroyedBlock, BlockType currentBlock)
        {
            return new MineCellVisualUpdate(
                x,
                y,
                currentBlock,
                BlockDamageStage.None,
                BlockImpactType.Destroy,
                destroyedBlock
            );
        }

        private BlockDamageStage ResolveDamageState(float durabilityPercent)
        {
            if (durabilityPercent >= 0.75f) return BlockDamageStage.Low;
            if (durabilityPercent >= 0.50f) return BlockDamageStage.Medium;
            if (durabilityPercent >= 0.25f) return BlockDamageStage.High;
            if (durabilityPercent > 0f) return BlockDamageStage.Critical;
            return BlockDamageStage.None;
        }
    }
}
