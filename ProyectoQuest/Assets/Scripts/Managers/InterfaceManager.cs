using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager instance;
    public TMP_InputField inputName;
    public TMP_InputField inputRut;
    public TMP_InputField inputEmail;
    public TMP_InputField inputPhone;
    public TMP_Dropdown inputGrade;


    private void Awake()
    {
        
    }
    private void Update()
    {
        string aadasss = inputGrade.options[0].text;
        Debug.Log(aadasss);
    }

    private void CreateInstance()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}
