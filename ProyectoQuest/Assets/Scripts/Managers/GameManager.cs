using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Target target;
    //User info
    [Space(20)]
    [HideInInspector] public string nameUser;
    [HideInInspector] public string rutUser;
    [HideInInspector] public string emailUser;
    [HideInInspector] public string phoneNumberUser;
    [HideInInspector] public string gradeUser;

    public float triggerLoadTime = 1.5f;
    public int affinityPointMax = 0;
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

    private void Update()
    {
        if(target == null) target = FindObjectOfType<Target>();
    }

    public void UpdateAffinityPointCount()
    {
        affinityPointMax = 0;
        foreach (Area area in GameManager.instance.target.areas)
        {
            if(area.affinity > affinityPointMax) affinityPointMax = area.affinity;
        }
    }
}
