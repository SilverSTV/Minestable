using System.Collections.Generic;
using Game.Scripts.Gameplay;
using UnityEngine;

namespace Game.Scripts.Configs
{
    [CreateAssetMenu(fileName = "BlockConfig", menuName = "Scriptable Objects/World/BlockConfig")]
    public class BlockSettings : ScriptableObject
    {
        public BlockType _blockTypeId;
        public string _configId;
        public string _spritePath;
        public int _maxDurability;
        public int _stabilityDamage;
        public int _dropAmountMin, _dropAmountMax;
        public int _spawnHeightMin, _spawnHeightMax;
        public List<ItemId> _dropItems;
        public bool _isSolid;
        public bool _isAffectedByGravity;
    }
}

