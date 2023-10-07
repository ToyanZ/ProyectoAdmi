using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstantiateManager_MG_Jump : MonoBehaviour
{
    public GameObject[] platforms;
    public GameObject[] enemies;
    public GameObject[] canvas;
    public TMP_Text coinsCount;
    public TMP_Text triesCount;
    public TMP_Text timeText;
    private float time;
    public GameObject player;
    public static InstantiateManager_MG_Jump instance;
    //public Vector3 addPosition = new Vector3(0, 10f, 0);

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i].SetActive(false);
        }
        canvas[0].SetActive(true);
        player.SetActive(false);
        int randomDistance = 10;
        for (int i = 0;i < 5; i++) 
        {
            randomDistance = Random.Range(randomDistance, randomDistance + 20);
            FirstInstantiates(randomDistance);
        }
        time = 15f;
    }

    private void Update()
    {
        timeText.text = FormatearTiempo();
        if(time <= 0)
        {
            ResetToWorld(2);
        }
    }

    public void StartGame()
    {
        player.SetActive(true);
        canvas[0].SetActive(false);
        canvas[1].SetActive(true);
    }

    public void LostGame()
    {
        if(GameManager.instance.minigamesTry == 0)
        {
            ResetToWorld(1);
        }
        else
        {
            time = 15;
            triesCount.text = GameManager.instance.minigamesTry.ToString()  ;
            canvas[1].SetActive(false);
            canvas[2].SetActive(true);
        }
    }
    private string FormatearTiempo()
    {
        time -= Time.deltaTime;
        int minutos = (int)(time / 60f);
        int segundos = (int)(time - minutos * 60);
        int miliSeg = (int)((time - (int)time) * 100f);
        return segundos.ToString("00") + ":" + miliSeg.ToString("00");
    }
    public void ResetToWorld(int numberCoint)
    {
        canvas[1].SetActive(false);
        canvas[3].SetActive(true);
        coinsCount.text = "X " + numberCoint.ToString();
        GameManager.instance.playerCoins += numberCoint;
        GameManager.instance.minigamesTry = 3;
    }

    public void InstantiateNewFloor(Transform newSpawnPosition)
    {
        Vector3 randomYPosition = new Vector3(0, Random.Range(8, 15), 0);
        Vector3 randomInstantite = new Vector3(Random.Range(-4, 4), newSpawnPosition.position.y, newSpawnPosition.position.z);  
        Instantiate(platforms[Random.Range(0, platforms.Length)], randomInstantite + randomYPosition, Quaternion.identity);
    }
    public void InstantiateNewFloorDestroyZone(Transform newSpawnPosition)
    {
        Vector3 randomYPosition = new Vector3(0, Random.Range(15, 20), 0);
        Vector3 randomInstantite = new Vector3(Random.Range(-4, 4), newSpawnPosition.position.y, newSpawnPosition.position.z);
        Instantiate(platforms[Random.Range(0, platforms.Length)], randomInstantite + randomYPosition, Quaternion.identity);
    }

    public void FirstInstantiates(float addDistance)
    {
        Vector3 distance = new Vector3(Random.Range(-10, 10), addDistance, 0);
        Instantiate(enemies[Random.Range(0, platforms.Length)], distance , Quaternion.identity);    
    }

    public void LoadScene()
    {
        GameManager.instance.MiniGameCompleted();
        SceneManager.LoadScene("Level1");
    }

    public void ReloadScene()
    {
        GameManager.instance.minigamesTry--;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void InstantiateNewEnemy(Transform newSpawnPosition)
    {
        Vector3 randomYPosition = new Vector3(0, Random.Range(15, 40), 0);
        Vector3 randomInstantite = new Vector3(Random.Range(-4, 4), newSpawnPosition.position.y, newSpawnPosition.position.z);
        Instantiate(enemies[Random.Range(0, platforms.Length)], randomInstantite + randomYPosition, Quaternion.identity);
    }
        
}
