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
    public List<Question> questions;

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
        if (character == null) character = FindObjectOfType<Character>();

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
            if (questions.Count == 0) { questions = FindObjectsOfType<Question>().ToList(); }
            bool allAnswered = true;

            int i = 0;
            foreach (Question question in questions)
            {
                i += 1;
                if (!question.answered)
                {
                    allAnswered = false;
                    break;
                }
            }
            progressBar.SimpleRefresh(i, questions.Count, Bar.NumericType.Ratio, Bar.NumericFormat.Integer);
            character.player.SetMove(true);
            return allAnswered ? MatchState.End : MatchState.Walking;
        }
    }

    private bool End()
    {
        InterfaceManager.instance.missionCompletedPopUp.SetActive(true);
        return true;
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
