using UnityEngine;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

using System.Runtime.InteropServices;

namespace Playniax.Windows
{

    public class Win32
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out Vector2Int position);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);
    }

#endif
}