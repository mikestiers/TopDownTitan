// https://docs.unity3d.com/ScriptReference/EditorGUI.MinMaxSlider.html

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Playniax.Ignition;

namespace Playniax.ParticleSystem
{
#if UNITY_EDITOR
    [CustomEditor(typeof(Emitter))]
    public class EmitterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Emitter myScript = (Emitter)target;

            if (GUILayout.Button("Test"))
            {
                myScript.Play(Vector3.zero, myScript.transform.parent, 1, 0);
            }
        }

        void OnDestroy()
        {
            EditorApplication.update = null;
        }
    }
#endif

    [AddComponentMenu("Playniax/Prototyping/Particle System/Emitter")]
    public class Emitter : MonoBehaviour
    {
#if UNITY_EDITOR
        public const string EDITOR_MODE_MARKER = " (Editor Mode)";
#endif
        [Tooltip("The Identifier.")]
        public string id = "Flash";
        [Tooltip("The number of particles to initialize or maximum of particles in the pool.")]
        public int initialize = 16;
        [Tooltip("Determines if the initial initalized particle pool is allowed to exceed the limit.")]
        public bool autoGrow = false;
#if UNITY_EDITOR
        public bool hideInHierarchy = true;
#endif
        [Tooltip("The sprite to use for the particles.")]
        public Sprite sprite;
        [Tooltip("The material to use for the particles.")]
        public Material material;
        [Tooltip("The scale at which time passes.")]
        public string timeScale = "1";
        [Tooltip("Suspends the execution for the given amount.")]
        public string delay = "0";
        [Tooltip("The particles time to live.")]
        public string ttl = "1";
        [Tooltip("The number of particles to use.")]
        public string particles = "8";
        [Tooltip("The origin of the particles.")]
        public string origin = "0";
        [Tooltip("The scale to render.")]
        public string scale = "1";
        [Tooltip("The angle of the particles.")]
        public string angle = "0,359";
        [Tooltip("The start scale of the particles.")]
        public string startScale = "1";
        [Tooltip("The target scale of the particles.")]
        public string targetScale = "1";
        [Tooltip("The fixed scale of the particles.")]
        public Vector3 fixedScale;
        public Vector3 constant;
        [Tooltip("The speed of the particles.")]
        public string speed = "1";
        [Tooltip("Determines if the effects implodes or not.")]
        public bool implode;
        [Tooltip("The friction of the particles.")]
        public string friction = "0";
        [Tooltip("The gravity of the particles.")]
        public string gravity = "0";
        [Tooltip("The spinning speed of the particles.")]
        public string spin = "0";
        [Tooltip("The start color of the particles.")]
        public Color startColor = new Color(1, 1, 1, 1);
        [Tooltip("The target color of the particles.")]
        public Color targetColor;
        public bool randomColorRange = false;
        [Tooltip("The renderer's order within a sorting layer.")]
        public string orderInLayer = "0";
        void OnValidate()
        {
            timeScale = Regex.Replace(timeScale, @"[^0-9.,]", "");
            delay = Regex.Replace(delay, @"[^0-9.,]", "");
            ttl = Regex.Replace(ttl, @"[^0-9.,]", "");
            origin = Regex.Replace(origin, @"[^0-9.,]", "");
            scale = Regex.Replace(scale, @"[^0-9.,]", "");
            angle = Regex.Replace(angle, @"[^0-9.,]", "");
            startScale = Regex.Replace(startScale, @"[^0-9.,]", "");
            targetScale = Regex.Replace(targetScale, @"[^0-9.,]", "");
            speed = Regex.Replace(speed, @"[^0-9.,]", "");
            friction = Regex.Replace(friction, @"[^0-9.,]", "");
            gravity = Regex.Replace(gravity, @"[^0-9.,]", "");
            spin = Regex.Replace(spin, @"[^0-9.,]", "");

            particles = Regex.Replace(particles, @"[^0-9,]", "");
            orderInLayer = Regex.Replace(orderInLayer, @"[^0-9,]", "");
        }

#if UNITY_EDITOR
        void Reset()
        {
            sprite = AssetDatabase.LoadAssetAtPath("Assets/Playniax/Prototyping/Particle System/Textures/glow 1.png", typeof(Sprite)) as Sprite;
        }
#endif
        public void OnEnable()
        {
            _emitters.Add(this);
        }

        public void OnDisable()
        {
            _emitters.Remove(this);
        }

        public static Emitter Find(string id)
        {
            if (_emitters == null) return null;

            for (int i = 0; i < _emitters.Count; i++)
            {
                if (_emitters[i].id == id) return _emitters[i];
            }

            return null;
        }

        public static int FindIndex(string id)
        {
            if (_emitters == null) return -1;

            for (int i = 0; i < _emitters.Count; i++)
            {
                if (_emitters[i].id == id) return i;
            }
            return -1;
        }

        public static void Play(string id, Vector3 position, Transform parent = null, float scale = 1, int sortingOrder = 0, float timeScale = 1)
        {
            for (int i = 0; i < _emitters.Count; i++)
            {
                if (_emitters[i].id == id) _emitters[i].Play(position, parent, scale, sortingOrder, timeScale);
            }
        }

        public void Play(Vector3 position, Transform parent = null, float scale = 1, int sortingOrder = 0, float timeScale = 1)
        {
            _Init();

            scale *= _RandomFloat(this.scale, 1);

            for (int i = 0; i < _RandomInt(particles, 8); i++)
            {
                var gameObject = _GetGameObject();
                if (gameObject == null) return;

                var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                var particle = gameObject.GetComponent<Particle>();

                spriteRenderer.enabled = false;

                spriteRenderer.sortingOrder = sortingOrder + _RandomInt(orderInLayer);

                particle.delay = _RandomFloat(delay);

                particle.timeScale = _FloatParse(this.timeScale) * timeScale;

                particle.startScale = _Vec3Parse(startScale) * scale;
                particle.targetScale = _Vec3Parse(targetScale) * scale;

                if (fixedScale.x != 0)
                {
                    particle.startScale = new Vector3(fixedScale.x * scale, particle.startScale.y, particle.startScale.z);
                    particle.targetScale.x = fixedScale.x * scale;
                }

                if (fixedScale.y != 0)
                {
                    particle.startScale = new Vector3(particle.startScale.x, fixedScale.y * scale, particle.startScale.z);
                    particle.targetScale.y = fixedScale.y * scale;
                }

                if (fixedScale.z != 0)
                {
                    particle.startScale = new Vector3(particle.startScale.x, fixedScale.y, particle.startScale.z * scale);
                    particle.targetScale.z = fixedScale.z * scale;
                }

                particle.friction = _RandomFloat(friction, 1);

                particle.gravity = _FloatParse(gravity);
                particle.constant = constant;

                if (randomColorRange == false)
                {
                    particle.startColor = startColor;
                    particle.targetColor = targetColor;
                }
                else
                {
                    particle.startColor = _RandomColor(startColor, targetColor, startColor.a);
                    particle.targetColor = _RandomColor(startColor, targetColor, targetColor.a);
                }

                if (material) spriteRenderer.material = material;

                particle.ttl = _RandomFloat(ttl, particle.ttl);

                var angle = _RandomInt(this.angle);

                var x = Mathf.Cos(angle * Mathf.Deg2Rad);
                var y = Mathf.Sin(angle * Mathf.Deg2Rad);
                var z = Mathf.Cos(angle * Mathf.Deg2Rad);

                var speed = _RandomFloat(this.speed) * scale;
                var origin = _RandomFloat(this.origin) * scale;

                var velocity = new Vector3(x, y, z);

                particle.velocity = velocity * speed * (implode ? -1 : 1);

                particle.spin = new Vector3(0, 0, _RandomFloat(spin));

                gameObject.transform.SetParent(parent);
                gameObject.transform.position = position;
                gameObject.transform.position += velocity * origin;

                gameObject.transform.localRotation = Quaternion.Euler(0, 0, angle);

                if (ParticlePlugin.instance) ParticlePlugin.instance.OnPostProcess(particle);

                gameObject.SetActive(true);
            }
        }

        GameObject _CreateParticle()
        {
            var particle = new GameObject().AddComponent<Particle>();
            particle.gameObject.SetActive(false);

            particle.name = sprite.name;

            var renderer = particle.gameObject.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.enabled = false;

#if UNITY_EDITOR
            if (hideInHierarchy || Application.isPlaying == false) particle.gameObject.hideFlags = HideFlags.HideInHierarchy;

            if (Application.isPlaying == true) particle.name += ObjectPooler.MARKER;

            if (Application.isPlaying == false) particle.name += EDITOR_MODE_MARKER;

            if (Application.isPlaying == false) EditorApplication.update += particle.UpdateParticle;
#else
            particle.name += ObjectPooler.MARKER;

#endif
            return particle.gameObject;
        }

        float _FloatParse(string str)
        {
            str = str.Trim();

            if (str == "") return 0;
#if UNITY_EDITOR
            str = Regex.Replace(str, @"[^0-9.]", "");
#endif
            return float.Parse(str, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
        }
        int _IntParse(string str)
        {
            str = str.Trim();

            if (str == "") return 0;

#if UNITY_EDITOR
            str = Regex.Replace(str, @"[^0-9]", "");
#endif
            return int.Parse(str, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
        }

        GameObject _GetGameObject(int index = 0)
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false) return _CreateParticle();
#endif
            for (int i = 0; i < _pool.Count; i++)
            {
                if (_pool[i] && !_pool[i].activeInHierarchy) return _pool[i];
            }

            if (autoGrow && sprite)
            {
                var particle = _CreateParticle();

                _pool.Add(particle);

                return particle;
            }

            return null;
        }

        void _Init()
        {

#if UNITY_EDITOR
            if (Application.isPlaying == false) return;
#endif

            if (initialize == _initialized) return;

            if (_pool == null) return;

            _pool.Clear();

            for (int i = 0; i < initialize; i++)
            {
                _pool.Add(_CreateParticle());
            }

            _initialized = initialize;
        }

        Color _RandomColor(Color startColor, Color targetColor, float alpha)
        {
            float r = Random.Range(startColor.r, targetColor.r);
            float g = Random.Range(startColor.g, targetColor.g);
            float b = Random.Range(startColor.b, targetColor.b);
            return new Color(r, g, b, alpha);
        }

        float _RandomFloat(string str, float defaultValue = 0)
        {
            str = str.Trim();

            if (str == "") return defaultValue;

            if (str.Contains(","))
            {
                string[] r = str.Split(',');

                if (r.Length == 1) return _FloatParse(str);

                float min = _FloatParse(r[0]);
                float max = _FloatParse(r[1]);

                return Random.Range(min, max);
            }
            else
            {
                return _FloatParse(str);
            }
        }

        int _RandomInt(string str, int defaultValue = 0)
        {
            str = str.Trim();

            if (str == "") return defaultValue;

            if (str.Contains(","))
            {
                string[] r = str.Split(',');

                if (r.Length == 1) return _IntParse(str);

                int min = _IntParse(r[0]);
                int max = _IntParse(r[1]);

                return Random.Range(min, max);
            }
            else
            {
                return _IntParse(str);
            }
        }

        Vector2 _Vec2Parse(string str)
        {
            var r = _RandomFloat(str);
            return new Vector2(r, r);
        }
        Vector3 _Vec3Parse(string str)
        {
            var r = _RandomFloat(str);
            return new Vector3(r, r, r);
        }

        static List<Emitter> _emitters = new List<Emitter>();

        int _initialized;

        List<GameObject> _pool = new List<GameObject>();
    }
}