using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestionHandler : MonoBehaviour
{
    public List<Question> questions;
    public Image barImage;
    public GameObject focusPoint;
    public UnityEvent OnGroupCompleted;
    public bool gamePlayed = false;
    public BoxCollider2D boxCollider2D;
    bool updating=false;
    public void ProgressUpdate()
    {
        if(!updating)StartCoroutine(Progress());
    }
    IEnumerator Progress()
    {
        updating = true;
        float count = 0;
        foreach (Question question in questions)
        {
            if(question.answered) count++;
        }
        float startPct = barImage.fillAmount;
        float endPct = count / questions.Count * 1.0f;

        float maxTime = GameManager.instance.questionHandlerBarUpdateTime;
        float time = 0;
        while (time < maxTime)
        {
            time += Time.deltaTime;
            
            barImage.fillAmount = Mathf.Lerp(startPct, endPct,  (time / maxTime));
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if(count == questions.Count) OnGroupCompleted?.Invoke();
        updating = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Character>() != null)
        {
            GameManager.instance.SetCurrentArea(this);
        }
    }

}
