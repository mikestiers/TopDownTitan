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
            hitWall = !hitWall;
        }
    }

    protected override void Start()
    {
        // Instantiate the starting weapon and set it as the active weapon
        weapon = Instantiate(weapon, transform);
        int randomDirection = Random.Range(0, 2);
        hitWall = randomDirection == 1;
        base.Start();
    }

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
