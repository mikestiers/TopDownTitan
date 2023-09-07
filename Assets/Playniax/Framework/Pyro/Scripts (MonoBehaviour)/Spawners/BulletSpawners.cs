#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using Playniax.Ignition;

namespace Playniax.Pyro
{
    public class BulletSpawners : BulletSpawnerBase
    {
#if UNITY_EDITOR

        // Not finished!

        [CustomEditor(typeof(BulletSpawners))]
        public class BulletSpawnersEditor : Editor
        {
            void OnSceneGUI()
            {
                var bulletSpawners = (BulletSpawners)target;

                if (bulletSpawners.drawGizmos && bulletSpawners.spawnPoints.Length > 0 && Selection.activeGameObject == bulletSpawners.gameObject)
                {
                    float size = .05f;
                    Vector3 snap = Vector3.one * 0.5f;

                    for (int i = 0; i < bulletSpawners.spawnPoints.Length; i++)
                    {
                        bulletSpawners.spawnPoints[i].position = Handles.FreeMoveHandle(bulletSpawners.spawnPoints[i].position, Quaternion.identity, size, snap, Handles.RectangleHandleCap);

                        var renderer = bulletSpawners.prefab.GetComponent<SpriteRenderer>();
                        var sprite = renderer.sprite;
                        var position = bulletSpawners.spawnPoints[i].position;

                        Handles.Label(position, sprite.texture);
                    }
                }
            }
        }
#endif
        [System.Serializable]
        public class SpawnPoints
        {
            public string name;
            public int group;
            public Vector3 position;
            public Vector3 rotation;
            public float speed = 16;
        }

        [System.Serializable]
        public class CollisionSettings
        {
            public bool useTheseSettings;
            public int structuralIntegrity = 1;
        }

        [System.Serializable]
        public class PowerSettings
        {
            public bool useTheseSettings;
            public float powerRange = 1000;
            public bool visualize;
            public float visualizeScale = .25f;
        }

        [System.Serializable]
        public class TriggerSettings
        {
#if UNITY_EDITOR
            [CustomPropertyDrawer(typeof(TriggerSettings))]
            public class Drawer : PropertyDrawer
            {
                public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
                {
                    Rect rect;

                    float y = 0;

                    EditorGUI.BeginProperty(position, label, property);

                    var indent = EditorGUI.indentLevel;

                    rect = new Rect(position.min.x, position.min.y + y, position.size.x, EditorGUIUtility.singleLineHeight); y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(rect, property.FindPropertyRelative("mode"));

                    if (property.FindPropertyRelative("mode").enumValueIndex == 1)
                    {
                        EditorGUI.indentLevel += 1;

                        rect = new Rect(position.min.x, position.min.y + y, position.size.x, EditorGUIUtility.singleLineHeight); y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                        EditorGUI.PropertyField(rect, property.FindPropertyRelative("Button1"));
                        rect = new Rect(position.min.x, position.min.y + y, position.size.x, EditorGUIUtility.singleLineHeight); y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                        EditorGUI.PropertyField(rect, property.FindPropertyRelative("Button2"));
                        rect = new Rect(position.min.x, position.min.y + y, position.size.x, EditorGUIUtility.singleLineHeight); y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                        EditorGUI.PropertyField(rect, property.FindPropertyRelative("autofire"));

                        if (property.FindPropertyRelative("autofire").boolValue == true)
                        {
                            EditorGUI.indentLevel = 2;

                            rect = new Rect(position.min.x, position.min.y + y, position.size.x, EditorGUIUtility.singleLineHeight); y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                            EditorGUI.PropertyField(rect, property.FindPropertyRelative("rapidFire"));
                        }
                    }

                    EditorGUI.indentLevel = indent;

                    EditorGUI.EndProperty();
                }

                public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
                {
                    float totalLines = 1;

                    if (property.FindPropertyRelative("mode").enumValueIndex == 1)
                    {
                        totalLines += 3;

                        if (property.FindPropertyRelative("autofire").boolValue == true)
                        {
                            totalLines += 1;
                        }
                    }

                    return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * totalLines;
                }
            }
#endif
            public enum Mode { Auto, Player, Smart };

            public Mode mode;
            public KeyCode Button1 = KeyCode.Joystick1Button0;
            public KeyCode Button2 = KeyCode.None;
            public bool autofire;
            public bool rapidFire;
        }

        public int group;
        public GameObject prefab;
        public Transform parent;
        public SpawnPoints[] spawnPoints = new SpawnPoints[1];
        public float scale = 1;
        public Vector3 rotation;
        [Header("Trigger Settings")]
        public TriggerSettings triggerSettings;
        public AudioProperties audioProperties;
        public CollisionSettings overrideCollisionSettings;
        public PowerSettings powerSettings = new PowerSettings();
        public bool prefabSmartOverrides = true;
#if UNITY_EDITOR
        public bool drawGizmos;
#endif
        public override void UpdateSpawner()
        {
            if (prefab == null) return;

            if (triggerSettings.mode == TriggerSettings.Mode.Auto)
            {
                if (timer.Update()) OnSpawn();
            }
            else if (triggerSettings.mode == TriggerSettings.Mode.Smart && BulletSpawnerHelper.count > 0)
            {
                if (timer.Update()) OnSpawn();
            }
            else if (triggerSettings.mode == TriggerSettings.Mode.Player)
            {
                if (triggerSettings.autofire)
                {
                    if (Input.GetKey(triggerSettings.Button1) || Input.GetKey(triggerSettings.Button2))
                    {
                        if (timer.Update()) OnSpawn();
                    }
                    else if (triggerSettings.rapidFire == true)
                    {
                        timer.timer = 0;
                    }
                }
                else
                {
                    if (Input.GetKeyDown(triggerSettings.Button1) || Input.GetKeyDown(triggerSettings.Button2))
                    {
                        if (timer.Countdown()) OnSpawn();
                    }
                }
            }
        }
        public override void IgnitionInit()
        {
            if (prefab && prefab.scene.rootCount > 0) prefab.SetActive(false);
        }
        public override void OnSpawn()
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (spawnPoints[i].group == group)
                {
                    OnSpawn(i);
                }
            }

            audioProperties.Play();
        }

        public void OnSpawn(int i)
        {
            var clone = Instantiate(prefab, transform.position, transform.rotation);
            if (clone)
            {
                clone.transform.Rotate(rotation);
                clone.transform.Rotate(spawnPoints[i].rotation);
                clone.transform.localScale *= scale;
                clone.transform.Translate(spawnPoints[i].position);

                var bulletBase = clone.GetComponent<BulletBase>();
                if (bulletBase) bulletBase.velocity = clone.transform.rotation * new Vector3(spawnPoints[i].speed, 0, 0);

                if (parent)
                {
                    clone.transform.parent = parent;
                }
                else
                {
                    clone.transform.parent = transform.parent;
                }

                if (overrideCollisionSettings.useTheseSettings) _OverrideCollisionSettings(clone);

                var scoreBase = clone.GetComponent<IScoreBase>();

                if (scoreBase != null)
                {
                    if (powerSettings.useTheseSettings)
                    {
                        if (timer.counter > 0)
                        {
                            var m = timer.counter / powerSettings.powerRange;

                            scoreBase.structuralIntegrity *= m + 1;

                            if (powerSettings.visualize) clone.transform.localScale *= m * powerSettings.visualizeScale + 1;
                        }
                    }

                    scoreBase.structuralIntegrity *= BulletSpawnerSettings.GetStructuralIntegrityMultiplier();
                }

                if (prefabSmartOverrides) _SmartOverrides(clone);

                clone.SetActive(true);
            }
        }

        void _OverrideCollisionSettings(GameObject clone)
        {
            var scoreBase = clone.GetComponent<IScoreBase>();
            if (scoreBase != null)
            {
                scoreBase.structuralIntegrity = overrideCollisionSettings.structuralIntegrity;
            }
        }
        void _SmartOverrides(GameObject clone)
        {
            if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_spriteRenderer)
            {
                var orderInLayer = _spriteRenderer.sortingOrder;
                _spriteRenderer = clone.GetComponent<SpriteRenderer>();
                if (_spriteRenderer != null) _spriteRenderer.sortingOrder = orderInLayer + 1;
            }

            var scoreBase = clone.GetComponent<IScoreBase>();
            if (scoreBase != null) scoreBase.friend = gameObject;
        }

        SpriteRenderer _spriteRenderer;
    }
}