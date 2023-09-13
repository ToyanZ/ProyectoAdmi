using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObjectHolder : MonoBehaviour
{
    public static PersistentObjectHolder instance;

    private void Awake()
    {
        if(instance != null) Destroy(gameObject);

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
