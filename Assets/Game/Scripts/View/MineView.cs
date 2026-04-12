using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Scripts.Configs;
using Game.Scripts.Editor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Scripts.Gameplay
{
    public class MineView : MonoBehaviour
    {
        // [SerializeField] private float _cellSize = 1f;
        // [SerializeField] private int _sortingOrder;
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileRegistry _tileRegistry;

        private TileSpriteProvider _tileSpriteProvider;

        private void Awake()
        {
            _tileSpriteProvider = new TileSpriteProvider();
        }

        private void OnDestroy()
        {
            _tileSpriteProvider?.Dispose();
        }

        public void RedrawAllAsync(MineGrid mineGrid, BlockDatabase blockDatabase)
        {
            if (mineGrid == null)
            {
                Debug.LogError("MineView.RenderAsync requires a MineGrid.", this);
                return;
            }

            var bounds = new BoundsInt(0, 0, 0, mineGrid.Width, mineGrid.Height, 1);
            var tiles = new TileBase[mineGrid.Width * mineGrid.Height];

            if (blockDatabase == null)
            {
                Debug.LogError("MineView.RenderAsync requires a BlockDatabase.", this);
                return;
            }

            // _tileSpriteProvider ??= new TileSpriteProvider();
            // await _tileSpriteProvider.Preload(blockDatabase.Settings);

            int i = 0;

            for (int y = 0; y < mineGrid.Height; y++)
            {
                for (int x = 0; x < mineGrid.Width; x++)
                {
                    var position = new Vector2Int(x, y);
                    var cell = mineGrid.GetBlock(x, y);

                    if (cell.BlockType == BlockType.Air || cell.BlockType == BlockType.Unknown)
                    {
                        ClearCell(x, y);
                        continue;
                    }

                    var tile = _tileRegistry.Get(cell.BlockType);

                    if (tile == null)
                    {
                        Debug.LogWarning(
                            $"MineView: block tile is not configured for block type {cell.BlockType} at {position}.",
                            this);
                        ClearCell(x,y);
                        continue;
                    }

                    tiles[i++] = _tileRegistry.Get(cell.BlockType);
                }
            }
            
            _tilemap.SetTilesBlock(bounds,tiles);
        }

        private void UpdateCell(int x, int y, CellState cell)
        {
            var pos = new Vector3Int(x, y, 0);

            if (cell.BlockType == BlockType.Air || cell.BlockType == BlockType.Unknown)
            {
                _tilemap.SetTile(pos, null);
                return;
            }

            var tile = _tileRegistry.Get(cell.BlockType);
            _tilemap.SetTile(pos, tile);
        }

        private void ClearCell(int x, int y)
        {
            var cell = new CellState {BlockType = BlockType.Air};
            UpdateCell(x,y,cell);
        }
    }
}
