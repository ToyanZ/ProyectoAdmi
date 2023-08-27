using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ContactTrigger : Trigger
{
    private void Start()
    {
        targets = new List<Target>();

        if (useGlobalLoadTime) loadTime = GameManager.instance.triggerLoadTime;
    }
    private void Update()
    {
        

        if (targets.Count > 0)
        {
            if (load < loadTime)
            {
                if (loadingLocked) return;

                load += Time.deltaTime;
                OnTriggerLoad?.Invoke();
                if (load >= loadTime) OnTriggerLoadDone?.Invoke();
            }
            else
            {
                if (stayLocked) return;
                OnTriggerStay?.Invoke();
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enterLocked) return;

        Target target = collision.gameObject.GetComponent<Target>();
        if (target != null)
        {
            if (!targets.Contains(target)) targets.Add(target);
            OnTriggerEnter?.Invoke();
            load = 0;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (exitLocked) return;

        Target target = collision.gameObject.GetComponent<Target>();
        if (target != null)
        {
            targets.Remove(target);
            OnTriggerExit?.Invoke();
            load = 0;
        }
    }

    public override float GetMaxValue() { return loadTime; }
    public override float GetCurrentValue() { return load; }
    public override bool GetBoolValue() { return false; }
    public override string GetStringValue() { return ""; }
}
