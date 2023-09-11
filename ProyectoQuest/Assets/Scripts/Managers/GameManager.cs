using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum BuildType { Pc, Phone}
    public enum GameState { Menu, Match}
    public enum MenuState { Check, SignIn, MainMenu}
    public enum MatchState { Walking, Answering, End}
    public static GameManager instance;

    public GameState gameState = GameState.Menu;
    public MenuState menuState = MenuState.Check;
    public MatchState matchState = MatchState.Walking;

    [Space(20)]
    public BuildType buildType = BuildType.Pc;
    //public Action 

    [Space(20)]
    public Character character;
    public float triggerLoadTime = 1.5f;
    public int affinityPointMax = 0;
    public float answerTimeMin = 3;
    public float answerTimeAvg = 8;
    public float answerTimeMax = 11;
    
    //User info
    [Space(20)]
    [HideInInspector] public string nameUser;
    [HideInInspector] public string rutUser;
    [HideInInspector] public string emailUser;
    [HideInInspector] public string phoneNumberUser;
    [HideInInspector] public string gradeUser;

    [Space(20)]
    public float matchProgressBarUpdateTime = 2f;
    public float questionHandlerBarUpdateTime = 1f;
    public float answerCompletedBarUpdateTime = 0.7f;
    public List<QuestionHandler> questionHandlers;

    [Space(20)]
    public UnityEvent OnAnswerCompleted;
    public Bar progressBar;


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
        StateMachine();
    }
    private void StateMachine()
    {
        switch (gameState)
        {
            case GameState.Menu:
                Menu();
                break;
            case GameState.Match:
                Match();
                break;
        }
    }
    private void Menu()
    {
        switch(menuState)
        {
            case MenuState.Check:
                menuState = Check();
                break;
            case MenuState.SignIn:
                menuState = SignIn();
                break;
            case MenuState.MainMenu:
                if (!MainMenu())
                {
                    InterfaceManager.instance.OnAnswerSelected?.Invoke();
                    gameState = GameState.Match;
                } 
                break;
        }
    }
    private MenuState Check()
    {
        return MenuState.SignIn;
    }
    private MenuState SignIn()
    {
        return MenuState.MainMenu;
    }
    private bool MainMenu()
    {
        return false;
    }


    private void Match()
    {
        if (character == null) character = FindObjectOfType<Character>();
        if (questionHandlers == null) questionHandlers = new List<QuestionHandler>();
        if (questionHandlers.Count == 0) questionHandlers = FindObjectsOfType<QuestionHandler>().ToList();


        switch (matchState)
        {
            case MatchState.Walking:
                matchState = Walking();
                break;
            case MatchState.Answering:
                matchState = Answering();
                break;
            case MatchState.End:
                if(!End())
                {
                    matchState = MatchState.Walking;
                    gameState = GameState.Menu;
                }
                break;
        }
    }
    private MatchState Walking()
    {
        character.player.SetMove(!InterfaceManager.instance.popUp.activeSelf);
        return InterfaceManager.instance.popUp.activeSelf ? MatchState.Answering : MatchState.Walking;
    }
    private MatchState Answering()
    {
        if (InterfaceManager.instance.popUp.activeSelf)
        {
            return MatchState.Answering;
        }
        else
        {
            //if (questionHandlers.Count == 0) { questionHandlers = FindObjectsOfType<QuestionHandler>().ToList(); }

            float totalQuestions = 0.0f;
            foreach (QuestionHandler questionHandler in questionHandlers)
            {
                foreach (Question question in questionHandler.questions)
                {
                    totalQuestions += 1.0f;
                }
            }


            float answeredQuestions = 0.0f;
            bool allAnswered = true;
            foreach (QuestionHandler questionHandler in questionHandlers)
            {
                foreach(Question question in questionHandler.questions)
                {
                    
                    if (question.answered) answeredQuestions += 1.0f;
                    if (allAnswered) if (!question.answered) allAnswered = false;
                }
                questionHandler.ProgressUpdate();
            }
            StartCoroutine(MatchProgressUpdate(answeredQuestions, totalQuestions));
            character.player.SetMove(true);
            
            return allAnswered ? MatchState.End : MatchState.Walking;
        }
    }

    IEnumerator MatchProgressUpdate(float current, float max)
    {
        float time = matchProgressBarUpdateTime;
        float start = progressBar.filler.fillAmount;
        while (time > 0.0f)
        {
            time -= Time.deltaTime;
            float progress = Mathf.Lerp(start, current, 1 - (time / matchProgressBarUpdateTime));
            
            progressBar.SimpleRefresh(progress, max, Bar.NumericType.Ratio, Bar.NumericFormat.Integer);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private bool End()
    {
        Invoke("ShowEndHUD", 2f);
        return true;
    }

    void ShowEndHUD()
    {
        InterfaceManager.instance.missionCompletedPopUp.SetActive(true);
    }



    public void UpdateAffinityPointCount()
    {
        affinityPointMax = 0;
        foreach (Area area in character.player.target.areas)
        {
            if(area.affinity > affinityPointMax) affinityPointMax = area.affinity;
        }
    }
}
