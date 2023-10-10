using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Trigger : StatElement
{
    [Space(20)]
    public float loadTime = 0;
    public float stayTime = 0;
    public bool useGlobalLoadTime = true;
    
    [Space(20)]
    [SerializeField] protected bool enterLocked = false;
    [SerializeField] protected bool loadingLocked = false;
    [SerializeField] protected bool stayLocked = false;
    [SerializeField] protected bool exitLocked = false;
    protected float load = 0;
    protected float stay = 0;
    
    [Space(20)]
    public UnityEvent OnTriggerEnter;
    public UnityEvent OnTriggerLoad;
    public UnityEvent OnTriggerLoadDone;
    public UnityEvent OnTriggerStay;
    public UnityEvent OnTriggerStayDone;
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
    public void EnterLock() { enterLocked = true; }
    public void LoadLock() { loadingLocked = true; }
    public void StayLock() { stayLocked = true; }
    public void ExitLock() { exitLocked = true; }

    public void EnterUnlock() { enterLocked = false; }
    public void LoadUnlock() { loadingLocked = false; }
    public void StayUnlock() { stayLocked = false; }
    public void ExitUnlock() { exitLocked = false; }
}
