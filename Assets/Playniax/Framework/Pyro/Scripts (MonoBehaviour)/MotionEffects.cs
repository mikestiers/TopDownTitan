using Playniax.Ignition;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Playniax.Pyro
{
    [AddComponentMenu("Playniax/Pyro/MotionEffects")]
    // Misc motion effects with support for rotating and linear motions.
    public class MotionEffects : MonoBehaviour
    {
#if UNITY_EDITOR
        [CanEditMultipleObjects]
        [CustomEditor(typeof(MotionEffects))]
        public class Inspector : Editor
        {
            SerializedProperty circularSettings;
            SerializedProperty linearSettings;
            SerializedProperty rotateSettings;
            SerializedProperty wobbleSettings;
            void OnEnable()
            {
                circularSettings = serializedObject.FindProperty("circularSettings");
                linearSettings = serializedObject.FindProperty("linearSettings");
                rotateSettings = serializedObject.FindProperty("rotateSettings");
                wobbleSettings = serializedObject.FindProperty("wobbleSettings");
            }
            override public void OnInspectorGUI()
            {
                EditorGUI.BeginChangeCheck();

                serializedObject.Update();

                var myScript = target as MotionEffects;

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(myScript), typeof(MotionEffects), false);
                EditorGUI.EndDisabledGroup();

                myScript.mode = (Mode)EditorGUILayout.EnumPopup("Mode", myScript.mode);

                if (myScript.mode == Mode.Circular)
                {
                    EditorGUILayout.PropertyField(circularSettings, new GUIContent("Circular Settings"));
                }
                else if (myScript.mode == Mode.Linear)
                {
                    EditorGUILayout.PropertyField(linearSettings, new GUIContent("Linear Settings"));
                }
                else if (myScript.mode == Mode.Rotate)
                {
                    EditorGUILayout.PropertyField(rotateSettings, new GUIContent("Rotate Settings"));
                }
                else if (myScript.mode == Mode.Wobble)
                {
                    EditorGUILayout.PropertyField(wobbleSettings, new GUIContent("Wobble Settings"));
                }

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(myScript);

                    serializedObject.ApplyModifiedProperties();
                }
            }
        }
#endif

        [System.Serializable]
        // Settings circular mode.
        public class CircularSettings
        {
            public float speed = 100;
            public float range = 1;
            public float angle = 90;
            public bool startRandom;
            public void Update(MotionEffects instance)
            {
                var localPosition = instance.transform.localPosition;

                localPosition.x += Mathf.Cos(angle * Mathf.Deg2Rad) * Time.deltaTime * range * speed / 100;
                localPosition.y -= Mathf.Sin(angle * Mathf.Deg2Rad) * Time.deltaTime * range * speed / 100;

                instance.transform.localPosition = localPosition;

                angle += Time.deltaTime * speed;

                angle = MathHelpers.Mod(angle, 360);
            }
        }

        [System.Serializable]
        // Settings linear mode.
        public class LinearSettings
        {
            // Velocity.
            public Vector3 velocity = new Vector3(1, 0, 0);
            // Friction.
            public float friction;

            public void Update(MotionEffects instance)
            {
                instance.transform.localPosition += velocity * Time.deltaTime;

                if (friction != 0) velocity *= 1 / (1 + (Time.deltaTime * friction));
            }
        }

        [System.Serializable]
        // Settings rotate mode.
        public class RotateSettings
        {
            // Rotation speed.
            public Vector3 rotation = new Vector3(0, 0, 100);
            // Determines if the object starts from a random angle or not.
            public bool startRandom;

            public void Update(MotionEffects instance)
            {
                instance.transform.Rotate(rotation * Time.deltaTime);
            }
        }

        [System.Serializable]
        // Settings wobble mode.
        public class WobbleSettings
        {
            public enum Mode { scale, x, y, z };

            public Mode mode;
            public float speed = 100;
            public float range = 1;
            public float angle = 90;
            public bool startRandom;
            public void Update(MotionEffects instance)
            {
                if (mode == Mode.scale)
                {
                    instance.transform.localScale += Vector3.one * Mathf.Cos(angle * Mathf.Deg2Rad) * Time.deltaTime * range * speed / 100;
                }
                else if (mode == Mode.x)
                {
                    instance.transform.localPosition += new Vector3(1, 0, 0) * Mathf.Cos(angle * Mathf.Deg2Rad) * Time.deltaTime * range * speed / 100;
                }
                else if (mode == Mode.y)
                {
                    instance.transform.localPosition += new Vector3(0, 1, 0) * Mathf.Sin(angle * Mathf.Deg2Rad) * Time.deltaTime * range * speed / 100;
                }
                else if (mode == Mode.z)
                {
                    instance.transform.localPosition += new Vector3(0, 0, 1) * Mathf.Cos(angle * Mathf.Deg2Rad) * Time.deltaTime * range * speed / 100;
                }

                angle += Time.deltaTime * speed;

                angle = MathHelpers.Mod(angle, 360);
            }
        }

        public enum Mode { Linear, Rotate, Circular, Wobble };

        // Mode can be Mode.Linear or Mode.Rotate.
        public Mode mode = Mode.Linear;
        // Settings circular mode.
        public CircularSettings circularSettings;
        // Settings linear mode.
        public LinearSettings linearSettings;
        // Settings rotate mode.
        public RotateSettings rotateSettings;
        // Settings wobble mode.
        public WobbleSettings wobbleSettings;

        void Awake()
        {
            if (mode == Mode.Circular)
            {
                if (circularSettings.startRandom == true) circularSettings.angle = Random.Range(0, 359);
            }
            else if (mode == Mode.Linear)
            {
            }
            else if (mode == Mode.Rotate)
            {
                if (rotateSettings.startRandom == true) transform.Rotate(0, 0, Random.Range(0, 359));
            }
            else if (mode == Mode.Wobble)
            {
                if (wobbleSettings.startRandom == true) wobbleSettings.angle = Random.Range(0, 359);
            }
        }

        void Update()
        {
            if (mode == Mode.Circular)
            {
                circularSettings.Update(this);
            }
            else if (mode == Mode.Linear)
            {
                linearSettings.Update(this);
            }
            else if (mode == Mode.Rotate)
            {
                rotateSettings.Update(this);
            }
            else if (mode == Mode.Wobble)
            {
                wobbleSettings.Update(this);
            }
        }
    }
}