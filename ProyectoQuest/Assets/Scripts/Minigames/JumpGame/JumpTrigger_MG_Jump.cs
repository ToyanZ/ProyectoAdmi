using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class JumpTrigger_MG_Jump : MonoBehaviour
{
    public float upForce;
    public bool isDestroyed;
    public bool disableSpawnPlatform = true;
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
                    if (isDestroyed) Destroy(this.gameObject);
                }
            }
        }
        if (collision.CompareTag("Floor"))
        {
            if (Random.Range(0, 100) > 50)
            {
                Debug.Log("Se redirigio el suelo");
                SendSpawnNewObject(this.transform);
                Destroy(this.gameObject);
            }
        }
    }

    void JumpUp(Collider2D collision)
    {
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
}
