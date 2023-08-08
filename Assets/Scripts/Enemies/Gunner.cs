using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : Enemy
{
    bool hitWall = false;
    Vector3 diagonalDirection;

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag == "Wall")
        {
            hitWall = true;
        }
    }

    void Start()
    {
        int randomDirection = Random.Range(0, 2);
        hitWall = randomDirection == 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup >= fireCoolDown)
        {
            fireCoolDown = Time.realtimeSinceStartup + fireCoolDown;
            weapon.Fire(enemyCollider);
        }

        // Calculate the diagonal movement direction
        if (hitWall)
        {
            diagonalDirection = -Vector3.right + downDirection;
        }
        else
        {
            diagonalDirection = Vector3.right + downDirection;
        }

        // Normalize the direction to maintain a consistent movement speed
        Vector3 moveDirection = diagonalDirection.normalized;

        // Move the GameObject diagonally at a constant speed
        transform.Translate(moveSpeed * Time.deltaTime * moveDirection);
    }
}
