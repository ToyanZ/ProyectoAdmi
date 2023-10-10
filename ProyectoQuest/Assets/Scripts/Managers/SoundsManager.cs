using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundsManager : MonoBehaviour
{
    public AudioClip[] temes;
    private bool isForm;

    private void Awake()
    {
        
        
    }
    private void Start()
    {
        gameObject.GetComponent<AudioSource>().volume = 0.2f;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Gachapon")
        {
            gameObject.GetComponent<AudioSource>().volume = 0.05f;
        }
        else
        {
            gameObject.GetComponent<AudioSource>().volume = 0.2f;
        }
        if (GameManager.instance.menuState == GameManager.MenuState.SignIn)
        {
            gameObject.GetComponent<AudioSource>().clip = temes[temes.Length - 1];
            if (!gameObject.GetComponent<AudioSource>().isPlaying)
            {
                gameObject.GetComponent<AudioSource>().Play();
            }
            isForm = true;
        }
        else if(GameManager.instance.menuState == GameManager.MenuState.MainMenu)
        {
            if (isForm)
            {
                gameObject.GetComponent<AudioSource>().Stop();
                isForm = false;
            }
            if (!gameObject.GetComponent<AudioSource>().isPlaying)
            {
                SelectASong();
            }
        }
        
    }

    private void SelectASong()
    {
        int numberRandom = Random.Range(0, temes.Length-1);
        gameObject.GetComponent<AudioSource>().clip = temes[numberRandom];
        gameObject.GetComponent<AudioSource>().Play();
    }
}
