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
    [HideInInspector] int currentStreet;
    [HideInInspector] private int currentStreetEnemi;
    [HideInInspector] private float vertical;
    public GameObject panelTutorial;
    public GameObject panelUI;
    [HideInInspector] public bool spawnEnemi = false;
    public GameObject panelLoseTry;
    public GameObject panelMinigameComplete;
    public TMP_Text coinsCount;
    public TMP_Text triesCount;

    public SpriteRenderer[] characters;
    private int currentCharacter;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.miniGameTutorial == true)
        {
            spawnEnemi = false;
            panelTutorial.SetActive(true);
        }
        else
        {
            spawnEnemi=true;
            panelTutorial.SetActive(false);
            panelUI.SetActive(true);
        }
        currentStreet = 2;
        time = 15f;
        if (GameManager.instance != null) currentCharacter = GameManager.instance.characterIndex;
        else currentCharacter = 0;
            
        //Debug.Log("El personaje es " + GameManager.instance.characterIndex);
        characters[currentCharacter].enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnEnemi)
        {
            carPlayer.SetActive(true);
            timeText.text = FormatearTiempo();
            if (time <= 0)
            {
                Victory(3);
            }
            count += Time.deltaTime;
            if (count > 1)
            {
                Invoke("CreateKart", Random.Range(1, 3));
                count = 0;
            }
            ChangeStreet();
            if (Input.GetButtonDown("Vertical"))
            {
                vertical = Input.GetAxisRaw("Vertical");
            }
        }
        else
        {
            carPlayer.SetActive(false);
        }
        
        
    }
    private void FixedUpdate()
    {
        if (vertical == 1){
            SubstractStreet(); 
            vertical = 0;
        }
        else if (vertical == -1){
            AddStreet();
            vertical = 0;
        }
    }

    public void TutorialScreen()
    {
        panelTutorial.SetActive(false);
        panelUI.SetActive(true);
        spawnEnemi = true;
    }
    public void CreateKart()
    {
        if (spawnEnemi)
        {
            int numberStreet = Random.Range(0, streets.Length);
            while (numberStreet == currentStreetEnemi)
            {
                numberStreet = Random.Range(0, streets.Length);
            }
            currentStreetEnemi = numberStreet;
            GameObject vehicle = Instantiate(carEnemi, new Vector2(15, streets[numberStreet].transform.position.y), quaternion.identity);
            if (numberStreet == 0) vehicle.GetComponent<CarEnemi>().vehicle.sortingOrder = 1;

            if (numberStreet == 1) vehicle.GetComponent<CarEnemi>().vehicle.sortingOrder = 100;

            if (numberStreet == 2) vehicle.GetComponent<CarEnemi>().vehicle.sortingOrder = 150;
        }
        
       
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
        anim.SetBool("Walking", carPlayer.transform.position.y == streets[currentStreet - 1].transform.position.y ? false : true);
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
    public void Victory(int numberCoint)
    {
        panelUI.SetActive(false);
        spawnEnemi = false;
        ResetToWorld(numberCoint);
        GameManager.instance.MiniGameCompleted();
    }
    public void ResetToWorld(int numberCoint)
    {
        panelMinigameComplete.SetActive(true);
        coinsCount.text = "X " + numberCoint.ToString();
        GameManager.instance.playerCoins += numberCoint;
        GameManager.instance.minigamesTry = 3;
        GameManager.instance.SaveCoins();

    }
    public void LoadWorldScene()
    {
        GameManager.instance.miniGameTutorial = true;
        SceneManager.LoadScene("Level1");
    }

    public void LoseTry()
    {
        GameManager.instance.miniGameTutorial = false;
        spawnEnemi = false;
        GameManager.instance.minigamesTry--;
        triesCount.text ="X " + GameManager.instance.minigamesTry.ToString();
        panelUI.SetActive(false);
        panelLoseTry.SetActive(true);
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
