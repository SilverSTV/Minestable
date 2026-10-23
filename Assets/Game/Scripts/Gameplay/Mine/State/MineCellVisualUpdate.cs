using Game.Scripts.Gameplay.Mine.Core;

namespace Game.Scripts.Gameplay.Mine.State
{
    public struct MineCellVisualUpdate
    {
        public readonly int X, Y;

        public readonly BlockType CurrentBlockType;
        public readonly BlockDamageStage DamageState;

        public readonly BlockImpactType ImpactType;
        public readonly BlockType ImpactBlockType;

        public MineCellVisualUpdate(int x, int y, BlockType currentBlockType, BlockDamageStage damageState, BlockImpactType impactType, BlockType impactBlockType)
        {
            X = x;
            Y = y;
            CurrentBlockType = currentBlockType;
            DamageState = damageState;
            ImpactType = impactType;
            ImpactBlockType = impactBlockType;
        }
    }
}
