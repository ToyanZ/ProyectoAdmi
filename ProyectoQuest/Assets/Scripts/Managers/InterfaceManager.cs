using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager instance;
    public TMP_InputField inputName;
    public TMP_InputField inputRut;
    public TMP_InputField inputEmail;
    public TMP_InputField inputPhone;
    public TMP_Dropdown inputGrade;

    //Question PopUp
    [Space(20)]
    public GameObject popUp;
    public TMP_Text statement;
    public List<AnswerButton> buttons;
    public Bar answerTimerBar;
    public List<Image> attackPoints;

    
    [Space(20)]
    public UnityEvent OnAnswerSelected;
    public GameObject missionCompletedPopUp;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Update()
    {
        //string aadasss = inputGrade.options[0].text;
        //Debug.Log(aadasss);
    }

    

    public void LoadQuestion(Question question)
    {
        statement.text = question.statement;

        for (int i = 0; i < question.answers.Count; i++)
        {
            string answerText = question.answers[i].statement;

            buttons[i].button.onClick.AddListener(() => {
                question.SelectAnswer(answerText);
                CloseQuestion();
            });

            buttons[i].text.text = answerText;
            buttons[i].gameObject.SetActive(true);
        }

        popUp.gameObject.SetActive(true);
    }
    private void CloseQuestion()
    {
        popUp.gameObject.SetActive(false);

        int count = buttons.Count;
        for(int i=0; i<count; i++)
        {
            buttons[i].button.onClick.RemoveAllListeners();
            buttons[i].gameObject.SetActive(false);
        }

        statement.text = "";
    }
    
}
