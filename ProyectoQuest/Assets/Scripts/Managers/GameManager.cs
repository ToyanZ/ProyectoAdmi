using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //User info
    [HideInInspector] public string nameUser;
    [HideInInspector] public string rutUser;
    [HideInInspector] public string emailUser;
    [HideInInspector] public string phoneNumberUser;
    [HideInInspector] public string gradeUser;

    public float questionTriggerTime = 2;
    public enum BuildType { Pc, Phone}

    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}
