using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : Enemy
{
    Vector3 directionToPlayer;

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup >= fireCoolDown)
        {
            fireCoolDown = Time.realtimeSinceStartup + fireCoolDown;
            weapon.Fire(enemyCollider);
        }

        // Calculate the movement direction toward the player (negate it so it moves down the screen)
        directionToPlayer = -(playerTransform.position - transform.position);

        // Normalize the direction to maintain a consistent movement speed
        Vector3 moveDirection = directionToPlayer.normalized;

        // Move the GameObject at a constant speed
        transform.Translate(moveSpeed * Time.deltaTime * moveDirection);
    }
}
