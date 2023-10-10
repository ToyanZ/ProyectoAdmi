using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    
    public void PlaySound(int id)
    {
        SFX_Manager.instance.PlaySound(id);
    }
}
