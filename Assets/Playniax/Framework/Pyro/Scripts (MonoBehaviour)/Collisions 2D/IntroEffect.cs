using UnityEngine;

namespace Playniax.Pyro
{
    public class IntroEffect : MonoBehaviour
    {
        [System.Serializable]
        public class FlickerSettings
        {
            public int count = 15;
            public float sustain = 1f;
            public SpriteRenderer[] spriteRenderer = new SpriteRenderer[0];
            public CollisionBase2D[] collisionBase = new CollisionBase2D[0];
            public float counter { get; set; }
            public float timer { get; set; }
        }

        [System.Serializable]
        public class ScaleSettings
        {
            public float speed = 5;

            public Vector3 scale { get; set; }
        }

        public enum Mode { Flicker, Scale };

        public Mode mode;
        public FlickerSettings flickerSettings = new FlickerSettings();
        public ScaleSettings scaleSettings = new ScaleSettings();
        void Start()
        {
            if (mode == Mode.Flicker)
            {
                if (CheckRenderer()) flickerSettings.spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
                if (CheckCollisionBase()) flickerSettings.collisionBase = GetComponentsInChildren<CollisionBase2D>();

                for (int i = 0; i < flickerSettings.collisionBase.Length; i++)
                    if (flickerSettings.collisionBase[i]) flickerSettings.collisionBase[i].isSuspended = true;
            }
            else if (mode == Mode.Scale)
            {
                scaleSettings.scale = transform.localScale;

                transform.localScale = Vector3.zero;
            }

            bool CheckRenderer()
            {
                if (flickerSettings.spriteRenderer.Length == 0) return true;

                for (int i = 0; i < flickerSettings.spriteRenderer.Length; i++)
                    if (flickerSettings.spriteRenderer[i] == null) return true;

                return false;
            }

            bool CheckCollisionBase()
            {
                if (flickerSettings.collisionBase.Length == 0) return true;

                for (int i = 0; i < flickerSettings.collisionBase.Length; i++)
                    if (flickerSettings.collisionBase[i] == null) return true;

                return false;
            }
        }
        void Update()
        {
            if (mode == Mode.Flicker)
            {
                if (flickerSettings.collisionBase[0] && flickerSettings.collisionBase[0].isSuspended == false) return;

                if (flickerSettings.counter == flickerSettings.count)
                {
                    for (int i = 0; i < flickerSettings.collisionBase.Length; i++)
                        if (flickerSettings.collisionBase[i]) flickerSettings.collisionBase[i].isSuspended = false;
                }

                flickerSettings.timer += Time.deltaTime;

                if (flickerSettings.timer > (flickerSettings.sustain / 10))
                {
                    for (int i = 0; i < flickerSettings.spriteRenderer.Length; i++)
                        if (flickerSettings.spriteRenderer[i]) flickerSettings.spriteRenderer[i].enabled = !flickerSettings.spriteRenderer[i].enabled;

                    flickerSettings.counter += .5f;
                    flickerSettings.timer = 0;
                }
            }
            else if (mode == Mode.Scale)
            {
                transform.localScale += scaleSettings.speed * Vector3.one * Time.deltaTime;

                if (transform.localScale.x >= scaleSettings.scale.x)
                {
                    transform.localScale = scaleSettings.scale;

                    enabled = false;
                }
            }
        }
    }
}
