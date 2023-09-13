using System.Collections;
using System.Collections.Generic;
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
            JumpUp(collision);
            if (disableSpawnPlatform)
            {
                SendSpawnNewObject(collision.transform);
                disableSpawnPlatform = false;
                if(isDestroyed) Destroy(this.gameObject);
            }
        }
    }

    void JumpUp(Collider2D collision)
    {
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
