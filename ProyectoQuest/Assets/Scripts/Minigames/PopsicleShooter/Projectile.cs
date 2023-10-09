using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D rigidBody;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Shooter shooter = collision.GetComponent<Shooter>();
        BallEnemy ballEnemy = collision.GetComponent<BallEnemy>();

        if(shooter != null)
        {
            shooter.TakeDamage();
        }
        else if (ballEnemy != null)
        {
            ballEnemy.TakeDamage(true);
        }
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
