using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public List<SpriteRenderer> healthPointHud;
    public Transform body;
    public int healthPoint = 3;

    public bool TakeDamage()
    {
        healthPoint -= 1;
        body.localScale = Vector3.one * 0.33f * healthPoint;

        for (int i=0; i<healthPointHud.Count; i++)
        {
            healthPointHud[i].enabled = i < healthPoint;
        }

        return healthPoint <= 0;
    }
}
