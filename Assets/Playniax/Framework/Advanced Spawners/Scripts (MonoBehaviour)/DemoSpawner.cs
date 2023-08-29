using UnityEngine;
using Playniax.Ignition;
using Playniax.Pyro;

namespace Playniax.Sequencer
{
    public class DemoSpawner : AdvancedSpawnerBase
    {
        public enum StartPosition { Left, Right, Top, Bottom, LeftOrRight, TopOrBottom, Random, Fixed };

        [Tooltip("Startposition.")]
        public StartPosition startPosition = StartPosition.Random;
        public float margin;
        [Tooltip("Number of objects to spawn.")]
        public int counter = 1;
        [Tooltip("Timer.")]
        public float timer;
        [Tooltip("Interval.")]
        public float interval = 1;
        public float intervalRange;
        public OffCamera.Mode offCameraMode = OffCamera.Mode.Loop;
        public BulletSpawner.Settings bulletSettings = new BulletSpawner.Settings();
        public CargoSettings cargoSettings = new CargoSettings();
        public SurpriseSettings surpriseSettings = new SurpriseSettings();
        public ChildSettings childSettings = new ChildSettings();
        public AISettings simpleAISettings = new AISettings();
        public bool bodyCount;
        public override void IgnitionInit()
        {
            for (int i = 0; i < prefabs.Length; i++)
                if (prefabs[i] && prefabs[i].scene.rootCount > 0) prefabs[i].SetActive(false);
        }
        public override void OnSequencerAwake()
        {
            ProgressCounter.Add(counter * prefabs.Length);
        }
        public override void OnSequencerUpdate()
        {
            if (timer <= 0)
            {
                _Spawn();

                timer = Random.Range(interval, interval + intervalRange);

                counter -= 1;

                if (counter <= 0) enabled = false;
            }
            else
            {
                timer -= 1 * Time.deltaTime;
            }

            void _Spawn()
            {
                OnSpawn();
            }
        }
        public virtual GameObject OnSpawn()
        {
            var position = Vector3.zero;
            var rotation = 0f;

            var index = Random.Range(0, prefabs.Length);
            var prefab = prefabs[index];
            var scale = prefab.transform.localScale;

            GetPosition(prefab, ref position, ref rotation, ref scale);

            var clone = Instantiate(prefab, position, Quaternion.Euler(0, 0, rotation), null);

            clone.transform.localScale = scale;

            clone.AddComponent<Register>();
            clone.AddComponent<ProgressCounter>();

            var collisionState = clone.GetComponent<CollisionState>();

            if (bodyCount && collisionState)
            {
                collisionState.bodyCount = true;

                GameData.spawned += 1;
            }

            var scoreBase = clone.GetComponent<IScoreBase>();
            if (scoreBase != null) scoreBase.structuralIntegrity *= AdvancedSpawnerSettings.GetStructuralIntegrityMultiplier();

            if (simpleAISettings.enabled)
            {
                var enemyAI = clone.GetComponent<EnemyAI>();
                if (enemyAI == null) enemyAI = clone.AddComponent<EnemyAI>();

                enemyAI.cruiserSettings = JsonUtility.FromJson<EnemyAI.CruiserSettings>(JsonUtility.ToJson(simpleAISettings));

                enemyAI.mode = EnemyAI.Mode.Cruiser;
            }

            if (offCameraMode != OffCamera.Mode.None)
            {
                var offCamera = clone.GetComponent<OffCamera>();
                if (offCamera == null) offCamera = clone.AddComponent<OffCamera>();

                offCamera.mode = offCameraMode;
            }

            if (bulletSettings.useTheseSettings && bulletSettings.prefab)
            {
                var bulletSpawner = clone.GetComponent<BulletSpawner>();
                if (bulletSpawner == null) bulletSpawner = clone.AddComponent<BulletSpawner>();

                bulletSpawner.mode = BulletSpawner.Mode.TargetPlayer;

                bulletSpawner.Set(bulletSettings);
            }

            if (collisionState && surpriseSettings.prefab != null && counter == 1)
            {
                collisionState.cargoSettings.prefab = new GameObject[1] { surpriseSettings.prefab };
                collisionState.cargoSettings.scale = surpriseSettings.scale;

                collisionState.cargoSettings.effectSettings = JsonUtility.FromJson<CollisionState.CargoSettings.EffectSettings>(JsonUtility.ToJson(surpriseSettings.effectSettings));
            }
            else if (collisionState && (_cargo || cargoSettings.releaseMode == CargoSettings.ReleaseMode.All))
            {
                collisionState.cargoSettings = JsonUtility.FromJson<CollisionState.CargoSettings>(JsonUtility.ToJson(cargoSettings));
            }
            else
            {
                _cargo = !_cargo;
            }

            if (childSettings.prefab && childSettings.random == true && Random.Range(0, 2) == 1 || childSettings.prefab && childSettings.random == false)
            {
                var child = Instantiate(childSettings.prefab, clone.transform.position, Quaternion.identity, clone.transform);
                child.transform.localPosition = childSettings.position;
                child.transform.localScale *= childSettings.scale;

                scoreBase = child.GetComponent<IScoreBase>();

                if (scoreBase != null)
                {
                    if (childSettings.overrideCollisionSettings.useTheseSettings)
                    {
                        scoreBase.structuralIntegrity = childSettings.overrideCollisionSettings.structuralIntegrity;
                    }

                    scoreBase.structuralIntegrity *= AdvancedSpawnerSettings.GetStructuralIntegrityMultiplierForChild();
                }
            }

            clone.SetActive(true);

            return clone;
        }
        public void GetPosition(GameObject obj, ref Vector3 position, ref float rotation, ref Vector3 scale)
        {
            if (startPosition == StartPosition.Left)
            {
                _GetPosition(obj, 0, ref position, ref rotation, ref scale);
            }
            else if (startPosition == StartPosition.Right)
            {
                _GetPosition(obj, 1, ref position, ref rotation, ref scale);
            }
            else if (startPosition == StartPosition.Top)
            {
                _GetPosition(obj, 2, ref position, ref rotation, ref scale);
            }
            else if (startPosition == StartPosition.Bottom)
            {
                _GetPosition(obj, 3, ref position, ref rotation, ref scale);
            }
            else if (startPosition == StartPosition.LeftOrRight)
            {
                _GetPosition(obj, Random.Range(0, 2), ref position, ref rotation, ref scale);
            }
            else if (startPosition == StartPosition.TopOrBottom)
            {
                _GetPosition(obj, Random.Range(2, 4), ref position, ref rotation, ref scale);
            }
            else if (startPosition == StartPosition.Random)
            {
                _GetPosition(obj, Random.Range(0, 4), ref position, ref rotation, ref scale);
            }
            else
            {
                position = transform.position;
                rotation = Random.Range(0, 360);
            }
        }
        public override void SetBullet(GameObject prefab, bool useTheseSettings)
        {
            bulletSettings.prefab = prefab;

            bulletSettings.useTheseSettings = useTheseSettings;
        }
        public override void SetCoin(GameObject prefab)
        {
            cargoSettings.Add(prefab);

            cargoSettings.effectSettings.motion = CollisionState.CargoSettings.EffectSettings.Motion.Down;
        }

        public override void SetPickup(GameObject prefab)
        {
            surpriseSettings.prefab = prefab;

            surpriseSettings.effectSettings.motion = CollisionState.CargoSettings.EffectSettings.Motion.Down;

            surpriseSettings.scale = .5f;
        }
        void _GetPosition(GameObject obj, int segment, ref Vector3 position, ref float rotation, ref Vector3 scale)
        {
            // Segment:

            // 0 = left
            // 1 = right
            // 2 = top
            // 3 = bottom

            var size = RendererHelpers.GetSize(obj);

            var min = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, transform.position.z - Camera.main.transform.position.z));
            var max = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, transform.position.z - Camera.main.transform.position.z));

            min.x -= size.x * .5f;
            max.x += size.x * .5f;
            min.y += size.y * .5f;
            max.y -= size.y * .5f;

            if (segment == 0)
            {
                position.x = min.x;
                position.y = Random.Range(min.y - size.y - margin, max.y + size.y + margin);
            }
            else if (segment == 1)
            {
                position.x = max.x;
                position.y = Random.Range(min.y - size.y - margin, max.y + size.y + margin);

                rotation = 180;
                scale.y *= -1;
            }
            else if (segment == 2)
            {
                position.x = Random.Range(min.x + size.x + margin, max.x - size.x - margin);
                position.y = min.y;

                rotation = -90;
            }
            else if (segment == 3)
            {
                position.x = Random.Range(min.x + size.x + margin, max.x - size.x - margin);
                position.y = max.y;

                rotation = 90;
            }
        }

        bool _cargo;
    }
}