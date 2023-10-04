using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager instance;

    public GameObject form;
    public TMP_InputField inputName;
    public TMP_InputField inputRut;
    public TMP_InputField inputEmail;
    public TMP_InputField inputPhone;
    public TMP_Dropdown inputGrade;
    public string userName;
    public string userRut;
    public string userEmail;
    public string userPhone;
    public string userGrade;

    [Space(20)]
    public GameObject tutorial;


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


    [Space(20)]
    public GameObject onScreenUI;
    public GameObject inGameUI;
    public GameObject joystick;
    public RectTransform statsUI;
    public RectTransform statsUIStartPoint;
    public RectTransform statsUIEndPoint;


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

    public void SendForm()
    {
        UserNameControl(inputName);
        UserRutControl(inputRut);
        UserEmailControl(inputEmail);
        userPhone = inputPhone.text;
        userGrade = inputGrade.captionText.text;
        print("****" + userRut + "****");
        //gameObject.GetComponent<Emailer>().CallSendEmail();
    }

    public void SendForm2()
    {
        gameObject.GetComponent<Emailer>().CallSendEmail();
    }

    public void UserNameControl(TMP_InputField inputField)
    {
        bool charInput = false;
        int index = 0;
        string realName;
        if (!String.IsNullOrEmpty(inputField.text))
        {

            foreach (Char c in inputField.text)
            {
                if (c != ' ')//omite los primeros espacios vacios antes del primer caracter
                {
                    charInput = true;
                    break;//se interrumpe al encontrar el primer caracter
                }
                index++;//guarda la posicion del primer caracter
            }
            if (charInput)
            {
                realName = inputField.text.Substring(index);//comienza desde el primer caracter encontrado
                /*if (realName.Length > 15) //Controla la cantidad de caracteres que tendrá como máximo el string
                {
                    realName = realName.Substring(0, 15);
                }*/
                userName = realName;
            }
        }
    }

    public void UserRutControl(TMP_InputField inputField)
    {
        int index = 0;
        char [] realRut = new char [9];
        int multiplier = 2;
        int totalAdd = 0;
        int total = 0;
        bool rightRut = true;
        int countKInRut = 0; 
        if (!String.IsNullOrEmpty(inputField.text))
        {
            foreach (Char c in inputField.text)
            {
                if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9' || c == 'k' || c == 'K')
                {
                    if(index < 9)
                    {
                        realRut[index] = c;
                        index++;
                    }
                    else
                    {
                        rightRut = false;
                    }
                    if(c == 'k' || c == 'K')
                    {
                        countKInRut++;
                    }
                }
            }
            if(index<8 || index > 9)
            {
                rightRut=false;
            }
            if (rightRut && countKInRut == 0 || countKInRut ==1)
            {
                for (int i = index - 2; i >= 0; i--) //comienza en la penultima posicion
                {
                    if (realRut[i] != 'k' && realRut[i] != 'K')
                    {
                        totalAdd = totalAdd + Convert.ToInt32(new string(realRut[i], 1)) * multiplier;
                        multiplier++;
                        if (multiplier > 7)
                        {
                            multiplier = 2;
                        }
                    }
                    else
                    {
                        rightRut = false;
                        break;
                    }

                }
            }
            if (rightRut && countKInRut == 0 || countKInRut == 1)
            {
                total = totalAdd / 11;
                total = total * 11;
                total = Math.Abs(totalAdd - total);
                total = Math.Abs(11 - total);//digito verificador
                if (total == 11)
                {
                    if (Convert.ToInt32(new string(realRut[index - 1], 1)) == 0)
                    {
                        for (int i = index - 1; i >= 0; i--)
                        {
                            int substract = (index - 1) - i;
                            if (substract == 0)
                            {
                                userRut = "-" + realRut[i];
                            }
                            else if (substract == 3 || substract == 6)
                            {
                                userRut = "." + realRut[i] + userRut;
                            }
                            else
                            {
                                userRut = realRut[i] + userRut;
                            }
                        }
                    }
                    else
                    {
                        rightRut = false;
                    }
                }
                else if (total == 10)
                {
                    if (realRut[index - 1] == 'k' || realRut[index - 1] == 'K')
                    {
                        for (int i = index - 1; i >= 0; i--)
                        {
                            int substract = (index - 1) - i;
                            if (substract == 0)
                            {
                                userRut = "-k";
                            }
                            else if (substract == 3 || substract == 6)
                            {
                                userRut = "." + realRut[i] + userRut;
                            }
                            else
                            {
                                userRut = realRut[i] + userRut;
                            }
                        }
                    }
                    else
                    {
                        rightRut = false;
                    }
                }
                else
                {
                    if (Convert.ToInt32(new string(realRut[index - 1], 1)) == total)
                    {
                        for (int i = index - 1; i >= 0; i--)
                        {
                            int substract = (index - 1) - i;
                            if (substract == 0)
                            {
                                userRut = "-" + realRut[i];
                            }
                            else if (substract == 3 || substract == 6)
                            {
                                userRut = "." + realRut[i] + userRut;
                            }
                            else
                            {
                                userRut = realRut[i] + userRut;
                            }
                        }
                    }
                    else
                    {
                        rightRut = false;
                    }
                }
            }
            if (!rightRut)
            {
                //Agregar texto en pantalla
                print("Verifique que su rut esté correctamente ingresado");
            }
        }
    }
    public void UserEmailControl(TMP_InputField inputField)
    {
        bool charInput = false;
        int index = 0;
        string realEmail;
        if (!String.IsNullOrEmpty(inputField.text))
        {

            foreach (Char c in inputField.text)
            {
                if (c != ' ')
                {
                    charInput = true;
                    break;
                }
                index++;
            }
            if (charInput)
            {
                realEmail = inputField.text.Substring(index);
                userEmail = realEmail;
            }
        }
    }


    public void HideMainGameUI()
    {
        onScreenUI.SetActive(false);
        inGameUI.SetActive(false);
        joystick.SetActive(false);
    }
    public void ShowMainGameUI()
    {
        onScreenUI.SetActive(true);
        inGameUI.SetActive(true);
        joystick.SetActive(true);
    }

    public void ShowPlayerStats()
    {
        StartCoroutine(ShowStats());
    }
    private IEnumerator ShowStats()
    {
        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime;
            statsUI.position = Vector3.Lerp(statsUIStartPoint.position, statsUIEndPoint.position, time / 1);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }


}
