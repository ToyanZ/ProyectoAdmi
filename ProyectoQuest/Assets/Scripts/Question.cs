using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : StatElement
{
    private float countDown = 0;
    public string statement = "";
    public List<Answer> answers;
    
    private void Start()
    {
        countDown = GameManager.instance.questionTriggerTime;
    }

    public void Enter()
    {
        GameManager.instance.LoadQuestion(this);
    }
    public void SelectAnswer(int index)
    {
        Answer answer = answers[index];
        foreach(Area area in GameManager.instance.target.areas)
        {
            foreach(Area.Stat areaStat in area.stats)
            {
                for(int i=0; i< answer.stats.Count; i++)
                {
                    if(areaStat == answer.stats[i])
                    {
                        if(answer.affinityPoints.Count == 1)
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
        }
    }











    public void StartCounting()
    {
        countDown = GameManager.instance.questionTriggerTime;
    }
    public void CountDown()
    {
        countDown -= Time.deltaTime;
        StatUpdate(this);
    }
    public override bool GetBoolValue() { return false; }
    public override float GetCurrentValue() { return countDown; }
    public override float GetMaxValue() { return GameManager.instance.questionTriggerTime; }
    public override string GetStringValue() { return ""; }
}
