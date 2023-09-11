using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionHandler : MonoBehaviour
{
    public List<Question> questions;
    public Image barImage;
    public void ProgressUpdate()
    {
        StartCoroutine(Progress());
    }
    IEnumerator Progress()
    {
        float count = 0;
        foreach (Question question in questions)
        {
            if(question.answered) count++;
        }
        float startPct = barImage.fillAmount;
        float endPct = count / questions.Count * 1.0f;

        float maxTime = GameManager.instance.answerCompletedBarUpdateTime;
        float time = maxTime;
        while (time > 0)
        {
            time -= Time.deltaTime;
            
            barImage.fillAmount = Mathf.Lerp(startPct, endPct, 1 - (time / maxTime));
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
