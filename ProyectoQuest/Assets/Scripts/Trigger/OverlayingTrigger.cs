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
        if(targets.Count > 0) OnTriggerStay?.Invoke();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Target target = collision.GetComponent<Target>();
        if (target != null)
        {
            if (!targets.Contains(target)) targets.Add(target);
            OnTriggerEnter?.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Target target = collision.GetComponent<Target>();
        if (target != null)
        {
            targets.Remove(target);
            OnTriggerExit?.Invoke();
        }
    }
}
