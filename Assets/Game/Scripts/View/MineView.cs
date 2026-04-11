using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Scripts.Configs;
using UnityEngine;

namespace Game.Scripts.Gameplay
{
    public class MineView : MonoBehaviour
    {
        [SerializeField] private float _cellSize = 1f;
        [SerializeField] private int _sortingOrder;

        private readonly Dictionary<Vector2Int, SpriteRenderer> _renderers = new();
        private TileSpriteProvider _tileSpriteProvider;
        private int _renderVersion;

        private void Awake()
        {
            _tileSpriteProvider = new TileSpriteProvider();
        }

        private void OnDestroy()
        {
            _tileSpriteProvider?.Dispose();
        }

        public async Task RenderAsync(MineGrid mineGrid, BlockDatabase blockDatabase)
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

            _tileSpriteProvider ??= new TileSpriteProvider();
            int renderVersion = ++_renderVersion;

            for (int y = 0; y < mineGrid.Height; y++)
            {
                for (int x = 0; x < mineGrid.Width; x++)
                {
                    var position = new Vector2Int(x, y);
                    var cell = mineGrid.GetBlock(x, y);

                    if (cell.BlockType == BlockType.Air || cell.BlockType == BlockType.Unknown)
                    {
                        ClearCell(position);
                        continue;
                    }

                    var blockSettings = blockDatabase.GetSettings(cell.BlockType);
                    var spriteReference = blockSettings.SpriteReference;

                    if (spriteReference == null || !spriteReference.RuntimeKeyIsValid())
                    {
                        Debug.LogWarning(
                            $"MineView: sprite reference is not configured for block type {cell.BlockType} at {position}.",
                            this);
                        ClearCell(position);
                        continue;
                    }

                    var sprite = await _tileSpriteProvider.GetSpriteAsync(spriteReference);
                    if (renderVersion != _renderVersion)
                    {
                        return;
                    }

                    if (sprite == null)
                    {
                        Debug.LogWarning(
                            $"MineView: Addressables sprite was not found for block type {cell.BlockType} at {position}.",
                            this);
                        ClearCell(position);
                        continue;
                    }

                    var renderer = GetOrCreateRenderer(position);
                    renderer.sprite = sprite;
                    renderer.transform.localPosition = GridToLocalPosition(x, y, mineGrid);
                    renderer.transform.localScale = GetSpriteScale(sprite);
                    renderer.sortingOrder = _sortingOrder;
                    renderer.enabled = true;
                }
            }
        }

        private SpriteRenderer GetOrCreateRenderer(Vector2Int position)
        {
            if (_renderers.TryGetValue(position, out var renderer) && renderer != null)
            {
                return renderer;
            }

            var cellObject = new GameObject($"Cell_{position.x}_{position.y}");
            cellObject.transform.SetParent(transform, false);

            renderer = cellObject.AddComponent<SpriteRenderer>();
            renderer.sortingOrder = _sortingOrder;

            _renderers[position] = renderer;
            return renderer;
        }

        private void ClearCell(Vector2Int position)
        {
            if (!_renderers.TryGetValue(position, out var renderer) || renderer == null)
            {
                return;
            }

            Destroy(renderer.gameObject);
            _renderers.Remove(position);
        }

        private Vector3 GridToLocalPosition(int x, int y, MineGrid mineGrid)
        {
            float centeredX = (x - (mineGrid.Width - 1) * 0.5f) * _cellSize;
            float centeredY = (y - (mineGrid.Height - 1) * 0.5f) * _cellSize;
            return new Vector3(centeredX, centeredY, 0f);
        }

        private Vector3 GetSpriteScale(Sprite sprite)
        {
            var size = sprite.bounds.size;
            if (size.x <= 0f || size.y <= 0f)
            {
                return Vector3.one;
            }

            return new Vector3(_cellSize / size.x, _cellSize / size.y, 1f);
        }
    }
}
