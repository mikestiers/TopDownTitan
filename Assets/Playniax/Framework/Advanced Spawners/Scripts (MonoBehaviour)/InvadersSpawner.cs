using UnityEngine;
using System.Collections.Generic;
using Playniax.Pyro;

namespace Playniax.Sequencer
{

    public class InvadersSpawner : AdvancedSpawnerBase
    {
        public float distance = 1;
        public float speedScale = 1;

        public AnimationCurve speedCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 5));
        public int total => rows * columns;
        public float killed => (float)invadersLeft / total;

        public int rows = 3;
        public int columns = 12;

        public Vector3 rotation = new Vector3(0, 0, 90);

        public float structuralIntegrityMultiplier = 1;
        public BulletSpawner.Settings bulletSettings = new BulletSpawner.Settings();
        public CargoSettings cargoSettings;
        public SurpriseSettings surpriseSettings;
        public ChildSettings childSettings;

        public int invadersLeft
        {
            get { return total - _Count(); }
        }

        public override void IgnitionInit()
        {
            for (int i = 0; i < prefabs.Length; i++)
                if (prefabs[i] && prefabs[i].scene.rootCount > 0) prefabs[i].SetActive(false);
        }
        public override void OnSequencerAwake()
        {
            ProgressCounter.Add(rows * columns);
        }

        public override void OnSequencerUpdate()
        {
            _Clean();

            if (state == 1)
            {
                _Spawn();

                state = 2;
            }
            else if (state > 1)
            {
                if (_list.Count == 0)
                {
                    enabled = false;

                    return;
                }
            }

            if (state == 2)
            {
                float speed = speedCurve.Evaluate(killed) * speedScale;

                transform.position += _direction * speed * Time.deltaTime;

                Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
                Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

                foreach (GameObject invader in _list)
                {
                    if (invader == null) continue;
                    if (invader.gameObject == null) continue;
                    if (invader.gameObject.activeInHierarchy == false) continue;

                    if (_direction.x > 0 && invader.transform.position.x >= (rightEdge.x - 1f))
                    {
                        _Advance();

                        break;
                    }
                    else if (_direction.x < 0 && invader.transform.position.x <= (leftEdge.x + 1f))
                    {
                        _Advance();

                        break;
                    }
                }
            }
            else if (state == 3)
            {
                float speed = speedCurve.Evaluate(killed) * speedScale;

                transform.position += Vector3.down * speed * Time.deltaTime;

                if (transform.position.y <= _position.y)
                {
                    transform.position = _position;

                    state = 2;
                }
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

        void _Advance()
        {
            _direction.x = -_direction.x;

            _position = transform.position + Vector3.down * distance;

            state = 3;
        }

        void _Clean()
        {
            for (int i = 0; i < _list.Count; i++)
                if (_list[i] == null) _list.RemoveAt(i);
        }
        int _Count()
        {
            _Clean();

            return _list.Count;
        }

        void _Spawn()
        {
            var localScale = transform.localScale;

            transform.localScale = Vector3.one;

            int surprise = Random.Range(0, rows * columns);

            for (int x = 0; x < rows; x++)
            {
                float width = distance * (columns - 1);
                float height = distance * (rows - 1);

                Vector2 centerOffset = new Vector2(-width * 0.5f, -height * 0.5f);
                Vector3 rowPosition = new Vector3(centerOffset.x, (distance * x) + centerOffset.y, 0f);

                for (int y = 0; y < columns; y++)
                {
                    var position = rowPosition;
                    position.x += distance * y;

                    var prefab = Random.Range(0, prefabs.Length);

                    var clone = Instantiate(prefabs[prefab], position, Quaternion.Euler(rotation), transform);

                    clone.SetActive(true);

                    clone.AddComponent<Register>();
                    clone.AddComponent<ProgressCounter>();

                    var scoreBase = clone.GetComponent<IScoreBase>();
                    if (scoreBase != null) scoreBase.structuralIntegrity *= structuralIntegrityMultiplier;

                    var offCamera = clone.GetComponent<OffCamera>();
                    if (offCamera == null) offCamera = clone.AddComponent<OffCamera>();

                    offCamera.mode = OffCamera.Mode.Destroy;
                    offCamera.directions = OffCamera.Directions.Bottom;

                    if (bulletSettings.useTheseSettings && bulletSettings.prefab)
                    {
                        var bulletSpawner = clone.GetComponent<BulletSpawner>();
                        if (bulletSpawner == null) bulletSpawner = clone.AddComponent<BulletSpawner>();

                        bulletSpawner.mode = BulletSpawner.Mode.TargetPlayer;

                        bulletSpawner.Set(bulletSettings);
                    }

                    if (surpriseSettings.prefab != null && _list.Count == surprise)
                    {
                        var collisionState = clone.GetComponent<CollisionState>();
                        if (collisionState)
                        {
                            if (collisionState.cargoSettings == null) collisionState.cargoSettings = new CollisionState.CargoSettings();

                            collisionState.cargoSettings.prefab = new GameObject[1] { surpriseSettings.prefab };
                            collisionState.cargoSettings.scale = surpriseSettings.scale;

                            collisionState.cargoSettings.effectSettings = JsonUtility.FromJson<CollisionState.CargoSettings.EffectSettings>(JsonUtility.ToJson(surpriseSettings.effectSettings));
                        }
                    }
                    else if (_cargo || cargoSettings.releaseMode == CargoSettings.ReleaseMode.All)
                    {
                        var collisionState = clone.GetComponent<CollisionState>();
                        if (collisionState)
                        {
                            collisionState.cargoSettings = JsonUtility.FromJson<CollisionState.CargoSettings>(JsonUtility.ToJson(cargoSettings));
                        }
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
                        }
                    }

                    //Vector3 position = rowPosition;
                    //position.x += distance * y;
                    //clone.transform.localPosition = position;
                    //clone.transform.localRotation = Quaternion.Euler(rotation);

                    var animator = clone.GetComponent<Animator>();
                    if (animator)
                    {
                        var state = animator.GetCurrentAnimatorStateInfo(0);
                        animator.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
                    }

                    _cargo = !_cargo;

                    _list.Add(clone);
                }
            }

            transform.localScale = localScale;
        }

        List<GameObject> _list = new List<GameObject>();

        bool _cargo;

        Vector3 _position;
        Vector3 _direction = Vector3.right;
    }
}