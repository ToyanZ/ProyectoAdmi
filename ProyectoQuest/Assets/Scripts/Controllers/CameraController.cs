using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera tCamera;
    public Transform target;
    public float deadZoneX = 5;
    public float deadZoneY = 7;
    
    public float offsetX = 0;
    public float offsetY = 0;

    [Space(20)]
    public Vector3 absolutePosition;
    public Vector3 relativePosition;
    public Vector3 cameraPosition;

    private Bounds bounds;
    private float z;

    private void FixedUpdate()
    {
        Follow();
        z = tCamera.transform.position.z;
    }

    void Follow()
    {
        if (target == null)
        {
            target = FindObjectOfType<Target>().transform;
            return;
        }

        bounds = new Bounds(target.position, Vector2.one);

        absolutePosition = bounds.center;
        relativePosition = absolutePosition + new Vector3(offsetX, offsetY, z);


    }

    public bool OutOfBounds()
    {
        return true;
    }

    private void OnDrawGizmos()
    {
        if (target == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(target.position, new Vector2(deadZoneX, deadZoneY));
    }

}
