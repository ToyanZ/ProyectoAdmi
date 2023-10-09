using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : MonoBehaviour
{
    public int id = 0;
    public Tilemap tilemap;
    public Transform worldPosition;
    public bool unlocked = false;


    public void Open()
    {
        StartCoroutine(OpenIE());
    }
    IEnumerator OpenIE()
    {
        float time = 0;
        float maxTime = GameManager.instance.doorsOpenTime;
        Instantiate(GameManager.instance.dustParticleEffect, worldPosition.position, Quaternion.identity);
        while (time < maxTime)
        {
            time += Time.deltaTime;
            Color color = tilemap.color;
            tilemap.color = new Color(color.r, color.g, color.b, 1 - (time / maxTime));
            yield return new WaitForSeconds(Time.deltaTime);
        }
        unlocked = true;
        gameObject.SetActive(false);
    }
}
