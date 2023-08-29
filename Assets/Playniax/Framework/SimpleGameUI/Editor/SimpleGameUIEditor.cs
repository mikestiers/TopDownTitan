using System.IO;
using UnityEngine;
using UnityEditor;

namespace Playniax.UI.SimpleGameUI
{
    [CustomEditor(typeof(SimpleGameUI))]
    public class SimpleGameUIEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            _Buttons();
        }

        void _Buttons()
        {
            if (Application.isPlaying) return;

            SimpleGameUI myScript = (SimpleGameUI)target;

            EditorGUILayout.Separator();

            if (GUILayout.Button("Add Scenes To Build"))
            {
                myScript.ScenesInBuild();

                EditorWindow.GetWindow(System.Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
            }

            if (GUILayout.Button("Set Current Scene Folder"))
            {
                myScript.searchInFolders = new string[] { Path.GetDirectoryName(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().path) };
            }
        }
    }
}
