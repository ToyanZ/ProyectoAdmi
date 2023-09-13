using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger_MG_Jump : MonoBehaviour
{
    public float upForce;
    public bool isDestroyed;

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Hace colision");
            JumpUp(collision);
            if (isDestroyed)
            {
                InstantiateManager_MG_Jump.instance.InstantiateNewFloor(collision.transform);
                Destroy(this.gameObject);
            }
        }
    }

    void JumpUp(Collider2D collision)
    {
        Vector2 impulso = transform.up.normalized * upForce;

        collision.gameObject.GetComponent<Rigidbody2D>().velocity = impulso;
        
    }
}
