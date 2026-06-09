using Game.Scripts.Gameplay;
using Game.Scripts.Gameplay.Mine.Core;
using UnityEngine;

namespace Game.Scripts.Configs
{
    [CreateAssetMenu(menuName = "Game/Generation/Mine/Veins Generation Settings")]
    public class VeinGenerationSettings : ScriptableObject,IEnumKey<BlockType>
    {
        [SerializeField] private BlockType _id;
        [SerializeField] private int _veinCount;
        [SerializeField] private int _stepsMin;
        [SerializeField] private int _stepsMax;
        [SerializeField] private int _thicknessMax;
        [SerializeField] private int _minY;
        [SerializeField] private int _maxY;


        public BlockType Id => _id;

        
        public int VeinCount => _veinCount;

        
        public int StepsMin => _stepsMin;

        
        public int StepsMax => _stepsMax;

        
        public int ThicknessMax => _thicknessMax;

        
        public int MinY => _minY;

        
        public int MaxY => _maxY;
    }
}
