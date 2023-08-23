using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class StatElement : MonoBehaviour
{
    public Action<StatElement> OnDataUpdatedAction;
    public UnityEvent<StatElement> OnDataUpdatedEvent;


    protected void StatUpdate(StatElement statElement)
    {
        OnDataUpdatedAction?.Invoke(statElement);
        OnDataUpdatedEvent?.Invoke(statElement);
    }

    public abstract float GetMaxValue();
    public abstract float GetCurrentValue();
    public abstract bool GetBoolValue();
    public abstract string GetStringValue();
}
