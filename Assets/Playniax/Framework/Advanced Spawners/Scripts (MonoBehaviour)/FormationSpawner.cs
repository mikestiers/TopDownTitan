using UnityEditor;
using UnityEngine;
using Playniax.Ignition;
using Playniax.Pyro;

namespace Playniax.Sequencer
{
    public class FormationSpawner : AdvancedSpawnerBase
    {
        public class Motion : MonoBehaviour
        {
            public Vector3 velocity;
            public float friction = .99f;
            public MonoBehaviour[] components;

            public void AddComponent(MonoBehaviour component)
            {
                if (components == null) components = new MonoBehaviour[0];

                components = ArrayHelpers.Add(components, component);
            }
            void Update()
            {
                transform.position += velocity * Time.deltaTime;

                if (friction != 0) velocity *= 1 / (1 + (Time.deltaTime * friction));

                if (components != null && velocity.magnitude < 1)
                {
                    for (int i = 0; i < components.Length; i++)
                        components[i].enabled = true;

                    components = null;
                }
            }
        }

        public enum Position { Left, Right, Top, Bottom };

        [Tooltip("Entrance.")]
        public Position[] entrance = new Position[1];
        public float speed = 3;
        public float friction = .99f;
        [Tooltip("Number of objects to spawn.")]
        public int counter = 1;
        [Tooltip("Space between objects.")]
        public float space;
        [Tooltip("offset from center start.")]
        public float offset;
        [Tooltip("Timer.")]
        public float timer;
        [Tooltip("Interval.")]
        public float interval;
        public float scale = 1;
        public float z;
        public bool faceDirection;
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
 
            if (surpriseSettings.prefab && surpriseSettings.prefab.scene.rootCount > 0) surpriseSettings.prefab.SetActive(false);

            cargoSettings.Init();
        }

        public override void OnSequencerAwake()
        {
            ProgressCounter.Add(counter * entrance.Length);

            _start = -space * counter * .5f + (space * .5f);
        }

        public override void OnSequencerUpdate()
        {
            if (timer > 0)
            {
                timer -= 1 * Time.deltaTime;
            }
            else
            {
                _Spawn();

                _cargo = !_cargo;

                timer = interval;

                counter -= 1;

                if (counter <= 0) enabled = false;
            }

            void _Spawn()
            {
                OnSpawn();
            }
        }

        public virtual void OnSpawn()
        {
            for (int i = 0; i < entrance.Length; i++)
            {
                var prefab = Random.Range(0, prefabs.Length);

                var position = _GetPosition(prefabs[prefab], entrance[i]);

                if (entrance[i] == Position.Left || entrance[i] == Position.Right) position.y = _start + offset;
                if (entrance[i] == Position.Top || entrance[i] == Position.Bottom) position.x = _start + offset;

                var rotation = 0;
                var localScale = prefabs[prefab].transform.localScale;

                if (faceDirection)
                {
                    if (entrance[i] == Position.Right)
                    {
                        rotation = 180;
                        localScale.y *= -1;
                    }
                    else if (entrance[i] == Position.Top)
                        rotation = -90;
                    else if (entrance[i] == Position.Bottom)
                        rotation = 90;
                }

                var clone = Instantiate(prefabs[prefab], position, Quaternion.Euler(0, 0, rotation));

                clone.gameObject.SetActive(true);

                clone.transform.localScale = localScale * scale;

                clone.gameObject.AddComponent<Register>();
                clone.gameObject.AddComponent<ProgressCounter>();

                var collisionState = clone.GetComponent<CollisionState>();

                if (bodyCount && collisionState)
                {
                    collisionState.bodyCount = true;

                    GameData.spawned += 1;
                }

                var scoreBase = clone.GetComponent<IScoreBase>();
                if (scoreBase != null) scoreBase.structuralIntegrity *= AdvancedSpawnerSettings.GetStructuralIntegrityMultiplier();

                if (bulletSettings.useTheseSettings && bulletSettings.prefab)
                {
                    var bulletSpawner = clone.GetComponent<BulletSpawner>();
                    if (bulletSpawner == null) bulletSpawner = clone.AddComponent<BulletSpawner>();

                    bulletSpawner.mode = BulletSpawner.Mode.TargetPlayer;

                    bulletSpawner.Set(bulletSettings);
                }

                if (collisionState && surpriseSettings.prefab != null && counter == 1 && i == 0)
                {
                        collisionState.cargoSettings.prefab = new GameObject[1] { surpriseSettings.prefab };
                        collisionState.cargoSettings.scale = surpriseSettings.scale;

                        collisionState.cargoSettings.effectSettings = JsonUtility.FromJson<CollisionState.CargoSettings.EffectSettings>(JsonUtility.ToJson(surpriseSettings.effectSettings));
                }
                else if (collisionState && (_cargo || cargoSettings.releaseMode == CargoSettings.ReleaseMode.All))
                {
                    collisionState.cargoSettings = JsonUtility.FromJson<CollisionState.CargoSettings>(JsonUtility.ToJson(cargoSettings));
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

                var motion = clone.gameObject.AddComponent<Motion>();


                if (simpleAISettings.enabled)
                {
                    var enemyAI = clone.GetComponent<EnemyAI>();
                    if (enemyAI == null) enemyAI = clone.AddComponent<EnemyAI>();

                    enemyAI.enabled = false;

                    enemyAI.cruiserSettings = JsonUtility.FromJson<EnemyAI.CruiserSettings>(JsonUtility.ToJson(simpleAISettings));

                    enemyAI.mode = EnemyAI.Mode.Cruiser;

                    motion.AddComponent(enemyAI);
                }

                if (offCameraMode != OffCamera.Mode.None)
                {
                    var offCamera = clone.GetComponent<OffCamera>();
                    if (offCamera == null) offCamera = clone.AddComponent<OffCamera>();

                    offCamera.enabled = false;

                    offCamera.mode = offCameraMode;

                    motion.AddComponent(offCamera);
                }

                if (entrance[i] == Position.Left)
                {
                    motion.velocity.x = speed;
                }
                else if (entrance[i] == Position.Right)
                {
                    motion.velocity.x = -speed;
                }
                else if (entrance[i] == Position.Top)
                {
                    motion.velocity.y = -speed;
                }
                else if (entrance[i] == Position.Bottom)
                {
                    motion.velocity.y = speed;
                }

                motion.friction = friction;
            }

            _start += space;
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

        Vector3 _GetPosition(GameObject gameObject, Position entrance)
        {
            var min = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, gameObject.transform.position.z - Camera.main.transform.position.z));
            var max = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, gameObject.transform.position.z - Camera.main.transform.position.z));

            var size = RendererHelpers.GetSize(gameObject) * .5f * scale;

            min.x -= size.x;
            max.x += size.x;

            min.y += size.y;
            max.y -= size.y;

            var position = gameObject.transform.position;

            if (entrance == Position.Left)
            {
                position.x = min.x;
                //position.y = Random.Range(min.y + size.y, max.y - size.y);
            }
            else if (entrance == Position.Right)
            {
                position.x = max.x;
                //position.y = Random.Range(min.y + size.y, max.y - size.y);
            }
            else if (entrance == Position.Top)
            {
                //position.x = Random.Range(min.x - size.x, max.x + size.x);
                position.y = min.y;
            }
            else if (entrance == Position.Bottom)
            {
                //position.x = Random.Range(min.x - size.x, max.x + size.x);
                position.y = max.y;
            }

            return position;
        }

        bool _cargo;
        float _start;
    }
}
