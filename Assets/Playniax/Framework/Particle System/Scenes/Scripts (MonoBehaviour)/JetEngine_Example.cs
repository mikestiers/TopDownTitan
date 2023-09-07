using Playniax.ParticleSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetEngine_Example : MonoBehaviour
{
    public Sprite sprite;
    public Material material;
    public int orderInLayer;
    public float timer;
    public float interval = 1;

    void Update()
    {
        timer -= 1;

        if (timer <= 0)
        {
            timer = interval;

            var renderer = new GameObject("Particle").AddComponent<SpriteRenderer>();

            renderer.sprite = sprite;

            if (material) renderer.material = material;

            renderer.sortingOrder = orderInLayer;

            renderer.transform.parent = transform;

            renderer.transform.position = transform.position;

            var particle = renderer.gameObject.AddComponent<Particle>();

            var r = Random.Range(.75f, 1);
            var g = Random.Range(.5f, .75f);
            var b = Random.Range(.25f, .5f);

            particle.startColor = new Color(r, g, b, 1);

            particle.targetColor = new Color(1, 1, 1, 0);

            particle.ttl = Random.Range(1.5f, 1.25f);

            particle.targetScale = Vector3.zero;

            var scale = Random.Range(0, .25f);
            particle.startScale *= scale;

            particle.velocity = Random.Range(.75f, 1.25f) * Vector3.left;

            particle.name += " (" + particle.gameObject.GetInstanceID() + ")";
        }
    }
}
