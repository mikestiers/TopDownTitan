using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juggernaut : Enemy
{
    Vector3 directionToPlayer;
    private Vector3 initialPosition;
    public float hoverRange = 1.0f; // Range of vertical hover motion
    public float hoverSpeed = 1.0f; // Speed of vertical hover motion
    public float moveRange = 2.0f;  // Range of horizontal movement
    //public float moveSpeed = 2.0f;  // Speed of horizontal movement

    protected override void Start()
    {
        initialPosition = transform.position;
        base.Start();
    }

    void Update()
    {
        if (Time.realtimeSinceStartup >= fireCoolDown)
        {
            fireCoolDown = Time.realtimeSinceStartup + fireCoolDown;
            Debug.Log("Fire");
            weapon.Fire(enemyCollider);
        }

        // Calculate the movement direction toward the player (negate it so it moves down the screen)
        //directionToPlayer = -(playerTransform.position - transform.position);

        // Calculate vertical hover motion using Mathf.Sin
        float verticalOffset = Mathf.Sin(Time.time * hoverSpeed) * hoverRange;

        // Calculate horizontal movement using Mathf.PingPong
        float horizontalOffset = Mathf.PingPong(Time.time * moveSpeed, moveRange * 2.0f) - moveRange;

        // Calculate the new position
        Vector3 newPosition = initialPosition + new Vector3(horizontalOffset, verticalOffset, 0f);

        // Update the enemy's position
        transform.position = newPosition;
    }
}
