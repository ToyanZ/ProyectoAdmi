using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Target target;
    
    
    private float horizontal;
    private float vertical;

    private void Update()
    {
        target.rigidBody.gravityScale = target.gravityScale;
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }
    private void FixedUpdate()
    {
        target.rigidBody.velocity = new Vector2(horizontal, vertical) * target.walkSpeed * Time.fixedDeltaTime;
    }
}
