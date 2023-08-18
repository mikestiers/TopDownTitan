using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float scrollSpeed = 10.0f;
    public float backgroundHeight = -30.0f;
    public Vector3 topPosition;

    void Update()
    {
        // Move the background image down at a constant speed
        transform.Translate(-transform.up * scrollSpeed * Time.deltaTime);

        // Reset the position when the background goes off-screen
        if (transform.position.y < backgroundHeight) // whatever the height is of the bg image
        {
            transform.position = topPosition; // new Vector3(transform.position.x, 5.0f, transform.position.z);
        }
    }
}
