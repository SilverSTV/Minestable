using System.Collections.Generic;
using Game.Scripts.Gameplay;
using UnityEngine;

namespace Game.Scripts.Configs
{
    [CreateAssetMenu(fileName = "BlockDatabase", menuName = "Scriptable Objects/World/BlockDatabase")]
    public class BlockDatabase : ScriptableObject
    {
        [field: SerializeField] public List<BlockSettings> Settings { get; private set; }

        private Dictionary<BlockType, BlockSettings> _settingsMap;

        public void Init()
        {
            _settingsMap = new Dictionary<BlockType, BlockSettings>();

            foreach (var setting in Settings)
            {
                var settingBlockTypeId = setting.BlockTypeId;
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
