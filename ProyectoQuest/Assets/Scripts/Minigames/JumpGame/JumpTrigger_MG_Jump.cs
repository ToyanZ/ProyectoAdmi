using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class JumpTrigger_MG_Jump : MonoBehaviour
{
    public float upForce;
    public bool isDestroyed;
    public bool disableSpawnPlatform = true;
    public Animator anim;
    public AudioSource audio;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<Rigidbody2D>().velocity.y <= 0)
            {
                JumpUp(collision);
                if (disableSpawnPlatform)
                {
                    SendSpawnNewObject(collision.transform);
                    disableSpawnPlatform = false;
                    if (isDestroyed)
                    {
                        if (anim != null)
                        {
                            anim.SetTrigger("Destroy");
                            audio.Play();
                        }
                        else Destroy(this.gameObject);
                    }
                }
            }
        }
        if (collision.CompareTag("Floor"))
        {
            RespawnFloor();
            Debug.Log("Se redirigio el suelo");
        }
    }

    void JumpUp(Collider2D collision)
    {
        SFX_Manager.instance.PlaySound(0);
        collision.GetComponent<Jumping_MG_Jump>().AnimJump();
        Vector2 impulso = transform.up.normalized * upForce;
        collision.gameObject.GetComponent<Rigidbody2D>().velocity = impulso;
        
    }

    public void SendSpawnNewObject(Transform playerPosition)
    {
        if (disableSpawnPlatform) 
        { 
            InstantiateManager_MG_Jump.instance.InstantiateNewFloor(playerPosition);
        }
    }

    public void RespawnFloor()
    {
        transform.position = new Vector2((Random.Range(-4, 4)), transform.position.y);
    }

    public void DestroyPlatform()
    {
        Destroy(this.gameObject);
    }
}
