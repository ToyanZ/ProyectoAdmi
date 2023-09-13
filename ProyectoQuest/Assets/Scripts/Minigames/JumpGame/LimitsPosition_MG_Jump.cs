using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LimitsPosition_MG_Jump : MonoBehaviour
{
    public Transform playerPosition;
    public float xPosition;

    private void Update()
    {
        GetYPositionFromPlayer();
    }

    void GetYPositionFromPlayer()
    {
        if(playerPosition != null)
        {
            Vector2 newPosition = new Vector2(xPosition, playerPosition.transform.position.y);
            transform.position = newPosition;
        }
    }
}
