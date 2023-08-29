using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingRotation : MonoBehaviour
{
    public float rotationSpeed = 20f;
    public float moveSpeed = 1f;

    void Update()
    {
        // Rotate around its z-axis
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        //// Move downwards
        float newYPosition = transform.position.y - moveSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, newYPosition, 0);
    }
}

