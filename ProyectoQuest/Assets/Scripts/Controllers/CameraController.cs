using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera tCamera;
    public Transform target;
    public float deadZoneX = 5;
    public float deadZoneY = 7;
    
    [Space(5)]
    public float offsetX = 0;
    public float offsetY = 0;

    [Space(5)]
    public float smoothingX = 0.3f;
    public float smoothingY = 0.3f;


    [Space(20)]
    public Vector3 absolutePosition;
    public Vector3 relativePosition;
    public Vector3 cameraPosition;

    private Bounds bounds;
    private Vector3 offset;
    private Vector3 velocity = Vector3.zero;
    private float z;


    private void Start()
    {
        absolutePosition = Vector3.zero;
        relativePosition = Vector3.zero;
        z = tCamera.transform.position.z;
    }
    private void FixedUpdate()
    {
        Follow();
        
    }

    void Follow()
    {
        if (target == null)
        {
            target = FindObjectOfType<Target>().transform;
            return;
        }

        bounds = new Bounds(target.position, new Vector3(0.1f, 0.1f, 0.1f));
        bounds.Encapsulate(target.position);
        offset = new Vector3(offsetX, offsetY, 0);
        //Add other targets if multiplayer

        //Out of bounds X axis
        if (OutOfBounds(bounds.center.x, absolutePosition.x, deadZoneX))
        {
            //Suavizar
            absolutePosition = new Vector3(bounds.center.x, absolutePosition.y, z);
        }
        //Out of bounds Y axis
        if (OutOfBounds(bounds.center.y, absolutePosition.y, deadZoneY))
        {
            absolutePosition = new Vector3(absolutePosition.x, bounds.center.y, z);
        }
        
        relativePosition = absolutePosition + offset;
       
        tCamera.transform.position = Vector3.SmoothDamp(tCamera.transform.position, relativePosition, ref velocity, smoothingX);
    }

    public bool OutOfBounds(float targetPos, float relativePos, float deadZone)
    {
        bool less = targetPos < relativePos - (deadZone / 2);
        bool more = targetPos > relativePos + (deadZone / 2);
        return less || more;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (target == null)
        {
            Gizmos.DrawWireCube(transform.position, new Vector2(deadZoneX, deadZoneY));
        }
        else
        {
            //Vector2 offset = new Vector2(offsetX, offsetY);
            Gizmos.DrawWireCube(absolutePosition, new Vector2(deadZoneX, deadZoneY));
            Gizmos.color = Color.yellow;    
            Gizmos.DrawWireCube(relativePosition, new Vector2(deadZoneX, deadZoneY));
            
            Gizmos.color = Color.green;    
            Gizmos.DrawWireCube(bounds.center, bounds.size);
            
        }
    }

}
