using UnityEngine;

namespace Game.Scripts.Configs
{
    [CreateAssetMenu(fileName = "WorldSettings", menuName = "Scriptable Objects/World/WorldSettings")]
    public class WorldSettings : ScriptableObject
    {
        [SerializeField] private int _worldWidth;
        [SerializeField] private int _worldDepth;
    }
}
