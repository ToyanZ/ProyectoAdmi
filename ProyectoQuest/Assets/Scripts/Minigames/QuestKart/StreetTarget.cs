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
    public GameObject street1;
    public GameObject street2;
    public GameObject street3;
    public GameObject carEnemi;
    public GameObject carPlayer;
    public float playerVelocity;
    private float count;
    private float time;
    public TMP_Text timeText;
    public GameObject panelVictory;
    [HideInInspector] public int currentStreet;
    // Start is called before the first frame update
    void Start()
    {
        currentStreet = 2;
        time = 15f;
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
        int numberStreet = Random.Range(1,4);
        if(numberStreet == 1)
        {
            Instantiate(carEnemi, new Vector2(15, street1.transform.position.y), Quaternion.identity);
        }else if(numberStreet == 2)
        {
            Instantiate(carEnemi, new Vector2(15, street2.transform.position.y), Quaternion.identity);
        }
        else if(numberStreet == 3)
        {
            Instantiate(carEnemi, new Vector2(15, street3.transform.position.y), Quaternion.identity);
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
        if(currentStreet == 1)
        {
            carPlayer.transform.position = Vector2.MoveTowards(carPlayer.transform.position, new Vector2(carPlayer.transform.position.x,street1.transform.position.y),playerVelocity*Time.deltaTime);
        }else if(currentStreet == 2)
        {
            carPlayer.transform.position = Vector2.MoveTowards(carPlayer.transform.position, new Vector2(carPlayer.transform.position.x, street2.transform.position.y), playerVelocity * Time.deltaTime);
        }
        else if(currentStreet == 3)
        {
            carPlayer.transform.position = Vector2.MoveTowards(carPlayer.transform.position, new Vector2(carPlayer.transform.position.x, street3.transform.position.y), playerVelocity * Time.deltaTime);
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
    public void Victory()
    {
        panelVictory.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Reset()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
