using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ContactTrigger : Trigger
{
    private void Start()
    {
        targets = new List<Target>();
    }
    private void Update()
    {
        if (targets.Count > 0)
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
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
        Target target = collision.gameObject.GetComponent<Target>();
        if (target != null)
        {
            targets.Remove(target);
            OnTriggerExit?.Invoke();
            load = 0;
        }
    }
}
