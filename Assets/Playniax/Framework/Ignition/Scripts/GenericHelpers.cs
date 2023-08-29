﻿#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Playniax.Ignition
{
    [System.Serializable]
    // The Timer class is used by the spawners for example.
    public class Timer
    {
#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(Timer))]
        public class Drawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                Rect rect;

                float y = 0;

                EditorGUI.BeginProperty(position, label, property);

                var indent = EditorGUI.indentLevel;

                rect = new Rect(position.min.x, position.min.y + y, position.size.x, EditorGUIUtility.singleLineHeight); y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("timer"));
                rect = new Rect(position.min.x, position.min.y + y, position.size.x, EditorGUIUtility.singleLineHeight); y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("interval"));
                rect = new Rect(position.min.x, position.min.y + y, position.size.x, EditorGUIUtility.singleLineHeight); y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("intervalRange"));
                rect = new Rect(position.min.x, position.min.y + y, position.size.x, EditorGUIUtility.singleLineHeight); y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("counter"));

                EditorGUI.indentLevel = indent;

                EditorGUI.EndProperty();
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                float totalLines = 4;

                return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * totalLines;
            }
        }
#endif

        [Min(0f)]
        [Tooltip("Timer")]
        public float timer;
        [Tooltip("Interval")]
        public float interval = 0.15f;
        [Min(0f)]
        [Tooltip("Interval Range. Any value over zero triggers random  mode (random value is between interval and interval + intervalRange)")]
        public float intervalRange;
        [Min(-1)]
        [Tooltip("Counter. (-1 is endless)")]
        public int counter = -1;

        public bool counterReachedZero { get; set; }

        public bool Countdown()
        {
            if (counter == -1) return true;

            if (counter == 0) return false;

            counter -= 1;

            return true;
        }

        public bool GetCounterZeroState()
        {
            var state = counterReachedZero;
            counterReachedZero = false;
            return state;
        }
        // Update will return true when timer reaches zero and resets the timer.
        public bool Update(bool ignoreCounter = false)
        {
            if (ignoreCounter)
            {
                timer -= Time.deltaTime;

                if (timer >= 0) return false;

                if (counter != 0) timer = Random.Range(interval, interval + intervalRange);
            }
            else
            {
                if (counter == 0) return false;

                timer -= Time.deltaTime;

                if (timer >= 0) return false;

                if (counter > 0)
                {
                    counter -= 1;

                    if (counter == 0) counterReachedZero = true;
                }

                if (counter != 0) timer = Random.Range(interval, interval + intervalRange);
            }

            return true;
        }
    }
}