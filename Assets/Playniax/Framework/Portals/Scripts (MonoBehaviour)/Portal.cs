using UnityEngine;
using Playniax.Ignition;

namespace Playniax.Portals
{
    public class Portal : SpawnerBase
    {
        public class Glow : MonoBehaviour
        {
            public float position;
            public float speed;
            public SpriteRenderer spriteRenderer;
            public Portal portal;

            void Update()
            {
                spriteRenderer.sortingOrder = portal.spriteRenderer.sortingOrder + Random.Range(-1, 1);

                var x = Mathf.Cos(position) * portal.glowPosition;
                var y = Mathf.Sin(position) * portal.glowPosition;

                transform.localPosition = new Vector3(x, y);

                position += speed * Time.deltaTime;
            }
        }

        public class Intro : MonoBehaviour
        {
            public Vector3 scale;
            public int steps = 10;
            void Update()
            {
                var localScale = transform.localScale;

                localScale.x += (scale.x - localScale.x) / steps;
                localScale.y += (scale.y - localScale.y) / steps;
                localScale.z += (scale.y - localScale.z) / steps;

                transform.localScale = localScale;

                if (transform.localScale == scale) Destroy(this);
            }
        }

        [System.Serializable]
        public class SpawnerSettings
        {
            [System.Serializable]
            public class SoundSettings
            {
                public AudioProperties open = new AudioProperties();
                public AudioProperties intro = new AudioProperties();
                public AudioProperties close = new AudioProperties();
            }

            public GameObject[] prefabs;
            public Transform parent;
            public float timer;
            public float interval;
            public float intervalRange;
            public int counter = 1;
            public int maxAtOnce = 1;
            public int introSteps = 10;
            //public float scale = 1f;
            public bool trackProgress;
            public SoundSettings soundSettings = new SoundSettings();
        }

        public Sprite glowSprite;
        public float glowPosition = 1.11f;
        public float glowSize = .5f;
        public int glowParticles = 25;
        public float glowRotationSpeed = 1;
        public Material glowMaterial;
        public int growSteps = 50;
        public float growScale = 1;
        public SpriteRenderer spriteRenderer;

        public SpawnerSettings spawnerSettings = new SpawnerSettings();

        public float growSize
        {
            get { return _growSize * growScale; }
        }

        public virtual bool isAllowed
        {
            get { return true; }
        }

        public override void IgnitionInit()
        {
            for (int i = 0; i < spawnerSettings.prefabs.Length; i++)
                if (spawnerSettings.prefabs[i] && spawnerSettings.prefabs[i].scene.rootCount > 0) spawnerSettings.prefabs[i].SetActive(false);
        }

        public virtual GameObject OnSpawn()
        {
            var clone = Instantiate(spawnerSettings.prefabs[_index], transform.position, Quaternion.identity, spawnerSettings.parent);
            if (clone)
            {
                if (spawnerSettings.trackProgress)
                {
                    var progress = clone.GetComponent<ProgressCounter>();
                    if (progress == null) progress = clone.AddComponent<ProgressCounter>();
                }

                var intro = clone.AddComponent<Intro>();
                intro.steps = spawnerSettings.introSteps;
                intro.scale = clone.transform.localScale;
                clone.transform.localScale = Vector3.zero;
                clone.SetActive(true);

                spawnerSettings.soundSettings.intro.Play();
            }

            return clone;
        }

        public virtual bool SetPosition()
        {
            return true;
        }

        public override void Awake()
        {
            base.Awake();

            _InitGlow();
            _InitSpawner();
        }

        void Update()
        {
            _Update();
        }

        void _GetPrefab()
        {
            _index = Random.Range(0, spawnerSettings.prefabs.Length);

            var size = RendererHelpers.GetSize(spawnerSettings.prefabs[_index]);

            _growSize = Mathf.Max(size.x, size.y) * .5f;
        }

        int _GetPrefabs()
        {
            var prefabs = 0;

            for (int i = 0; i < spawnerSettings.prefabs.Length; i++)
                if (spawnerSettings.prefabs[i]) prefabs += 1;

            return prefabs;
        }

        void _InitGlow()
        {
            for (int i = 0; i < glowParticles; i++)
            {
                var glow = new GameObject("Glow " + (i + 1)).AddComponent<Glow>();
                glow.portal = this;
                glow.speed = Random.Range(-glowRotationSpeed, glowRotationSpeed);
                glow.position = Random.Range(0, 359) * Mathf.Deg2Rad;
                glow.transform.localScale *= Random.Range(glowSize / 2, glowSize);

                glow.spriteRenderer = glow.gameObject.AddComponent<SpriteRenderer>();
                glow.spriteRenderer.sprite = glowSprite;
                glow.spriteRenderer.material = glowMaterial;
                glow.spriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;

                glow.transform.SetParent(spriteRenderer.transform);
            }
        }
        void _InitSpawner()
        {
            if (_GetPrefabs() > 0)
            {
                transform.localScale = new Vector3(0, 0, 1);

                spriteRenderer.gameObject.SetActive(false);
            }

            if (spawnerSettings.trackProgress)
            {
                GameData.progressScale += spawnerSettings.counter;
                GameData.progress += spawnerSettings.counter;
            }
        }

        void _Update()
        {
            if (_GetPrefabs() == 0) return;

            if (isAllowed && _state == 0)
            {
                if (spawnerSettings.counter == -1)
                {
                    if (spawnerSettings.timer <= 0)
                    {
                        _GetPrefab();

                        _state = 1;

                        spawnerSettings.timer = Random.Range(spawnerSettings.interval, spawnerSettings.interval + spawnerSettings.intervalRange);
                    }
                    else
                    {
                        spawnerSettings.timer -= 1 * Time.deltaTime;
                    }
                }
                else if (spawnerSettings.counter > 0)
                {
                    if (spawnerSettings.timer <= 0)
                    {
                        _GetPrefab();

                        _state = 1;

                        spawnerSettings.counter -= 1;

                        if (spawnerSettings.counter > 0)
                        {
                            spawnerSettings.timer = Random.Range(spawnerSettings.interval, spawnerSettings.interval + spawnerSettings.intervalRange);
                        }
                    }
                    else
                    {
                        spawnerSettings.timer -= 1 * Time.deltaTime;
                    }
                }

            }
            else if (_state == 1)
            {
                if (SetPosition() == true)
                {
                    spriteRenderer.gameObject.SetActive(true);

                    spawnerSettings.soundSettings.open.Play();

                    _state = 2;
                }
            }
            else if (_state == 2)
            {
                var size = _growSize * growScale;

                transform.localScale += new Vector3(size / growSteps, size / growSteps, 0) * Time.deltaTime * 50;

                if (transform.localScale.x >= size)
                {
                    _space = 1;

                    _state = 3;
                }
            }
            else if (_state == 3)
            {
                _space -= Time.deltaTime;

                if (_space <= 0)
                {
                    if (spawnerSettings.maxAtOnce <= 1)
                    {
                        _state = 4;
                    }
                    else
                    {
                        if (spawnerSettings.counter == -1)
                        {
                            _multipleCounter = Random.Range(0, spawnerSettings.maxAtOnce);
                        }
                        else
                        {
                            _multipleCounter = Random.Range(0, spawnerSettings.maxAtOnce);

                            if (_multipleCounter > spawnerSettings.counter) _multipleCounter = spawnerSettings.counter;

                            spawnerSettings.counter -= _multipleCounter;
                        }

                        _state = 5;
                    }
                }
            }
            else if (_state == 4)
            {
                OnSpawn();

                _space = 1;

                _state = 6;
            }
            else if (_state == 5)
            {
                _space -= Time.deltaTime;

                if (_space <= 0)
                {
                    OnSpawn();

                    _multipleCounter -= 1;

                    if (_multipleCounter < 0)
                    {
                        _space = 1;

                        _state = 6;
                    }
                    else
                    {
                        _space = .5f;
                    }
                }
            }
            else if (_state == 6)
            {
                _space -= Time.deltaTime;

                if (_space <= 0)
                {
                    _state = 7;
                }

            }
            else if (_state == 7)
            {
                var size = _growSize * growScale;

                transform.localScale -= new Vector3(size / growSteps, size / growSteps, 0) * Time.deltaTime * 50;

                if (transform.localScale.x <= 0)
                {
                    if (spawnerSettings.counter == 0)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        spriteRenderer.gameObject.SetActive(false);

                        _state = 0;
                    }

                    spawnerSettings.soundSettings.close.Play();
                }
            }
        }

        int _index;
        float _growSize;
        int _multipleCounter;
        float _space;
        int _state;
    }
}
