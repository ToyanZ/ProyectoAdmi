using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarEnemi : MonoBehaviour
{
    public GameObject panelLose;
    public float carVelocity;
    public GameObject streetTarget;

    public SpriteRenderer vehicle;
    public Sprite[] vehicles;
    // Start is called before the first frame update
    void Start()
    {
        panelLose = GameObject.FindGameObjectWithTag("Lose");
        streetTarget = GameObject.FindGameObjectWithTag("Target");
        vehicle.sprite = vehicles[Random.Range(0, vehicles.Length)];
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * carVelocity * Time.deltaTime);
        StartCoroutine(CarDestroy());
    }

    IEnumerator CarDestroy()
    {
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.SetActive(false);
        panelLose.transform.GetChild(0).gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
}
