using UnityEngine;
using UnityEngine.Events;

namespace Playniax.UI.SimpleGameUI
{
    public class LeftOrRightTouch : MonoBehaviour
    {
        public UnityEvent onLeft;
        public UnityEvent onRight;

        void Update()
        {
        if (SimpleGameUI.instance && (SimpleGameUI.instance.isBusy || SimpleGameUI.instance.isMouseOverPauseButton)) return;

            if (Input.GetMouseButtonDown(0) == false) return;

            if (Input.mousePosition.x < Screen.width / 2)
            {
                onLeft.Invoke();
            }
            else
            {
                onRight.Invoke();
            }

        }
    }
}