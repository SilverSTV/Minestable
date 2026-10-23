using System.Security.Cryptography.X509Certificates;
using Game.Scripts.Gameplay.PlayerResources;
using UnityEngine;

namespace Game.Scripts.Configs
{
    [CreateAssetMenu(menuName = "Game/Generation/Mine/Cave Generation Settings")]
    public class CaveGenerationSettings : ScriptableObject
    {
        [SerializeField] private int _caveCount;
        [SerializeField] private int _radiusXMin;
        [SerializeField] private int _radiusXMax;
        [SerializeField] private int _radiusYMin;
        [SerializeField] private int _radiusYMax;
        [SerializeField] private int _yMin;
        [SerializeField] private int _yMax;


        public int CaveCount => _caveCount;

        
        public int RadiusXMin => _radiusXMin;

        
        public int RadiusXMax => _radiusXMax;

        
        public int RadiusYMin => _radiusYMin;

        
        public int RadiusYMax => _radiusYMax;

        
        public int YMin => _yMin;

        
        public int YMax => _yMax;
    }
}
