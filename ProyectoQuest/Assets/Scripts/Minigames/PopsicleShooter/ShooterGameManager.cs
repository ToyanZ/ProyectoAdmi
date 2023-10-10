using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShooterGameManager : MonoBehaviour
{
    public static ShooterGameManager instance;
    public Shooter shooter;

    public Image timeBar;
    public float maxTime = 15;
    [HideInInspector] public float time = 0;
    public float enemyMoveAmount = 2;
    public float enemyMoveTime = 0.5f;
    public float enemyPauseTime = 0.25f;
    public float enemySpawnRadius = 6;
    public float enemySpawntime = 0.5f;

    public GameObject endScreen;
    bool ending = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void StartGame()
    {
        StartCoroutine(StartIE());
    }
    IEnumerator StartIE()
    {
        time = maxTime;
        timeBar.fillAmount = 1;
        if (InterfaceManager.instance != null)
        {
            InterfaceManager.instance.joystick.SetActive(true);
        }

        while (true)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                timeBar.fillAmount = time / maxTime;
            }

            if (time <= 0)
            {
                if (!ending)
                {
                    ending = true;
                    endScreen.SetActive(true);
                }
            }
            if (shooter.health <= 0)
            {
                if (!ending)
                {
                    ending = true;
                    endScreen.SetActive(true);
                }
            }
        }
    }

    public void BackToMainGame()
    {
        GameManager.instance.MiniGameCompleted();
        SceneManager.LoadScene("Level1");
    }
}
