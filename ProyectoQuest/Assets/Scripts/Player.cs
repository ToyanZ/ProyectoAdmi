using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Target target;
    
    
    private float horizontal;
    private float vertical;
    private bool canMove = true;

    private void Update()
    {
        if (!canMove) return;
        target.rigidBody.gravityScale = target.gravityScale;
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }
    private void FixedUpdate()
    {
        if (!canMove)
        {
            target.rigidBody.velocity = Vector2.zero;
            return;
        }
        target.rigidBody.velocity = new Vector2(horizontal, vertical) * target.walkSpeed * Time.fixedDeltaTime;
    }

    public void SetMove(bool can)
    {
        canMove = can;
    }
    public bool Moving()
    {
        return canMove;
    }
}
