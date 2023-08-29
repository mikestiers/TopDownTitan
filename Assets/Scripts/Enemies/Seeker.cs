using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : Enemy
{
    Vector3 directionToPlayer;

    protected override void Start()
    {
        // Instantiate the starting weapon and set it as the active weapon
        weapon = Instantiate(weapon, transform);
        base.Start();
    }

    void Update()
    {
        if (Time.realtimeSinceStartup >= fireCoolDown)
        {
            fireCoolDown = Time.realtimeSinceStartup + fireCoolDown;
            weapon.Fire(enemyCollider);
        }

        // Calculate the movement direction toward the player (negate it so it moves down the screen)
        directionToPlayer = -(playerTransform.position - transform.position);

        Debug.DrawLine(transform.position, playerTransform.position, Color.red);

        // Normalize the direction to maintain a consistent movement speed
        Vector3 moveDirection = directionToPlayer.normalized;
        moveDirection.z = 0;

        // Move the GameObject at a constant speed
        transform.position -= moveDirection * moveSpeed * Time.deltaTime;
    }
}
