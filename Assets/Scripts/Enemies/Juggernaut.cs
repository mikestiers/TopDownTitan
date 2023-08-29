using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juggernaut : Enemy
{
    Vector3 directionToPlayer;
    private Vector3 initialPosition;
    public float hoverRange = 0.25f; // Range of vertical hover motion
    public float hoverSpeed = 0.25f; // Speed of vertical hover motion
    public float moveRange = 2.0f;  // Range of horizontal movement

    protected override void Start()
    {
        // Instantiate the starting weapon and set it as the active weapon
        weapon = Instantiate(weapon, transform);
        base.Start();
        initialPosition = new Vector3(transform.position.x, transform.position.y - hoverRange * 5, 0);
    }

    void Update()
    {
        if (Time.realtimeSinceStartup >= fireCoolDown)
        {
            fireCoolDown = Time.realtimeSinceStartup + fireCoolDown;
            weapon.Fire(enemyCollider);
        }

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
