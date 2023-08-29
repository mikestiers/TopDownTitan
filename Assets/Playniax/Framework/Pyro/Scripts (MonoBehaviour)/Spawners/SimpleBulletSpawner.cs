using UnityEngine;

namespace Playniax.Pyro
{
    public class SimpleBulletSpawner : BulletSpawnerBase
    {
        public GameObject prefab;
        public Transform parent;
        public Vector3 position;
        public float scale = 1;

        public override void UpdateSpawner()
        {
            if (prefab == null) return;

            if (BulletSpawnerHelper.count > 0 && timer.Update()) OnSpawn();
        }
        public override void IgnitionInit()
        {
            if (prefab && prefab.scene.rootCount > 0) prefab.SetActive(false);
        }

        public override void OnSpawn()
        {
            var clone = Instantiate(prefab, transform.position, transform.rotation);
            if (clone)
            {
                clone.transform.localScale *= scale;
                clone.transform.Translate(position, Space.Self);

                if (parent)
                {
                    clone.transform.parent = parent;
                }
                else
                {
                    clone.transform.parent = transform.parent;
                }

                //if (overrideCollisionSettings.useTheseSettings) _OverrideCollisionSettings(clone);

                //if (prefabSmartOverrides) _SmartOverrides(clone);

                clone.SetActive(true);
            }
        }
    }
}