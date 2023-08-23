using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : StatElement
{
    private float countDown = 0;
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
        countDown-=Time.deltaTime;
        StatUpdate(this);
    }

    public override bool GetBoolValue() { return false; }
    public override float GetCurrentValue() { return countDown; }
    public override float GetMaxValue() { return GameManager.instance.questionTriggerTime; }
    public override string GetStringValue() { return ""; }
}
