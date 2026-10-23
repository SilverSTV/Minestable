using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Gameplay;
using Game.Scripts.Gameplay.Mine.Core;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Scripts.Editor
{
    [CreateAssetMenu(menuName = "Scriptable Objects/TileRegistry")]
    public class TileRegistry : ScriptableObject
    {
        [Serializable]
        public struct Entry
        {
            public BlockType Type;
            public BlockTile Tile;
        }
        
        [Serializable]
        public struct DamageEntry
        {
            public BlockDamageStage Stage;
            public TileBase Tile;
        }

        [SerializeField] private List<Entry> _entries;
        [SerializeField] private List<DamageEntry> _damageEntries;
        
        private Dictionary<BlockDamageStage, TileBase> _damageDict;
        private Dictionary<BlockType, BlockTile> _dict;

        public BlockTile Get(BlockType type)
        {
            if (_dict == null)
                _dict = _entries.ToDictionary(e => e.Type, e => e.Tile);

            return _dict[type];
        }
        
        public TileBase GetDamageTile(BlockDamageStage stage)
        {
            if (_damageDict == null)
                _damageDict = _damageEntries.ToDictionary(e => e.Stage, e => e.Tile);

            return _damageDict.TryGetValue(stage, out var tile) ? tile : null;
        }
    }
}
