using Playniax.Ignition;

namespace Playniax.Pyro
{
    public class BulletSpawnerBase : IgnitionBehaviour
    {
        public bool automatically = true;
        public Timer timer = new Timer();
        public string id;
        public static BulletSpawnerBase GetSpawner(string id)
        {
            var allBulletSpawners = FindObjectsOfType<BulletSpawnerBase>();

            for (int i = 0; i < allBulletSpawners.Length; i++)
                if (allBulletSpawners[i].id == id) return allBulletSpawners[i];

            return null;
        }

        public virtual bool AllowSpawning() { return true; }
        public virtual void LateUpdate()
        {
            if (automatically == true) UpdateSpawner();
        }
        public virtual void OnSpawn() { }
        public virtual void UpdateSpawner() { }
    }
}
