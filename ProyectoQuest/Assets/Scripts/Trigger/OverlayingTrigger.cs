using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class OverlayingTrigger : Trigger
{
    private void Start()
    {
        targets = new List<Target>();
    }

    private void Update()
    {
        if(targets.Count > 0)
        {
            if (load < loadTime)
            {
                load += Time.fixedDeltaTime;
                OnTriggerLoad?.Invoke();
            }
            else
            {
                OnTriggerStay?.Invoke();
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Target target = collision.GetComponent<Target>();
        if (target != null)
        {
            if (!targets.Contains(target)) targets.Add(target);
            OnTriggerEnter?.Invoke();
            load = 0;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Target target = collision.GetComponent<Target>();
        if (target != null)
        {
            targets.Remove(target);
            OnTriggerExit?.Invoke();
            load = 0;
        }
    }
}
