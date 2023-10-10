using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Manager : MonoBehaviour
{
    
    public static SFX_Manager instance;
    public AudioSource audio;
    public AudioClip[] sfx;

    public void Awake()
    {
        instance = this;
    }

    public void PlaySound(int id)
    {
        audio.clip = sfx[id];
        audio.Play();
    }
}
