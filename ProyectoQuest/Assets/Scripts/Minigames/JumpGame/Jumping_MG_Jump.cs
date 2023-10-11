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

    [SerializeField] GameObject particles;

    public Animator anim;
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
        if (GameManager.instance != null)
        {
            if(GameManager.instance.buildType == GameManager.BuildType.Phone) Movement();
            else
            {
                Movement();
                if (Input.GetAxisRaw("Horizontal") == -1) MoveLeft();
                else if (Input.GetAxisRaw("Horizontal") == 1) MoveRight();
                else StopMove();
                Movement();
            }


        }
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

    public void LostPlayer()
    {
        Instantiate(particles, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public void AnimJump()
    {
        anim.SetTrigger("Jump");
    }
    
}
