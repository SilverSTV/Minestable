using Game.Scripts.Configs;

namespace Game.Scripts.Gameplay
{
    public class WorldBlock
    {
        private int _durability;
        private BlockType _blockType;

        public BlockType BlockType
        {
            get => _blockType;
            set => _blockType = value;
        }

        public int Durability
        {
            get => _durability;
            set => _durability = value;
        }
    }
}