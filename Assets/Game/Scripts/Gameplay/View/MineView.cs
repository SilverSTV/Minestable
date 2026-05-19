using System.Collections.Generic;
using Game.Scripts.Configs;
using Game.Scripts.Editor;
using Game.Scripts.Gameplay;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Scripts.View
{
    public class MineView : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
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

            var bounds = new BoundsInt(0, 0, 0, mineGrid.Width, mineGrid.Height, 1);
            var tiles = new TileBase[mineGrid.Width * mineGrid.Height];
            var i = 0;

            for (var y = 0; y < mineGrid.Height; y++)
            {
                for (var x = 0; x < mineGrid.Width; x++)
                {
                    var position = new Vector2Int(x, y);
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
                            $"MineView: block tile is not configured for block type {cell.BlockType} at {position}.",
                            this);
                        tiles[i++] = null;
                        continue;
                    }

                    tiles[i++] = tile;
                }
            }

            _tilemap.SetTilesBlock(bounds, tiles);
        }

        public void UpdateCell(int x, int y, CellState cell)
        {
            if (!TryResolveReferences())
            {
                return;
            }

            var pos = new Vector3Int(x, y, 0);

            if (cell.BlockType == BlockType.Air || cell.BlockType == BlockType.Unknown)
            {
                _tilemap.SetTile(pos, null);
                return;
            }

            var tile = _tileRegistry.Get(cell.BlockType);
            _tilemap.SetTile(pos, tile);
        }
        
        public void ApplyChanges(List<CellChange> changes)
        {
            foreach (var change in changes)
            {
                var pos = new Vector3Int(change.x, change.y, 0);

                if (change.state.BlockType == BlockType.Air)
                {
                    _tilemap.SetTile(pos, null);
                }
                else
                {
                    var tile = _tileRegistry.Get(change.state.BlockType);
                    _tilemap.SetTile(pos, tile);
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
    }
}
