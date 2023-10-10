using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallEnemy : MonoBehaviour
{
    public float health = 4;
    public Rigidbody2D rigidBody;
    public Image healthBar;
    public AnimationCurve animationCurve;

    private void Start()
    {
        StartCoroutine(Follow());
    }

    private void Update()
    {
        healthBar.fillAmount = health / 4;
    }

    IEnumerator Follow()
    {
        while (true)
        {
            Vector2 direction = ShooterGameManager.instance.shooter.center.position - transform.position;
            Vector2 newPosition = (Vector2)transform.position + direction.normalized * ShooterGameManager.instance.enemyMoveAmount;
            Vector3 start = transform.position;
            float time = 0;
            float maxTime = ShooterGameManager.instance.enemyMoveTime;
            while (time < maxTime)
            {
                time += Time.deltaTime;
                transform.position = Vector3.Lerp(start, newPosition, animationCurve.Evaluate(time / maxTime));
                yield return new WaitForSeconds(Time.deltaTime);
            }

            yield return new WaitForSeconds(ShooterGameManager.instance.enemyPauseTime);
        }
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
