using UnityEditor;
using Playniax.Essentials.Editor;

namespace Playniax.UI.SimpleGameUI.Menu
{
    public class Helpers : EditorWindowHelpers
    {
        [MenuItem("Playniax/SimpleGameUI/Music Player", false, 71)]
        public static void Add_Music_Player()
        {
            GetAssetAtPath("Assets/Playniax/Framework/SimpleGameUI/Prefabs/Music Player.prefab");
        }
    }
}


