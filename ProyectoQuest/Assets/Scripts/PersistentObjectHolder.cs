using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObjectHolder : MonoBehaviour
{
    public static PersistentObjectHolder instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        
    }
}
