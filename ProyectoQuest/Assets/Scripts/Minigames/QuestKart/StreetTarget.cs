using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class StreetTarget : MonoBehaviour
{

    public GameObject[] streets;
    public GameObject carEnemi;
    public GameObject carPlayer;
    public float playerVelocity;
    private float count;
    private float time;
    public TMP_Text timeText;
    public GameObject panelVictory;
     public int currentStreet;

    public GameObject[] characters;
    private int currentCharacter;
    // Start is called before the first frame update
    void Start()
    {
        currentStreet = 2;
        time = 15f;
        if (GameManager.instance != null) currentCharacter = GameManager.instance.characterIndex;
        else currentCharacter = 0;
            
        characters[currentCharacter].SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = FormatearTiempo();
        if(time <= 0)
        {
            Victory();
        }
        count += Time.deltaTime;
        if(count > 2)
        {
            Invoke("CreateKart", Random.Range(1,3));
            count = 0;
        }
        ChangeStreet();
    }

    public void CreateKart()
    {
        int numberStreet = Random.Range(0, streets.Length);
        GameObject vehicle = Instantiate(carEnemi, new Vector2(15, streets[numberStreet].transform.position.y), quaternion.identity);
        if (numberStreet == 0) vehicle.GetComponent<CarEnemi>().vehicle.sortingOrder = 1;
        if (numberStreet == 1) vehicle.GetComponent<CarEnemi>().vehicle.sortingOrder = 100;
        if (numberStreet == 2) vehicle.GetComponent<CarEnemi>().vehicle.sortingOrder = 150;
    }

    public void AddStreet()
    {
        if(currentStreet < 3)
        {
            currentStreet++;
        }
    }
     public void SubstractStreet()
    {
        if(currentStreet > 1)
        {
            currentStreet--;
        }
    }

    public void ChangeStreet()
    {
        carPlayer.transform.position = Vector2.MoveTowards(carPlayer.transform.position, new Vector2(carPlayer.transform.position.x, streets[currentStreet - 1].transform.position.y),playerVelocity*Time.deltaTime);
        if(currentStreet == 1) characters[currentCharacter].GetComponent<SpriteRenderer>().sortingOrder = 1;
        else if(currentStreet == 2) characters[currentCharacter].GetComponent<SpriteRenderer>().sortingOrder = 100;
        else if(currentStreet == 3) characters[currentCharacter].GetComponent<SpriteRenderer>().sortingOrder = 150;
    }
    private string FormatearTiempo()
    {
        time -= Time.deltaTime;
        int minutos = (int)(time / 60f);
        int segundos = (int)(time - minutos * 60);
        int miliSeg = (int)((time - (int)time) * 100f);
        return segundos.ToString("00") + ":" + miliSeg.ToString("00");
    }
    public void Victory()
    {
        panelVictory.SetActive(true);
        GameManager.instance.MiniGameCompleted();
        SceneManager.LoadScene("Level1");
    }

    void GoToLevel1()
    {

    }

    public void Reset()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
