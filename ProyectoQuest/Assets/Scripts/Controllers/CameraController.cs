using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera tCamera;
    public Transform target;

    [Space(20)]
    public Vector3 absolutePosition;
    public Vector3 relativePosition;
}
