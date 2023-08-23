using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public float walkSpeed = 60;
    public float gravityScale = 0;
    
    
    private float horizontal;
    private float vertical;

    private void Update()
    {
        rigidBody.gravityScale = gravityScale;
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }
    private void FixedUpdate()
    {
        rigidBody.velocity = new Vector2(horizontal, vertical) * walkSpeed * Time.fixedDeltaTime;
    }
}
