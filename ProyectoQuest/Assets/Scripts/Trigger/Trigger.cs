using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Trigger : MonoBehaviour
{
    public float loadTime = 0;
    protected float load = 0;
    public UnityEvent OnTriggerEnter;
    public UnityEvent OnTriggerLoad;
    public UnityEvent OnTriggerStay;
    public UnityEvent OnTriggerExit;

    protected List<Target> targets;

    public Target GetLastTarget()
    {
        return targets[targets.Count];
    }
    public List<Target> GetTargets()
    {
        return targets;
    }
}
