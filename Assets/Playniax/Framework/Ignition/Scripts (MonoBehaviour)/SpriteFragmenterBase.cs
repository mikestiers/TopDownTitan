using UnityEngine;

public class SpriteFragmenterBase : MonoBehaviour
{
    public int size = 32;

    public bool removeRedundantSprites = true;

    public bool update;

    public SpriteRenderer spriteRenderer;

    public virtual Collider2D AddCollider(SpriteRenderer renderer) 
    {
        return renderer.gameObject.AddComponent<BoxCollider2D>();
    }
    public virtual void OnFragment(SpriteRenderer renderer) { }

    public virtual void OnFragmented() { }

    void Start()
    {
        _Fragment();

        OnFragmented();
    }

    void Update()
    {
        if (update) _Update();
    }

    void _Fragment()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null) return;

        spriteRenderer.enabled = false;

        var collider = GetComponent<Collider2D>();

        int width = spriteRenderer.sprite.texture.width / size;
        int height = spriteRenderer.sprite.texture.height / size;

        var x = -(spriteRenderer.sprite.texture.width - size) / 2 / spriteRenderer.sprite.pixelsPerUnit;
        var y = -(spriteRenderer.sprite.texture.height - size) / 2 / spriteRenderer.sprite.pixelsPerUnit;

        for (float w = 0; w < width; w++)
        {
            for (float h = 0; h < height; h++)
            {
                var renderer = new GameObject(w * size + "," + h * size + "," + size + "," + size).AddComponent<SpriteRenderer>();
                renderer.transform.parent = transform;
                renderer.transform.localScale = Vector3.one;
                renderer.transform.localPosition = new Vector3(x + w * size / spriteRenderer.sprite.pixelsPerUnit, y + h * size / spriteRenderer.sprite.pixelsPerUnit);
                renderer.sprite = Sprite.Create(spriteRenderer.sprite.texture, new Rect(w * size, h * size, size, size), new Vector2(0.5f, 0.5f), spriteRenderer.sprite.pixelsPerUnit);
                renderer.color = spriteRenderer.color;

                if (collider)
                {
                    var boxCollider = AddCollider(renderer);

                    if (collider.Distance(boxCollider).isOverlapped == false)
                    {
                        if (removeRedundantSprites) Destroy(boxCollider.gameObject); else Destroy(boxCollider);
                    }
                    else
                    {
                        OnFragment(renderer);
                    }
                }
            }
        }

        if (collider) Destroy(collider);
    }

    void _Update()
    {
        var renderers = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            var rectangle = renderers[i].name.Split(",");

            if (rectangle.Length != 4) continue;

            var x = int.Parse(rectangle[0]);
            var y = int.Parse(rectangle[1]);
            var w = int.Parse(rectangle[2]);
            var h = int.Parse(rectangle[3]);

            renderers[i].sprite = Sprite.Create(spriteRenderer.sprite.texture, new Rect(x, y, w, h), new Vector2(0.5f, 0.5f), spriteRenderer.sprite.pixelsPerUnit);
        }
    }
}