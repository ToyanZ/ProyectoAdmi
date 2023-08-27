using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour
{
    //private float countDown = 0;
    public string statement = "";
    public List<Answer> answers;
    
    /*
    private void Start()
    {
        countDown = GameManager.instance.questionTriggerTime;
    }
    public void StartCounting()
    {
        countDown = GameManager.instance.questionTriggerTime;
    }
    public void CountDown()
    {
        countDown -= Time.deltaTime;
        StatUpdate(this);
    }*/

    public void LoadPopUp()
    {
        InterfaceManager.instance.LoadQuestion(this);
    }
    public void SelectAnswer(string statement)
    {
        Answer answer = answers.Find(answer => answer.statement == statement);

        foreach(Area area in GameManager.instance.target.areas)
        {
            for (int i = 0; i < answer.stats.Count; i++)
            {
                if (area.stats.Contains(answer.stats[i]))
                {
                    if (answer.affinityPoints.Count == 1)
                    {
                        area.affinity += answer.affinityPoints[0];
                    }
                    else
                    {
                        area.affinity += answer.affinityPoints[i];
                    }
                }
            }
        }

        //GameManager.instance.UpdateAffinityPointCount();
        GameManager.instance.affinityPointMax += 1;
        InterfaceManager.instance.OnAnswerSelected?.Invoke();
    }











    /*
    public override bool GetBoolValue() { return false; }
    public override float GetCurrentValue() { return countDown; }
    public override float GetMaxValue() { return GameManager.instance.questionTriggerTime; }
    public override string GetStringValue() { return ""; }
    */
}
