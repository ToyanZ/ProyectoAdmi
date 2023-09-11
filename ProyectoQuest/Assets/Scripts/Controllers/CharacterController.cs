using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterController : MonoBehaviour
{
    

    public Vector2 GetDirection()
    {
        //Vector2 direction = (controller.position - transform.position);
        //direction = smoothing ? direction.normalized * (direction.magnitude / circleCollider.radius): direction.normalized;
        //return dragging ? direction : Vector2.zero;
        return Vector2.zero;
    }
}
