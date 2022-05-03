using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Autor: Joan Daniel Guerrero Garcia
 * 
 * Description:
 * A script that manages horizontal and vertical movement of a player.
 * Can move with WASD and/or with the arrow keys.
 * 
 * Requirements:
 * - GameObject Player with Animator, RigidBody2D and BoxCollider2D
 */

public class PlayerMovementTopDown : MonoBehaviour
{
    public static bool facingRight = true;
    public float movementSpeed = 5;

    private float horizontal;
    private float vertical;

    Rigidbody2D rigidbody2d;

    //---------------------------------------ADD WHEN NEEDED
    //Animator animator;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        //---------------------------------------ADD WHEN NEEDED
        //animator = GetComponent<Animator>();
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        //---------------------------------------ADD WHEN NEEDED
        //animator.SetFloat("velocity", Mathf.Abs(rigidbody2d.velocity.x + rigidbody2d.velocity.y));
    }

    void FixedUpdate()
    {
        rigidbody2d.velocity = new Vector2(horizontal * movementSpeed, vertical * movementSpeed);

        if ((horizontal > 0.0f && !facingRight) || (horizontal < 0.0f && facingRight))
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(Vector3.up, 180.0f, Space.World);
    }
}
