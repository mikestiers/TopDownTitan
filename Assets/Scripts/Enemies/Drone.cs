using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Enemy
{
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
