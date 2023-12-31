using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KillTrigger_MG_Jump : MonoBehaviour
{

    public Rigidbody2D rb;
    public float speed;
    public bool isLeft;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SFX_Manager.instance.PlaySound(1);
            collision.GetComponent<Jumping_MG_Jump>().LostPlayer();
            InstantiateManager_MG_Jump.instance.LostGame();
        }
    }

    private void FixedUpdate()
    {
        if (isLeft && transform.position.x < -5) isLeft = !isLeft;
        else if(!isLeft && transform.position.x > 4.8) isLeft = !isLeft;
        Movement();
    }

    void Movement()
    {
        float horizontal = isLeft ? -speed : speed;
        rb.velocity = new Vector2(horizontal, rb.velocity.y);
    }
}
