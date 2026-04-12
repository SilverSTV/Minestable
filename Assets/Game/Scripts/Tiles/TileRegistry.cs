using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Gameplay;
using UnityEngine;

namespace Game.Scripts.Editor
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Tiles")]
    public class TileRegistry : ScriptableObject
    {
        [Serializable]
        public struct Entry
        {
            public BlockType Type;
            public BlockTile Tile;
        }

        [SerializeField] private List<Entry> _entries;

        private Dictionary<BlockType, BlockTile> _dict;

        public BlockTile Get(BlockType type)
        {
            if (_dict == null)
                _dict = _entries.ToDictionary(e => e.Type, e => e.Tile);

            return _dict[type];
        }
    }
}
