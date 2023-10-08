using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using UnityEngine.XR;

public class Zone : MonoBehaviour
{
    public bool gamePlayed = false;
    public int id = 1;
    [Space(20)]
    public Bar progressBar;
    public GameObject cameraPosition;
    public GameObject cameraIndicativePosition;
    public BoxCollider2D boxCollider2D;
    public List<Question> questions;

    
    [SerializeField] bool updating = false;
    [SerializeField] bool open = false;
    public void ProgressUpdate()
    {
        if (!updating) StartCoroutine(Progress());
    }
    IEnumerator Progress()
    {
        updating = true;
        
        float count = 0;
        foreach (Question question in questions)
        {
            if(question.answered) count++;
        }
        float startPct = progressBar.filler.fillAmount;
        float endPct = count / questions.Count * 1.0f;

        float maxTime = GameManager.instance.questionHandlerBarUpdateTime;
        float time = 0;
        while (time < maxTime)
        {
            time += Time.deltaTime;
            progressBar.filler.fillAmount = Mathf.Lerp(startPct, endPct,  (time / maxTime));
            yield return new WaitForSeconds(Time.deltaTime);
        }

        updating = false;
    }

    public void Open()
    {
        if (!open) StartCoroutine(OpenDoor());
    }
    IEnumerator OpenDoor()
    {
        open = true;
        
        Door current = null;
        while(current == null)
        {
            List<Door> doors = FindObjectsOfType<Door>().ToList();
            foreach (Door door in doors)
            {
                if (door.id == id) current = door;
            }
            yield return null;
        }

        GameManager.instance.character.gameObject.SetActive(true);
        
        GameManager.instance.cameraTracking.StopAllCoroutines();
        GameManager.instance.cameraTracking.GoTo(cameraIndicativePosition);


        float time = 0;
        float maxTime = GameManager.instance.doorsOpenTime;
        Instantiate(GameManager.instance.dustParticleEffect, current.worldPosition.position, Quaternion.identity);
        while (time < maxTime)
        {
            time += Time.deltaTime;
            Color color = current.tilemap.color;
            current.tilemap.color = new Color(color.r, color.g, color.b, 1 - (time / maxTime));
            yield return new WaitForSeconds(Time.deltaTime);
        }
        current.gameObject.SetActive(false);

        InterfaceManager.instance.ShowMainGameUI();


        open = false;
        yield return null;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Character>() != null)
        {
            GameManager.instance.SetCurrentArea(this);
        }
    }

}
