using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Target target;
    public Joystick joystick;
    public SpriteRenderer[] characters;
    public Animator[] anim;

    private float horizontal;
    private float vertical;
    private bool canMove = true;
    int direction;

    private void Start()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].enabled = false;
        }
        characters[GameManager.instance.characterIndex].enabled = true;
    }

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

        switch (GameManager.instance.buildType)
        {
            case GameManager.BuildType.Pc:
                target.rigidBody.velocity = new Vector2(horizontal, vertical) * target.walkSpeed * Time.fixedDeltaTime;
                break;
            case GameManager.BuildType.Phone:
                target.rigidBody.velocity = joystick.GetDirectionRaw() * target.walkSpeed * Time.fixedDeltaTime;
                break;
        }

        if (target.rigidBody.velocity.x != 0 || target.rigidBody.velocity.y != 0)
        {
            DirectionAnim();
            SetAnimation();
        }
               //Debug.Log("Vertical: " + target.rigidBody.velocity.y);
            //Debug.Log("Horizontal: " + target.rigidBody.velocity.x);
        //bool idle = (horizontal == 0 && vertical == 0) ? true : false;
        anim[GameManager.instance.characterIndex].SetBool("Idle", (target.rigidBody.velocity.x == 0 && target.rigidBody.velocity.y == 0) ? true : false);
        //if (horizontal == 0 && vertical == 0) anim[GameManager.instance.characterIndex].SetBool("Idle", true);
    }

    public void SetMove(bool can)
    {
        canMove = can;
    }
    public bool Moving()
    {
        return canMove;
    }

    public void DirectionAnim()
    {

        if (target.rigidBody.velocity.y > 0)
        {
            if(direction != 1)
            {
                //Debug.Log("1");
                direction = 1;
                anim[GameManager.instance.characterIndex].SetTrigger("Up");
                return;
            }
        }
        else if (target.rigidBody.velocity.y < 0) 
        {
            if (direction != 4)
            {
                //Debug.Log("4");
                direction = 4;
                anim[GameManager.instance.characterIndex].SetTrigger("Down");
                return;
            }
        }
                
        else if (target.rigidBody.velocity.x < 0) 
        {
            if (direction != 2)
            {
                //Debug.Log("2");
                direction = 2;
                anim[GameManager.instance.characterIndex].SetTrigger("Left");
                return;
            }
        }
        else if (target.rigidBody.velocity.x > 0)
        {
            if (direction != 3)
            {
                //Debug.Log("3");
                direction = 3;
                anim[GameManager.instance.characterIndex].SetTrigger("Right");
                return;
            }
        }
    }

    void SetAnimation()
    {

    }

}

