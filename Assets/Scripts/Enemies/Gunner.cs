using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : Enemy
{
    bool hitWall = false;
    Vector3 diagonalDirection;

    public override void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hello");
        base.OnTriggerEnter2D(other);
        //if (other.tag == "Wall")
        //{
        //    hitWall = !hitWall;
        //}
    }

    protected override void Start()
    {
        // Instantiate the starting weapon and set it as the active weapon
        weapon = Instantiate(weapon, transform);
        int randomDirection = Random.Range(0, 2);
        hitWall = randomDirection == 1;
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (Time.realtimeSinceStartup >= fireCoolDown)
        {
            fireCoolDown = Time.realtimeSinceStartup + fireCoolDown;
            weapon.Fire(enemyCollider);
        }

        // Check for screen boundaries
        Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPos.x > 1 || screenPos.x < 0)
        {
            hitWall = !hitWall;
        }

        // Calculate the diagonal movement direction
        if (hitWall)
        {
            diagonalDirection = transform.up + (-transform.right);
        }
        else
        {
            diagonalDirection = transform.up + (transform.right);
        }

        // Normalize the direction to maintain a consistent movement speed
        Vector3 moveDirection = diagonalDirection.normalized;

        // Move the GameObject diagonally at a constant speed
        transform.Translate(moveSpeed * Time.deltaTime * moveDirection);
    }
}
