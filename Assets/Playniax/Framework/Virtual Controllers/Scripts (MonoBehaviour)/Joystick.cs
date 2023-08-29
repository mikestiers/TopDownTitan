// https://pressstart.vip/tutorials/2018/06/22/39/mobile-joystick-in-unity.html

using UnityEngine;
using UnityEngine.UI;
namespace Playniax.VirtualControllers
{
    public class Joystick : MonoBehaviour
    {
        public static Joystick instance;

        public Image image;

        public Vector2 direction;
        //    public float deadzone = 8;

        public float range = 16;

        void Awake()
        {
            instance = this;

            if (image == null) image = GetComponent<Image>();

            image.enabled = false;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _pointA = Input.mousePosition;

                image.enabled = true;
            }
            if (Input.GetMouseButton(0))
            {
                _pointB = Input.mousePosition;

                _touchStart = true;
            }
            else
            {
                _touchStart = false;
            }

            if (_touchStart)
            {
                direction = Vector2.ClampMagnitude(_pointB - _pointA, range);

                transform.position = new Vector2(_pointA.x + direction.x, _pointA.y + direction.y);
            }
            else
            {
                image.enabled = false;

                direction = default;
            }
        }

        bool _touchStart = false;

        Vector2 _pointA;
        Vector2 _pointB;
    }
}