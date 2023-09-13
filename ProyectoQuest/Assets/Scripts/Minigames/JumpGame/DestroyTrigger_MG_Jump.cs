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
            InstantiateManager_MG_Jump.instance.InstantiateNewFloor(collision.transform);
            Debug.Log("Choca con una plataforma");
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Player"))
        {
            if(imagen != null) imagen.enabled = true;

            Destroy(collision.gameObject);
            InstantiateManager_MG_Jump.instance.LostGame();
            if(video != null) video.Play();

        }
    }
}
