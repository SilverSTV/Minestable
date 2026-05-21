using System.Collections.Generic;
using Game.Scripts.Gameplay;
using UnityEngine;

namespace Game.Scripts.Configs
{
    [CreateAssetMenu(menuName = "Game/Generation/Mine/Veins Settings Database")]
    public class VeinSettingsDatabase : ScriptableObjectDatabase<BlockType,VeinGenerationSettings>
    {
    }
}
