using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallEnemy : MonoBehaviour
{
    public float health = 4;
    public Rigidbody2D rigidBody;
    public Image healthBar;

    private void Update()
    {
        healthBar.fillAmount = health / 4;
    }

    public void TakeDamage(bool normal)
    {
        if (normal)
        {
            health -= 1;
            ShooterGameManager.instance.shooter.AddPoint(0.5f);

            if (health <= 0)
            {
                ShooterGameManager.instance.shooter.AddPoint(2);
                Destroy(gameObject);
            }
        }
        else
        {
            health -= 2;
            if (health <= 0) Destroy(gameObject);
        }
        
    }
}
