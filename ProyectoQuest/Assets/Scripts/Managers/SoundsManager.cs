using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public AudioClip[] temes;

    private void Awake()
    {
        int numberRandom = Random.Range(0, temes.Length);
        gameObject.GetComponent<AudioSource>().clip = temes[numberRandom];
        gameObject.GetComponent<AudioSource>().Play();
    }

}
