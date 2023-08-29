using UnityEngine;

namespace Playniax.Ignition
{
    public class SpriteHelpers
    {
        public static Sprite[] CreateTiles(Texture2D source, int size, Vector2 pivot, float pixelsPerUnit = 100)
        {
            int width = source.width / size;
            int height = source.height / size;

            var sprites = new Sprite[width * height];

            var i = 0;

            for (float w = 0; w < width; w++)
            {
                for (float h = 0; h < height; h++)
                {
                    sprites[i] = Sprite.Create(source, new Rect(w * size, h * size, size, size), pivot, pixelsPerUnit);

                    i += 1;
                }
            }

            return sprites;
        }

        public static void SetColor(GameObject gameObject, Color color)
        {
            var spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();

            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].color = color;
            }
        }
    }
}