using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum BuildType { Pc, Phone }
    public enum GameState { Menu, Match }
    public enum MenuState { Check, SignIn, MainMenu }
    public enum MatchState { Walking, Answering, MiniGame, End }
    public static GameManager instance;

    
    public bool testing = true;
    public BuildType buildType = BuildType.Pc;
    
    [Header("Sates")]
    public GameState gameState = GameState.Menu;
    public MenuState menuState = MenuState.Check;
    public MatchState matchState = MatchState.Walking;


    [Header("Time And Counting")]
    public int affinityPointMax = 0;
    public int minigamesTry = 3;

    [Space(10)]
    public float triggerLoadTime = 1.5f;
    public float matchProgressBarUpdateTime = 2f;
    public float questionHandlerBarUpdateTime = 1f;
    public float answerCompletedBarUpdateTime = 0.7f;

    [Space(10)]
    public float doorsOpenTime = 2f;

    [Header("References")]
    public Bar progressBar;
    public CameraTracking cameraTracking;
    public GameObject dustParticleEffect;
    public List<Zone> zones;

    
    
    [HideInInspector] public Character character;
    [HideInInspector] public Zone currentZone;
    
    //User info
    [HideInInspector] public string nameUser;
    [HideInInspector] public string rutUser;
    [HideInInspector] public string emailUser;
    [HideInInspector] public string phoneNumberUser;
    [HideInInspector] public string gradeUser;
    
    [HideInInspector] public int characterIndex = 0;
    [HideInInspector] public bool miniGameCompleted = false;
    [HideInInspector] public bool miniGameTutorial = true;
    [HideInInspector] public int playerCoins = 0;


    //Eliminar
    [HideInInspector] public float answerTimeMin = 3;
    [HideInInspector] public float answerTimeAvg = 8;
    [HideInInspector] public float answerTimeMax = 11;
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
        StateMachine();
        if(buildType == BuildType.Pc) { InterfaceManager.instance.joystick.SetActive(false); }
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
        switch (menuState)
        {
            case MenuState.Check:
                menuState = Check();
                break;
            case MenuState.SignIn:
                menuState = SignIn();
                break;
            case MenuState.MainMenu:
                MainMenu();
                break;
        }
    }
    private MenuState Check()
    {
        if (!testing)
        {
            InterfaceManager.instance.form.gameObject.SetActive(true);
            InterfaceManager.instance.tutorial.gameObject.SetActive(false);
        }
        
        return MenuState.SignIn;
    }
    private MenuState SignIn()
    {
        return MenuState.MainMenu;
    }
    private void MainMenu()
    {
        gameState = GameState.Match;
    }



    private void Match()
    {
        if (character == null)
        {
            character = FindObjectOfType<Character>();
            return;
        }
        if (zones == null)
        {
            zones = new List<Zone>();
            return;
        }
        if (zones.Count == 0) zones = FindObjectsOfType<Zone>().ToList();


        switch (matchState)
        {
            case MatchState.Walking:
                Walking();
                break;
            case MatchState.Answering:
                Answering();
                break;
            case MatchState.MiniGame:
                MiniGame();
                break;
            case MatchState.End:
                End();
                break;
        }
    }
    private void Walking()
    {
        if (InterfaceManager.instance.popUp.activeSelf)
        {
            character.player.SetMove(false);
            matchState = MatchState.Answering;
        }
    }
    private void Answering()
    {
        if (!InterfaceManager.instance.popUp.activeSelf)
        {
            //Cuenta el total de preguntas
            float totalQuestions = 0.0f;
            foreach (Zone questionHandler in zones)
            {
                foreach (Question question in questionHandler.questions)
                {
                    totalQuestions += 1.0f;
                }
            }

            //Cuenta el total de preguntas respondidas y
            //revisa si todas las preguntas de la zona actual estan respondidas
            float answeredQuestions = 0.0f;
            bool allAnswered = true;
            foreach (Zone questionHandler in zones)
            {
                foreach (Question question in questionHandler.questions)
                {
                    if (question.answered) answeredQuestions += 1.0f;

                    if (questionHandler == currentZone)
                    {
                        if (!question.answered) allAnswered = false;
                    }
                }
                questionHandler.ProgressUpdate();
            }
            StartCoroutine(MatchProgressUpdate(answeredQuestions, totalQuestions));
            character.player.SetMove(true);


            //Si se responden todas las preguntas del juego
            if (answeredQuestions == totalQuestions)
            {
                matchState = MatchState.End;
            }
            else if (allAnswered)//Si se responden todas las preguntas del area / zona
            {
                if (currentZone == null)
                {
                    float distance = -1;
                    foreach (Zone questionHandler in zones)
                    {
                        float currentDist = (questionHandler.boxCollider2D.transform.position - character.transform.position).magnitude;
                        if (distance == -1 || currentDist < distance)
                        {
                            distance = currentDist;
                            currentZone = questionHandler;
                        }
                    }
                    return;
                }

                if (currentZone.gamePlayed == false)
                {
                    currentZone.gamePlayed = true;
                    StartCoroutine(StartMiniGameSetUp());
                }
            }
            else //Si aún le quedan preguntas por responder
            {
                matchState = MatchState.Walking;
            }
        }
    }
    IEnumerator StartMiniGameSetUp()
    {
        while(currentZone.barImage.fillAmount < 0.98) yield return null;

        InterfaceManager.instance.HideMainGameUI();
        character.gameObject.SetActive(false);
        matchState = MatchState.MiniGame;
        
        LevelManager.instance.LoadRandomGame("Level1", 0);
    }
    IEnumerator MatchProgressUpdate(float current, float max)
    {
        float time = matchProgressBarUpdateTime;
        float start = progressBar.filler.fillAmount * max;
        while (time > 0.0f)
        {
            time -= Time.deltaTime;
            float progress = Mathf.Lerp(start, current, 1 - (time / matchProgressBarUpdateTime));

            progressBar.SimpleRefresh(progress, max, Bar.NumericType.Ratio, Bar.NumericFormat.Integer);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    private void MiniGame()
    {
        if (miniGameCompleted)
        {
            InterfaceManager.instance.ShowMainGameUI();
            matchState = MatchState.Walking;
            currentZone.Open();
            character.gameObject.SetActive(true);
        }
    }
    private void End()
    {
        matchState = MatchState.Walking;
        gameState = GameState.Menu;
        //Invoke("ShowEndHUD", 2f);
        InterfaceManager.instance.ShowPlayerStats();
    }
    
    private void ShowEndHUD()
    {
        InterfaceManager.instance.missionCompletedPopUp.SetActive(true);
    }
    public void MiniGameCompleted()
    {
        miniGameCompleted = true;
    }

    public void UpdateAffinityPointCount()
    {
        affinityPointMax = 0;
        foreach (Area area in InterfaceManager.instance.afinityAreas)
        {
            if(area.affinity > affinityPointMax) affinityPointMax = area.affinity;
        }
    }
    
    public void SetCurrentArea(Zone questionHandler)
    {
        currentZone = questionHandler;
        cameraTracking.GoTo(questionHandler.cameraPosition);
    }
}
