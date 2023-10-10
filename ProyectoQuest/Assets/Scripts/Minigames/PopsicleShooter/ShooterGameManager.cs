using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShooterGameManager : MonoBehaviour
{
    public static ShooterGameManager instance;
    public Shooter shooter;

    public Image timeBar;
    public TMP_Text timeText;
    public TMP_Text coinsText;
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

    private void Update()
    {
        if(GameManager.instance == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                shooter.Shoot();
            }
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - shooter.center.position;
            shooter.tip.position = (Vector2)shooter.center.position + (direction.normalized * shooter.tipRadius);

            //shooter.rb.velocity = (shooter.walkForce * direction.normalized * Time.deltaTime);
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
                timeText.text = time.ToString("0.00");
            }

            if (time <= 0)
            {
                timeText.text = "0";
                if (!ending)
                {
                    ending = true;
                    coinsText.text = "3";
                    GameManager.instance.playerCoins += 3;
                    GameManager.instance.SaveCoins();
                    endScreen.SetActive(true);
                }
            }
            if (shooter.health <= 0)
            {
                if (!ending)
                {
                    ending = true;
                    coinsText.text = "1";
                    GameManager.instance.playerCoins += 1;
                    GameManager.instance.SaveCoins();
                    endScreen.SetActive(true);
                }
            }

            yield return null;
        }
    }

    public void BackToMainGame()
    {
        GameManager.instance.MiniGameCompleted();
        SceneManager.LoadScene("Level1");
    }
}
