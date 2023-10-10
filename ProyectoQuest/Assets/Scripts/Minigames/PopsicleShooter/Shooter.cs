using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Shooter : MonoBehaviour
{
    public float maxHealth = 6;
    public float health = 3;
    public Image healthBar;
    public Rigidbody2D rb;

    [Space(20)]
    public Transform tip;
    public Transform center;
    public Projectile projectile;
    public float shootForce = 40;
    public float walkForce = 10;

    [Space(20)]
    public float powerPointsMax = 24;
    public float powerPoints = 0;
    public Image powerBar;
    public Button powerButton;
    public GameObject powerParticles;
    public float powerRadius = 3;
    public float powerForce = 20;

    public SpriteRenderer[] characters;

    public AudioSource[] sfx;
    
    [HideInInspector] public float tipRadius = 0;
    private void Start()
    {
        health = maxHealth;
        healthBar.fillAmount = 1;
        tipRadius = (tip.position - center.position).magnitude;

        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].enabled = false;
        }
        if (GameManager.instance != null) characters[GameManager.instance.characterIndex].enabled = true;
        else characters[0].enabled = true;
    }

    private void Update()
    {
        powerBar.fillAmount = powerPoints / powerPointsMax;
        powerButton.interactable = powerPoints == powerPointsMax;

        if(GameManager.instance != null )
        {
            Vector2 direction = GameManager.instance.character.player.joystick.GetDirectionRaw();

            if( direction != Vector2.zero )
            {
                tip.position = (Vector2)center.position + (direction.normalized * tipRadius);

                //rb.velocity = (walkForce * direction.normalized * Time.deltaTime);
            }
        }
    }

    public void TakeHealing()
    {
        health += 0.1f;
        if (health >= maxHealth) health = maxHealth;
        healthBar.fillAmount = health / maxHealth;
    }

    public void TakeDamage()
    {
        sfx[1] .Play();
        health -= 1;

        if (health < 0) health = 0;
        healthBar.fillAmount = health / maxHealth;
    }

    public void Shoot()
    {
        sfx[0].Play();
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
                current.rigidBody.AddForce(direction.normalized * powerForce, ForceMode2D.Impulse);
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
