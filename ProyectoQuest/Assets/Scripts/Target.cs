using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public float walkSpeed = 60;
    public float gravityScale = 0;

    [Space(10)]
    public List<Area> areas;
}
