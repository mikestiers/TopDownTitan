#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace Playniax.InfiniteScroller
{
    [AddComponentMenu("Playniax/Prototyping/Infinite Scroller/Tile Layer")]
    public class TileLayer : LayerBase
    {
        public Scroller scroller;
        [Tooltip("Sprite to tile.")]
        public Sprite sprite;
        public int orderInLayer = 0;
        public float z;
        [Tooltip("Size of the tilemap.")]
        public Vector2 count = new Vector2(10, 10);
        public bool seamless;
        [Tooltip("Sprite material.")]
        public Material material;

#if UNITY_EDITOR
        void Reset()
        {
            scroller = FindObjectOfType<Scroller>();

            sprite = AssetDatabase.LoadAssetAtPath("Assets/Playniax/Essentials/Textures/Retro/Gravity Zone/space tile.png", typeof(Sprite)) as Sprite;
        }
#endif
        public override void Scroll(float x, float y)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] == null) continue;

                var size = scroller.size * .5f * _GetSpeed() / speed;

                var position = objects[i].transform.position;

                position.x -= x * speed;
                position.y -= y * speed;

                if (position.x > size.x) position.x -= size.x * 2;
                if (position.x < -size.x) position.x += size.x * 2;

                if (position.y > size.y) position.y -= size.y * 2;
                if (position.y < -size.y) position.y += size.y * 2;

                objects[i].transform.position = position;
            }
        }

        float _GetSpeed()
        {
            var layers = scroller.GetLayers();

            var speed = layers[0].speed;

            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i].speed > speed) speed = layers[i].speed;
            }

            return speed;
        }

        void Awake()
        {
            if (scroller == null) scroller = FindObjectOfType<Scroller>();
        }
        void Start()
        {
            var size = sprite.rect.size / sprite.pixelsPerUnit;

            var worldSize = scroller.size * .5f;

            var scale = new Vector2(worldSize.x / size.x / count.x, worldSize.y / size.y / count.y);

            scale *= _GetSpeed() / speed;

            var flip = new Vector2(1, 1);

            for (float x = -count.x; x < count.x; x++)
            {
                for (float y = -count.y; y < count.y; y++)
                {
                    var tile = new GameObject(sprite.name);

                    var spriteRenderer = tile.AddComponent<SpriteRenderer>();

                    if (material) spriteRenderer.material = material;

                    spriteRenderer.sprite = sprite;
                    spriteRenderer.sortingOrder = orderInLayer;

                    tile.transform.position = new Vector3((size.x * .5f * scale.x) + x * size.x * scale.x, (size.y * .5f * scale.y) + y * size.y * scale.y, z);
                    tile.transform.localScale = scale * flip;

                    tile.transform.SetParent(transform);

                    objects.Add(tile);

                    if (seamless) flip.y = -flip.y;
                }
                if (seamless) flip.x = -flip.x;
            }
        }
    }
}