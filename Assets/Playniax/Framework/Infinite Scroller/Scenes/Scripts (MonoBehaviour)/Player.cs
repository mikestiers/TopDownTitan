using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playniax.InfiniteScroller
{
    public class Player : MonoBehaviour
    {
        // Some variables
        public string horizontal = "Horizontal";
        public string vertical = "Vertical";
        public float rotationSpeed = 250;
        public float speed = 1;
        public Scroller scroller;

        void Update()
        {
            // Reading the user input
            var h = Input.GetAxis(horizontal);
            var v = Input.GetAxis(vertical);

            // Rotate the player
            transform.Rotate(0, 0, -h * rotationSpeed * Time.deltaTime);

            // Getting the angle
            float angle = gameObject.transform.eulerAngles.z;

            // Calculate the velocity
            scroller.velocity += (Vector2)(Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right * v * speed * Time.deltaTime);
        }
    }
}
