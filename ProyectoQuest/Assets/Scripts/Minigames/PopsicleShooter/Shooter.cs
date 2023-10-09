using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Shooter : MonoBehaviour
{
    public float health = 3;
    public Image healthBar;

    [Space(20)]
    public Transform tip;
    public Transform center;
    public Projectile projectile;
    public float shootForce = 40;

    [Space(20)]
    public float powerPointsMax = 24;
    public float powerPoints = 0;
    public Image powerBar;
    public Button powerButton;
    public GameObject powerParticles;
    public float powerRadius = 3;

    private void Start()
    {
        healthBar.fillAmount = 1;
    }

    private void Update()
    {
        powerBar.fillAmount = powerPoints / powerPointsMax;
        powerButton.interactable = powerPoints == powerPointsMax;

        //transform.up = GameManager.instance.character.player.joystick.GetDirectionRaw();
        //GameManager.instance.character.player.target.rigidBody.AddForce(50 * transform.up * Time.deltaTime);
    }

    public void TakeDamage()
    {
        health -= 1;

        if (health < 0) health = 0;
        healthBar.fillAmount = health / 3;
    }

    public void Shoot()
    {
        Projectile bullet = Instantiate(projectile, tip.position, Quaternion.identity);
        
        Vector2 direction = tip.position - center.position;
        
        bullet.rigidBody.velocity = direction.normalized * shootForce;
    }

    public void AddPoint(float value)
    {
        powerPoints += value;
        if (powerPoints > powerPointsMax)
        {
            powerPoints = powerPointsMax;
        }
    }

    public void Power()
    {
        Instantiate(powerParticles, center.position, Quaternion.identity);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(center.position, powerRadius);
        foreach (Collider2D collider in colliders)
        {
            BallEnemy current = null;
            if (collider.TryGetComponent<BallEnemy>(out current))
            {
                current.TakeDamage(false);
                Vector2 direction= collider.transform.position - center.position;
                current.rigidBody.AddForce(direction.normalized * 20);
            }
        }
        powerPoints = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(center.position, powerRadius);
    }

}
