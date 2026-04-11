using System.Collections.Generic;
using Game.Scripts.Gameplay;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace Game.Scripts.Configs
{
    [CreateAssetMenu(fileName = "BlockConfig", menuName = "Scriptable Objects/World/BlockConfig")]
    public class BlockSettings : ScriptableObject
    {
        [field: SerializeField, FormerlySerializedAs("_blockTypeId")]
        public BlockType BlockTypeId { get; private set; }

        [field: SerializeField, FormerlySerializedAs("_configId")]
        public string ConfigId { get; private set; }

        [field: SerializeField, FormerlySerializedAs("_spriteKey")]
        public AssetReferenceSprite SpriteReference { get; private set; }

        [field: SerializeField, FormerlySerializedAs("_maxDurability")]
        public int MaxDurability { get; private set; }

        [field: SerializeField, FormerlySerializedAs("_stabilityDamage")]
        public int StabilityDamage { get; private set; }

        [field: SerializeField, FormerlySerializedAs("_dropAmountMin")]
        public int DropAmountMin { get; private set; }

        [field: SerializeField, FormerlySerializedAs("_dropAmountMax")]
        public int DropAmountMax { get; private set; }

        [field: SerializeField, FormerlySerializedAs("_spawnHeightMin")]
        public int SpawnHeightMin { get; private set; }

        [field: SerializeField, FormerlySerializedAs("_spawnHeightMax")]
        public int SpawnHeightMax { get; private set; }

        [field: SerializeField, FormerlySerializedAs("_dropItems")]
        public List<ItemId> DropItems { get; private set; }

        [field: SerializeField, FormerlySerializedAs("_isSolid")]
        public bool IsSolid { get; private set; }

        [field: SerializeField, FormerlySerializedAs("_isAffectedByGravity")]
        public bool IsAffectedByGravity { get; private set; }
    }
}
