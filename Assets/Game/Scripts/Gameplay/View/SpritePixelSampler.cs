using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Gameplay.View
{
    public static class SpritePixelSampler
    {
        public static List<Color32> CollectOpaquePixels(Sprite sprite, byte minAlpha = 32)
        {
            var result = new List<Color32>();
            if (sprite == null)
            {
                return result;
            }

            var texture = sprite.texture;
            var rect = sprite.textureRect;

            var startX = Mathf.FloorToInt(rect.x);
            var startY = Mathf.FloorToInt(rect.y);
            var width = Mathf.FloorToInt(rect.width);
            var height = Mathf.FloorToInt(rect.height);

            var pixels = texture.GetPixels32();

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var index = (startY + y) * texture.width + (startX + x);
                    var color = pixels[index];

                    if (color.a >= minAlpha)
                    {
                        result.Add(color);
                    }
                }
            }

            return result;
        }
        
        public static List<Color32> CollectOpaquePixelsFromAtlasSprite(Sprite sprite, byte minAlpha = 32)
        {
            var result = new List<Color32>();

            if (sprite == null)
            {
                return result;
            }

            var texture = sprite.texture;
            if (texture == null)
            {
                return result;
            }

            // Для чтения пикселей у текстуры должен быть включён Read/Write.
            var rect = sprite.textureRect;

            var startX = Mathf.RoundToInt(rect.x);
            var startY = Mathf.RoundToInt(rect.y);
            var width = Mathf.RoundToInt(rect.width);
            var height = Mathf.RoundToInt(rect.height);

            var atlasPixels = texture.GetPixels32();
            var atlasWidth = texture.width;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var atlasIndex = (startY + y) * atlasWidth + (startX + x);
                    var color = atlasPixels[atlasIndex];

                    if (color.a < minAlpha)
                    {
                        continue;
                    }

                    result.Add(color);
                }
            }

            return result;
        }

        public static Color32 GetRandomOpaquePixelFromAtlasSprite(Sprite sprite, byte minAlpha = 32)
        {
            var pixels = CollectOpaquePixelsFromAtlasSprite(sprite, minAlpha);

            if (pixels.Count == 0)
            {
                return new Color32(255, 255, 255, 255);
            }

            return pixels[Random.Range(0, pixels.Count)];
        }
    }
}
