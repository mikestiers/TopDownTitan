using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Enemy
{
    private void Start()
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

        transform.Translate(moveSpeed * Time.deltaTime * downDirection);
    }
}
