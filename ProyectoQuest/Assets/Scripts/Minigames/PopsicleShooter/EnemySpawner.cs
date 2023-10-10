using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform center;
    
    public BallEnemy ballEnemy;
    float time = 0;
    private void Update()
    {
        if(ShooterGameManager.instance.time > 0 && ShooterGameManager.instance.shooter.health > 0)
        {
            time += Time.deltaTime;
            
            if(time >= ShooterGameManager.instance.enemySpawntime)
            {
                Vector2 randomPosition = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
                Vector2 position = (Vector2)center.position + (randomPosition.normalized * ShooterGameManager.instance.enemySpawnRadius);
                Instantiate(ballEnemy, position, Quaternion.identity);
                time = 0;
            }
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if(ShooterGameManager.instance!=null)
        {
            Gizmos.DrawWireSphere(center.position, ShooterGameManager.instance.enemySpawnRadius);
        }
    }
}
