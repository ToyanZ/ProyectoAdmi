using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

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

    //Character AreaStats
    [Space(20)]
    public UnityEvent OnAnswerSelected;
    //public List<Bar> statBars;
    //public float updateStatTime = 1;

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        //string aadasss = inputGrade.options[0].text;
        //Debug.Log(aadasss);
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
    
    /*
    private IEnumerator UpdateTargetAreaStats()
    {
        int max = 0;
        foreach (Area area in GameManager.instance.target.areas)
        {
            max += area.affinity;
        }

        float time = 0;
        while (time < updateStatTime)
        {
            float currentVal = 0;

            for (int i = 0; i < GameManager.instance.target.areas.Count; i++)
            {
                if (statBars.Count < i) continue;

                float prevVal = statBars[i].filler.fillAmount;
                float scale = (time / updateStatTime);
                currentVal = GameManager.instance.target.areas[i].affinity * scale;
                
                

                //statBars[i].Refresh(currentVal, max);
            }

            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    */
}
