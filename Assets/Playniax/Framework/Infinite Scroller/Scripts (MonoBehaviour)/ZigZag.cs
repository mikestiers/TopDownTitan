using UnityEngine;

namespace Playniax.InfiniteScroller
{
    public class ZigZag : MonoBehaviour
    {
        public KeyCode controlKey;
        public float rotationSpeed = 100;
        public float speed = 1;
        public Scroller scroller;
        void Update()
        {
            if (_previousDirection == 0 && _direction != 0) _previousDirection = _direction;

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(controlKey))
            {
                _direction = 0;
            }
            else if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(controlKey))
            {
                if (_direction == 0) _direction = _previousDirection;
                _direction = -_direction;
                _previousDirection = _direction;
            }

            transform.Rotate(new Vector3(0, 0, _direction * rotationSpeed) * Time.deltaTime);

            scroller.velocity += (Vector2)transform.right * speed * speed * Time.deltaTime;
        }

        int _direction = 1;
        int _previousDirection;
    }
}
