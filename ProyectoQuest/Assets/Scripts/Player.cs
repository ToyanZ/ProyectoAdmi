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
    bool lockTrigger;

    private void Start()
    {
        ChangeNewCharacter();
    }

    private void OnEnable()
    {
        //ChangeNewCharacter();
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
        }
        //Debug.Log("Vertical: " + target.rigidBody.velocity.y);
        //Debug.Log("Horizontal: " + target.rigidBody.velocity.x);
        //bool idle = (horizontal == 0 && vertical == 0) ? true : false;
        float yVel = target.rigidBody.velocity.y;
        float xVel = target.rigidBody.velocity.x;
        anim[GameManager.instance.characterIndex].SetBool("Idle", (xVel == 0 && yVel == 0) ? true : false);
        direction =  (xVel == 0 && yVel == 0) ? 0 : direction;


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
        float yVel = target.rigidBody.velocity.y;
        float xVel = target.rigidBody.velocity.x;
        if ( yVel > 0)
        {
            if (xVel > 0 && yVel > xVel) UpAnim();
            else if (xVel > 0 && yVel < xVel) RightAnim();
            else if (xVel < 0 && yVel < (xVel * -1)) LeftAnim();
        }
        else if(yVel < 0)
        {
            if (xVel > 0 && (yVel * -1) > xVel) DownAnim();
            else if (xVel > 0 && (yVel * -1) < xVel) RightAnim();
            else if (xVel < 0 && (yVel * -1) < (xVel * -1)) LeftAnim();
        }

    }

    public void UpAnim()
    {
        if (direction != 1)
        {
            direction = 1;
            anim[GameManager.instance.characterIndex].SetTrigger("Up");
        }
    }

    public void DownAnim()
    {
        if (direction != 1)
        {
            direction = 1;
            anim[GameManager.instance.characterIndex].SetTrigger("Down");
        }
    }

    public void RightAnim()
    {
        if (direction != 2)
        {
            direction = 2;
            anim[GameManager.instance.characterIndex].SetTrigger("Right");
        }
    }

    public void LeftAnim()
    {
        if (direction != 3)
        {
            direction = 3;
            anim[GameManager.instance.characterIndex].SetTrigger("Left");
        }
    }

    public void ChangeNewCharacter()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].enabled = false;
        }
        characters[GameManager.instance.characterIndex].enabled = true;
    }

}

