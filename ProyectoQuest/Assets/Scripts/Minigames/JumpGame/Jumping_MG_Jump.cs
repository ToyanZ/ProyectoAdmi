using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Jumping_MG_Jump : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;

    float horizontal;

    public SpriteRenderer[] characters;

    private void Start()
    {
        for(int i = 0; i < characters.Length; i++) 
        {
            characters[i].enabled = false;
        }
        if(GameManager.instance != null) characters[GameManager.instance.characterIndex].enabled = true;
        else characters[0].enabled = true;
    }

    private void Update()
    {

        Movement();
    }

    void Movement()
    {
        //float horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    public void MoveLeft()
    {
        horizontal = -1;
    }

    public void MoveRight()
    {
        horizontal = 1;
    }

    public void StopMove() 
    {
        horizontal = 0;
    }

    
}
