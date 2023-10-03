using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class DestroyTrigger_MG_Jump : MonoBehaviour
{
    public VideoPlayer video;
    public RawImage imagen;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Floor"))
        {
            
            collision.GetComponent<JumpTrigger_MG_Jump>().SendSpawnNewObject(collision.transform);
            //InstantiateManager_MG_Jump.instance.InstantiateNewFloorDestroyZone(collision.transform);
            
            Destroy(collision.gameObject);
            
        }
        if (collision.CompareTag("Player"))
        {
            if(imagen != null) imagen.enabled = true;

            collision.GetComponent<Jumping_MG_Jump>().LostPlayer();
            //Destroy(collision.gameObject);
            InstantiateManager_MG_Jump.instance.LostGame();
            if(video != null) video.Play();
        }
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            InstantiateManager_MG_Jump.instance.InstantiateNewEnemy(collision.transform);
        }
    }
}
