using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public float moveSpeed = 5f;
    public Rigidbody rb;
    public Weapon weapon;
    public BoxCollider playerCollider;

    Vector3 moveDirection;

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0))
        {
            weapon.Fire(playerCollider);
        }

        moveDirection = new Vector3(moveX, moveY, 0).normalized;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed, 0);
    }
}
