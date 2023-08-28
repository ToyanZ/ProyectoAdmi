using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterController : MonoBehaviour
{
    public Transform controller;
    Vector3 touchPos = Vector3.zero;
    bool dragging = false;
    public CircleCollider2D circleCollider;
    public bool smoothing = false;

    private void Update()
    {
        if (dragging)
        {
            Vector2 distance = touchPos - transform.position;

            bool outOfRange = distance.magnitude > circleCollider.radius;
            if (outOfRange) touchPos = ((Vector2)transform.position + (distance.normalized * circleCollider.radius)) * new Vector3(1, 1, 0);
            controller.position = touchPos;
        }
        else
        {
            controller.localPosition = Vector2.zero;
        }
    }


    private void OnMouseDrag()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            touchPos.z = 0;
            dragging = true;
        }
        else
        {
            dragging = false;
        }

        if (Input.GetMouseButton(0))
        {
            touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchPos.z = 0;
            dragging = true;
        }
    }

    private void OnMouseUp()
    {
        dragging = false;
    }


    public Vector2 GetDirection()
    {
        Vector2 direction = (controller.position - transform.position);
        direction = smoothing ? direction.normalized * (direction.magnitude / circleCollider.radius): direction.normalized;
        return dragging ? direction : Vector2.zero;
    }
}
