#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using Playniax.Ignition;

namespace Playniax.ParticleSystem
{
#if UNITY_EDITOR
    [CustomEditor(typeof(EmitterGroup))]
    public class EmitterGroupEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EmitterGroup myScript = (EmitterGroup)target;

            if (GUILayout.Button("Test"))
            {
                myScript.Call(Vector3.zero, myScript.transform.parent, 1, 0);
            }
        }

        void OnDestroy()
        {
            EditorApplication.update = null;

            var particles = FindObjectsOfType<Particle>();

            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i].name.Contains(Emitter.EDITOR_MODE_MARKER)) DestroyImmediate(particles[i].gameObject);
            }
        }
    }
#endif

    [AddComponentMenu("Playniax/Prototyping/Particle System/Emitter Group")]
    public class EmitterGroup : ServiceBase
    {
        [Tooltip("The scale at which time passes.")]
        public float timeScale = 1;
        [Tooltip("The scale to render.")]
        public float scale = 1;
        [Tooltip("The scale in pixels.")]
        public int size;

        public static EmitterGroup Call(string id, Vector3 position, Transform parent = null, float scale = 1, int sortingOrder = 0, float timeScale = 1)
        {
            for (int i = 0; i < services.Count; i++)
            {
                if (services[i].id == id)
                {
                    var group = services[i] as EmitterGroup;
                    if (group)
                    {
                        group.Call(position, parent, scale * group.scale, sortingOrder, timeScale);

                        return group;
                    }
                }
            }

            return null;
        }
        public void Call(Vector3 position, Transform parent = null, float scale = 1, int sortingOrder = 0, float timeScale = 1)
        {
            var emitters= GetComponentsInChildren<Emitter>();

            for (int i = 0; i < emitters.Length; i++)
            {
                emitters[i].Play(position, parent, scale * this.scale, sortingOrder, timeScale * this.timeScale);
            }
        }
    }
}