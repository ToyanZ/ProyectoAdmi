using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShooterGameManager : MonoBehaviour
{
    public static ShooterGameManager instance;
    public Shooter shooter;

    public Image timeBar;
    public float maxTime = 15;
    public float time = 0;

    public GameObject endScreen;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        time = maxTime;
        timeBar.fillAmount = 1;
    }
    bool ending = false;
    private void Update()
    {
        if(time > 0)
        {
            //time -= Time.deltaTime;
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


    public void BackToMainGame()
    {
        GameManager.instance.MiniGameCompleted();
        SceneManager.LoadScene("Level1");
    }
}
