using UnityEngine;
using Playniax.Ignition;

namespace Playniax.Pyro
{
    public class Spawner360 : BulletSpawnerBase
    {
        [System.Serializable]
        public class CollisionSettings
        {
            public bool useTheseSettings;
            public int structuralIntegrity = 1;
        }

        public GameObject prefab;
        public Transform parent;
        public Vector3 position;
        public float scale = 1;
        public int counter = 10;
        public float speed = 8;
        //public bool smartSpawn;
        public bool autoDestroy;

        public CollisionSettings overrideCollisionSettings;
        public bool prefabSmartOverrides = true;

        public override void UpdateSpawner()
        {
            if (prefab == null) return;

            if (timer.Update()) OnSpawn();
        }
        public override void IgnitionInit()
        {
            if (prefab && prefab.scene.rootCount > 0) prefab.SetActive(false);
        }
        public override void OnSpawn()
        {
            for (int i = 0; i < counter; i++)
            {
                var clone = Instantiate(prefab, transform.position, transform.rotation, parent);
                if (clone)
                {
                    clone.transform.localScale *= scale;
                    clone.transform.Translate(position, Space.Self);

                    if (overrideCollisionSettings.useTheseSettings) _OverrideCollisionSettings(clone);

                    if (prefabSmartOverrides) _SmartOverrides(clone);

                    var bulletBase = clone.GetComponent<BulletBase>();
                    if (bulletBase)
                    {
                        float angle = i * (360f / counter) * Mathf.Deg2Rad;

                        clone.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);

                        bulletBase.velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
                    }
                    else
                    {
                        var rb = clone.GetComponent<Rigidbody2D>();
                        if (rb)
                        {
                            float angle = i * (360f / counter) * Mathf.Deg2Rad;

                            clone.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);

                            rb.velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
                        }
                    }

                    clone.SetActive(true);
                }
            }

            if (autoDestroy && timer.counter == 0) Destroy(gameObject);
        }

        void _OverrideCollisionSettings(GameObject clone)
        {
            var scoreBase = clone.GetComponent<IScoreBase>();
            if (scoreBase != null)
            {
                scoreBase.structuralIntegrity = overrideCollisionSettings.structuralIntegrity;
            }
        }
        void _SmartOverrides(GameObject clone)
        {
            if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_spriteRenderer)
            {
                var orderInLayer = _spriteRenderer.sortingOrder;
                _spriteRenderer = clone.GetComponent<SpriteRenderer>();
                if (_spriteRenderer != null) _spriteRenderer.sortingOrder = orderInLayer + 1;
            }

            var scoreBase = clone.GetComponent<IScoreBase>();
            if (scoreBase != null) scoreBase.friend = gameObject;
        }

        SpriteRenderer _spriteRenderer;
    }
}