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

    [Header("Form UI")]
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

    [Header("Tutorial UI")]
    public GameObject tutorial;


    [Header("Question Pop Up")]
    public GameObject popUp;
    public RectTransform portrait;
    public TMP_Text statement;
    public List<AnswerButton> answerButtons;
    public List<Area> afinityAreas;


    [Header("Game UI")]
    public GameObject onScreenUI;
    public GameObject inGameUI;
    public GameObject joystick;
    public RectTransform statsUI;
    public RectTransform statsUIStartPoint;
    public RectTransform statsUIEndPoint;
    public GameObject missionCompletedPopUp;

    [Header("Game UI")]
    public Button gachapon;
    public TMP_Text coinText;
    public string coinName;

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
    private void Start()
    {
        LoadCareerButton();
    }
    private void Update()
    {
        //string aadasss = inputGrade.options[0].text;
        //Debug.Log(aadasss);
        
        gachapon.interactable = GameManager.instance.playerCoins > 0 ? true : false;
        coinText.text = coinName + " x " + GameManager.instance.playerCoins.ToString();
    }

    
    private void LoadCareerButton()
    {
        foreach(Area area in afinityAreas)
        {
            area.creerButton.onClick.AddListener(() =>
            {
                //Minimiza todos los otros cuadros
                foreach (Area area in afinityAreas)
                {
                    area.relatedCareers.gameObject.SetActive(false);
                    area.relatedCareers.localScale = new Vector3(1, 0, 1);
                }
                //Expande el cuadro actual
                StartCoroutine(ShowRelatedCareersIE(area.relatedCareers));
            });
        }
    }
    IEnumerator ShowRelatedCareersIE(RectTransform rectTransform)
    {
        rectTransform.gameObject.SetActive(true);
        float time = 0;
        float maxTime = 0.2f;
        while (time < maxTime)
        {
            time += Time.deltaTime;
            Vector3 scale = rectTransform.localScale;
            rectTransform.localScale = new Vector3(scale.x, time / maxTime, scale.z);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public void LoadQuestion(Question question)
    {
        statement.text = question.statement;//Actualiza el texto de la nueva pregunta
        question.portaitInstance = Instantiate(question.npcPortait);
        question.portaitInstance.SetParent(portrait);
        question.portaitInstance.localScale = Vector3.one;

        //Por cada respueta
        for (int i = 0; i < question.answers.Count; i++)
        {
            string answerText = question.answers[i].statement;

            //Cuando haga click en el boton asociado
            answerButtons[i].button.onClick.AddListener(() => 
            {
                question.SelectThisAnswer(answerText); //Se ejecutará el metedodo "Select this answer"
                CloseQuestion(); //Y se cierra el dialogo de pregunta
            });

            answerButtons[i].text.text = answerText; //Actualiza el texto de las respuestas.
            answerButtons[i].gameObject.SetActive(true); //Activa el botón
        }

        popUp.gameObject.SetActive(true);//Activa la ventana de pregunta
    }
    private void CloseQuestion()
    {
        popUp.gameObject.SetActive(false);

        int count = answerButtons.Count;
        for(int i=0; i<count; i++)
        {
            answerButtons[i].button.onClick.RemoveAllListeners();
            answerButtons[i].gameObject.SetActive(false);
            Destroy(portrait.GetChild(0).gameObject);
        }

        statement.text = "";
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

    public void UpdateAfinityBars()
    {
        foreach(Area area in afinityAreas)
        {
            area.affinityBar.SimpleRefresh(area.affinity, 4);
        }
    }

    //Se llama desde trigger (editor)
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

}
