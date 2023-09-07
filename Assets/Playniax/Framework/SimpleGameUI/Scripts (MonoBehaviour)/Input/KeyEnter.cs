using UnityEngine;
using UnityEngine.UI;

namespace Playniax.UI.SimpleGameUI
{
    public class KeyEnter : MonoBehaviour
    {
        public Button button;

        public KeyCode keyCode;

        void Awake()
        {
            if (button == null) button = GetComponent<Button>();
        }

        void Update()
        {
            if (SimpleGameUI.instance && SimpleGameUI.instance.screenSettings.effects.screenFader.activeInHierarchy == false && Input.GetKeyDown(keyCode)) button.onClick.Invoke();
        }
    }
}