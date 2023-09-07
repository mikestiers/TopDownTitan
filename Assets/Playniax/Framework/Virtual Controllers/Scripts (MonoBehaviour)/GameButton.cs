using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Playniax.VirtualControllers
{
    public class GameButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
    {
        public KeyCode key;
        public UnityEvent onDown;
        void Update()
        {
            if (_mouseDown || Input.GetKey(key)) onDown.Invoke();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            _mouseDown = true;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            _mouseDown = false;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
        }
        public void OnPointerExit(PointerEventData eventData)
        {
        }

        bool _mouseDown;
    }
}
