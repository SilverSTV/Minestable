using System.Collections.Generic;
using Game.Scripts.Gameplay;
using UnityEngine;

namespace Game.Scripts.Configs
{
    [CreateAssetMenu(fileName = "BlockDatabase", menuName = "Scriptable Objects/World/BlockDatabase")]
    public class BlockDatabase : ScriptableObject
    {
        [SerializeField] private List<BlockSettings> _settings;

        private Dictionary<BlockType, BlockSettings> _settingsMap;

        public void Init()
        {
            _settingsMap = new Dictionary<BlockType, BlockSettings>();

            foreach (var setting in _settings)
            {
                var settingBlockTypeId = setting._blockTypeId;
                if (_settingsMap.ContainsKey(settingBlockTypeId))
                    Debug.LogWarning($"Settings map already has {settingBlockTypeId} block type!");

                _settingsMap[settingBlockTypeId] = setting;
            }
        }

        public BlockSettings GetSettings(BlockType blockType)
        {
            if (_settingsMap is null)
                Init();
            return _settingsMap[blockType];
        }
    }
}
