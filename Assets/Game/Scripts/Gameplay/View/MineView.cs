using System.Collections.Generic;
using Game.Scripts.Configs;
using Game.Scripts.Editor;
using Game.Scripts.Gameplay.Mine.Core;
using Game.Scripts.Gameplay.Mine.State;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Scripts.Gameplay.View
{
    public class MineView : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private Tilemap _damageTilemap;
        [SerializeField] private TileRegistry _tileRegistry;
        

        public Tilemap Tilemap
        {
            get
            {
                TryResolveReferences();
                return _tilemap;
            }
        }

        public void RedrawAllAsync(MineGrid mineGrid, BlockDatabase blockDatabase)
        {
            if (mineGrid == null)
            {
                Debug.LogError("MineView.RenderAsync requires a MineGrid.", this);
                return;
            }

            if (blockDatabase == null)
            {
                Debug.LogError("MineView.RenderAsync requires a BlockDatabase.", this);
                return;
            }

            if (!TryResolveReferences())
            {
                return;
            }

            var bounds = new BoundsInt(0, -mineGrid.Height +1, 0, mineGrid.Width, mineGrid.Height, 1);
            var tiles = new TileBase[mineGrid.Width * mineGrid.Height];
            var i = 0;

            for (var worldY = bounds.yMin; worldY < bounds.yMax; worldY++)
            {
                var y = WorldYToGridY(worldY);
                
                for (var x = 0; x < mineGrid.Width; x++)
                {
                    var cell = mineGrid.GetBlock(x, y);

                    if (cell.BlockType == BlockType.Air || cell.BlockType == BlockType.Unknown)
                    {
                        tiles[i++] = null;
                        continue;
                    }

                    var tile = _tileRegistry.Get(cell.BlockType);
                    if (tile == null)
                    {
                        Debug.LogWarning(
                            $"MineView: block tile is not configured for block type {cell.BlockType} at X:{x} & Y:{y}.",
                            this);
                        tiles[i++] = null;
                        continue;
                    }

                    tiles[i++] = tile;
                }
            }

            _tilemap.SetTilesBlock(bounds, tiles);
        }

        public void UpdateCell(int x, int gridY, CellState cell)
        {
            if (!TryResolveReferences())
            {
                return;
            }

            var y = WorldYToGridY(gridY);
            var pos = new Vector3Int(x, y, 0);

            if (cell.BlockType == BlockType.Air || cell.BlockType == BlockType.Unknown)
            {
                _tilemap.SetTile(pos, null);
                _damageTilemap.SetTile(pos,null);
                return;
            }

            var tile = _tileRegistry.Get(cell.BlockType);
            _tilemap.SetTile(pos, tile);
        }
        
        public void ApplyChanges(List<MineCellVisualUpdate> changes)
        {
            foreach (var change in changes)
            {
                var worldY = WorldYToGridY(change.Y);
                var pos = new Vector3Int(change.X, worldY, 0);

                if (change.CurrentBlockType == BlockType.Air)
                {
                    _tilemap.SetTile(pos, null);
                    _damageTilemap.SetTile(pos,null);
                }
                else
                {
                    var tile = _tileRegistry.Get(change.CurrentBlockType);
                    _tilemap.SetTile(pos, tile);
                    ApplyDamageToTilemap(pos,change.DamageState);
                }
            }
        }

        private bool TryResolveReferences()
        {
            if (_tilemap == null)
            {
                _tilemap = GetComponentInChildren<Tilemap>();
            }

            if (_tileRegistry == null)
            {
                Debug.LogError("MineView requires a TileRegistry reference.", this);
                return false;
            }

            if (_tilemap == null)
            {
                Debug.LogError("MineView requires a Tilemap reference.", this);
                return false;
            }

            return true;
        }
        
        private int WorldYToGridY(int worldY)
        {
            return -worldY;
        }

        private void ApplyDamageToTilemap(Vector3Int pos, BlockDamageStage blockDamageStage)
        {
            var tile = _tileRegistry.GetDamageTile(blockDamageStage);
                _damageTilemap.SetTile(pos, tile);
        }
    }
}
