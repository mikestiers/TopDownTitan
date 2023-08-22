using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interceptor : Enemy
{
    Vector3 directionToPlayer;
    public float avoidanceSpeed = 20.0f;
    public float avoidanceDistance = 2.0f;
    public LayerMask bulletLayer;
    private Vector3 avoidanceDirection;

    private bool isAvoiding = false;

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

        // Check for incoming bullets
        RaycastHit hit;
        Vector3 rayDirection = transform.up * -1; // Ray direction is opposite of the enemy's up direction
        if (Physics.Raycast(transform.position, rayDirection, out hit, avoidanceDistance, bulletLayer))
        {
            // Avoid the bullet
            isAvoiding = true;
            avoidanceDirection = Vector3.Cross(rayDirection, Vector3.forward).normalized;
        }
        else
        {
            // No bullets detected, stop avoiding
            isAvoiding = false;
        }

        // Move the enemy
        if (isAvoiding)
        {
            // Move in the avoidance direction
            transform.Translate(avoidanceSpeed * Time.deltaTime * avoidanceDirection);
        }
        else
        {
            // Move down the screen
            transform.Translate(moveSpeed * Time.deltaTime * downDirection);
        }
    }
}
