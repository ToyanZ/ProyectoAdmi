using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject popUp;
    public TMP_Text statement;
    public Button buttonPrefab;
    public Target target;
    public void LoadQuestion(Question question)
    {
        statement.text = question.statement;
        //Usar un for
        Button b1 = Instantiate(buttonPrefab);
        Button b2 = Instantiate(buttonPrefab);
        Button b3 = Instantiate(buttonPrefab);
        Button b4 = Instantiate(buttonPrefab);

        b1.onClick.AddListener(() => { question.SelectAnswer(0); });
        b2.onClick.AddListener(() => { question.SelectAnswer(1); });
        b3.onClick.AddListener(() => { question.SelectAnswer(2); });
        b4.onClick.AddListener(() => { question.SelectAnswer(3); });
    }

}
