using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Trigger : StatElement
{
    [Space(20)]
    public float loadTime = 0;
    public bool useGlobalLoadTime = true;
    protected float load = 0;
    
    [Space(20)]
    public UnityEvent OnTriggerEnter;
    public UnityEvent OnTriggerLoad;
    public UnityEvent OnTriggerLoadDone;
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
