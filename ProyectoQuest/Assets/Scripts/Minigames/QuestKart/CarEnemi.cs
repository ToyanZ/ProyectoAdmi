using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarEnemi : MonoBehaviour
{
    public float carVelocity;
    public GameObject streetTarget;
    public GameObject panelMinigameComplete;

    public SpriteRenderer vehicle;
    public Sprite[] vehicles;

    public GameObject particles;
    // Start is called before the first frame update
    void Start()
    {
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
        Instantiate(particles, collision.transform.position, Quaternion.identity);
        collision.gameObject.SetActive(false);
        /*if (GameManager.instance.minigamesTry == 0)
        {
            streetTarget.GetComponent<StreetTarget>().ResetToWorld(1);
        }
        else
        {
            streetTarget.GetComponent<StreetTarget>().LoseTry();
        }*/
        streetTarget.GetComponent<StreetTarget>().LoseTry();
    }

    public void ResetToWorld(int numberCoint)
    {
        GameManager.instance.minigamesTry = 3;

    }
}
