using UnityEngine;
using Playniax.Pyro;

namespace Playniax.Sequencer
{
    public class AdvancedSpawnerBase : SequenceBase
    {

        [System.Serializable]
        public class CargoSettings : CollisionState.CargoSettings
        {
            public enum ReleaseMode { Half, All };

            public ReleaseMode releaseMode;
        }

        [System.Serializable]
        public class ChildSettings
        {
            public GameObject prefab;
            public Vector3 position;
            public float scale = 1;
            public bool random;

            public CollisionSettings overrideCollisionSettings = new CollisionSettings();
        }

        [System.Serializable]
        public class CollisionSettings
        {
            public bool useTheseSettings;
            public int structuralIntegrity = 1;
        }

        [System.Serializable]
        public class AISettings : EnemyAI.CruiserSettings
        {
            public bool enabled = true;
        }

        [System.Serializable]
        public class SurpriseSettings
        {
            public GameObject prefab;
            public float scale = 1;

            public CollisionState.CargoSettings.EffectSettings effectSettings = new CollisionState.CargoSettings.EffectSettings();
        }

        /*
        [System.Serializable]
        public class PrefabSettings
        {
            public GameObject prefab;
            public float scale = 1;

            public CollisionSettings collisionSettings = new CollisionSettings();
        }

        public PrefabSettings[] prefabSettings = new PrefabSettings[1];
        */

        [Tooltip("Prefabs to use.")]
        public GameObject[] prefabs;

        public virtual void SetBullet(GameObject prefab, bool useTheseSettings) { }
        public virtual void SetCoin(GameObject prefab) { }
        public virtual void SetPickup(GameObject prefab) { }
    }
}
