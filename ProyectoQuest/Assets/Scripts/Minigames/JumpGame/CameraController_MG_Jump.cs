using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController_MG_Jump : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 0, -10f);
    public float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    private float currentYPosition = 0;

    [SerializeField] private Transform target;


    private void FixedUpdate()
    {
        GetObjetiveToFollow();
    }

    void GetObjetiveToFollow()
    {
        if(target != null)
        {
            Vector3 targetPosition = target.position + offset;
            Vector3 fixedPosition = new Vector3(0, targetPosition.y, targetPosition.z);
            if(currentYPosition < target.position.y)
            {
                currentYPosition = transform.position.y;
                transform.position = Vector3.SmoothDamp(transform.position, fixedPosition, ref velocity, smoothTime);
            }
        }
    }
}
