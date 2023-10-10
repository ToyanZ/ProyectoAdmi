using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundsManager : MonoBehaviour
{
    public AudioClip[] temes;

    private void Awake()
    {
        gameObject.GetComponent<AudioSource>().volume = 0.1f;
        SelectASong();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Gachapon")
        {
            gameObject.GetComponent<AudioSource>().volume = 0.05f;
        }
        else
        {
            gameObject.GetComponent<AudioSource>().volume = 0.1f;
        }
        if (!gameObject.GetComponent<AudioSource>().isPlaying)
        {
            SelectASong();
        }
    }

    private void SelectASong()
    {
        int numberRandom = Random.Range(0, temes.Length-1);
        gameObject.GetComponent<AudioSource>().clip = temes[numberRandom];
        gameObject.GetComponent<AudioSource>().Play();
    }
}
