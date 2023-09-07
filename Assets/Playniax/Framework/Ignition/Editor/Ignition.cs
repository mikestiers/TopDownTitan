using UnityEditor;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Playniax.Essentials.Editor;

namespace Playniax.Ignition.UI.Editor
{
    public class Helpers : EditorWindowHelpers
    {
        [MenuItem("Playniax/Ignition/UI/ScrollBox", false, 51)]
        public static void Add_ScrollBox()
        {
            var file = SceneManager.GetActiveScene().path;
            var path = Path.GetDirectoryName(file);

            if (File.Exists(path + "/script.txt") == false) FileUtil.CopyFileOrDirectory("Assets/Playniax/Framework/Ignition/Editor/Ignition/script.txt", path + "/script.txt");
            if (File.Exists(path + "/how to play.txt") == false) FileUtil.CopyFileOrDirectory("Assets/Playniax/Framework/Ignition/Editor/Ignition/how to play.txt", path + "/how to play.txt");
            if (File.Exists(path + "/prologue.txt") == false) FileUtil.CopyFileOrDirectory("Assets/Playniax/Framework/Ignition/Editor/Ignition/prologue.txt", path + "/prologue.txt");

            AssetDatabase.Refresh();

            var canvas = GetCanvas();
            if (canvas == null) canvas = AddCanvas();

            var rectTransform = new GameObject("ScrollBox", typeof(RectTransform)).GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(640, 320);
            rectTransform.transform.SetParent(canvas.transform);
            rectTransform.transform.localPosition = Vector3.zero;

            var image = rectTransform.gameObject.AddComponent<Image>();
            image.color = new Color(.25f, .25f, .25f, .25f);
            image.maskable = true;

            rectTransform.gameObject.AddComponent<Mask>();

            var script = AssetDatabase.LoadAssetAtPath(path + "/script.txt", typeof(TextAsset)) as TextAsset;

            var scrollBox = rectTransform.gameObject.AddComponent<ScrollBox>();
            scrollBox.externalScript = script;
            scrollBox.useExternalScript = true;

            scrollBox.assetBank = new AssetBank();

            var font = Resources.GetBuiltinResource<Font>("Arial.ttf");

            var howtoplay = AssetDatabase.LoadAssetAtPath(path + "/how to play.txt", typeof(TextAsset)) as TextAsset;
            var prologue = AssetDatabase.LoadAssetAtPath(path + "/prologue.txt", typeof(TextAsset)) as TextAsset;
            var playniax = AssetDatabase.LoadAssetAtPath("Assets/Playniax/Framework/Ignition/Textures/playniax.png", typeof(Sprite)) as Sprite;

            scrollBox.assetBank.assets = new Object[4] { font, howtoplay, prologue, playniax };

            scrollBox.gameObject.AddComponent<ScrollBoxAutoScroll>();

            Undo.RegisterCreatedObjectUndo(scrollBox.gameObject, "Create object");

            Selection.activeGameObject = scrollBox.gameObject;
        }
    }
}
