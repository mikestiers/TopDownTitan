using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
