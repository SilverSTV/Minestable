using Game.Scripts.Gameplay;
using Game.Scripts.Gameplay.Mine.Core;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Scripts.Editor
{
    [CreateAssetMenu]
    public class BlockTile : Tile
    {
        public BlockType BlockType;
    }
}
