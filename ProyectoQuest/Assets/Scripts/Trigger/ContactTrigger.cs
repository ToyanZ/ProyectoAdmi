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
        if (targets.Count > 0) OnTriggerStay?.Invoke();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Target target = collision.gameObject.GetComponent<Target>();
        if (target != null)
        {
            if (!targets.Contains(target)) targets.Add(target);
            OnTriggerEnter?.Invoke();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        Target target = collision.gameObject.GetComponent<Target>();
        if (target != null)
        {
            targets.Remove(target);
            OnTriggerExit?.Invoke();
        }
    }
}
