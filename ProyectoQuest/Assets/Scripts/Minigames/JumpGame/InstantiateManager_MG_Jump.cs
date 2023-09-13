using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstantiateManager_MG_Jump : MonoBehaviour
{
    public GameObject[] platforms;
    public static InstantiateManager_MG_Jump instance;
    public GameObject[] canvas;
    //public Vector3 addPosition = new Vector3(0, 10f, 0);

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        FirstInstantiates(20);
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i].SetActive(false);
        }
        canvas[0].SetActive(true);
        Time.timeScale = 0;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        canvas[0].SetActive(false);
        canvas[1].SetActive(true);
    }

    public void LostGame()
    {
        canvas[1].SetActive(false);
        canvas[2].SetActive(true);
    }

    public void InstantiateNewFloor(Transform newSpawnPosition)
    {
        Vector3 randomYPosition = new Vector3(0, Random.Range(5, 15), 0);
        Vector3 randomInstantite = new Vector3(Random.Range(-4, 4), newSpawnPosition.position.y, newSpawnPosition.position.z);  
        Instantiate(platforms[Random.Range(0, platforms.Length)], randomInstantite + randomYPosition, Quaternion.identity);    
    }

    public void FirstInstantiates(float addDistance)
    {
        Vector3 distance = new Vector3(Random.Range(-4, 4), addDistance, 0);
        Instantiate(platforms[Random.Range(0, platforms.Length)], distance , Quaternion.identity);    
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
        
}
