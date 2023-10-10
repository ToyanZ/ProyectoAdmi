using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum BuildType { Pc, Phone }
    public enum GameState { Menu, Match }
    public enum MenuState { Check, SignIn, MainMenu }
    public enum MatchState { Walking, Answering, MiniGame, End, Gachapon }
    public static GameManager instance;

    
    public bool testing = true;
    public BuildType buildType = BuildType.Pc;
    
    [Header("Sates")]
    public GameState gameState = GameState.Menu;
    public MenuState menuState = MenuState.Check;
    public MatchState matchState = MatchState.Walking;


    [Header("Time And Counting")]
    public float affinityPointMax = 0;
    public int minigamesTry = 3;

    [Space(10)]
    public float triggerLoadTime = 1.5f;
    public float matchProgressBarUpdateTime = 2f;
    public float questionHandlerBarUpdateTime = 1f;
    public float answerCompletedBarUpdateTime = 0.7f;
    public float showAffinityScreenTime = 0.5f;

    [Space(10)]
    public float doorsOpenTime = 2f;

    [Header("References")]
    public Bar progressBar;
    public CameraTracking cameraTracking;
    public GameObject dustParticleEffect;
    public List<Zone> zones;
    public List<DoorInfo> doorInfo;
    
    [HideInInspector] public Character character;
    //[HideInInspector] 
    public Zone currentZone;
    
    //User info
    [HideInInspector] public string nameUser;
    [HideInInspector] public string rutUser;
    [HideInInspector] public string emailUser;
    [HideInInspector] public string phoneNumberUser;
    [HideInInspector] public string gradeUser;
    
    public int characterIndex = 0;
    [HideInInspector] public bool miniGameCompleted = false;
    [HideInInspector] public bool miniGameTutorial = true;
    //[HideInInspector] 
    public int playerCoins = 0;


    bool mainProgressBarUpdating = false;
    MatchState prevState = MatchState.Walking;
    bool gachaponEntered = false;
    List<GameObject> points;
    private void Awake()
    {
        playerCoins = PlayerPrefs.GetInt("coins", 0);
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
        points = new List<GameObject>();
        List<Door> doors = FindObjectsOfType<Door>().ToList();
        doorInfo = new List<DoorInfo>();

        foreach (Door door in doors)
        {
            DoorInfo door1 = new DoorInfo(door.id, door);
            door1.unlocked = false;
            doorInfo.Add(door1);
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
        if (InterfaceManager.instance.form.activeSelf)
        {
            return MenuState.SignIn;
        }
        return MenuState.MainMenu;
    }
    private void MainMenu()
    {
        cameraTracking.GoTo(currentZone.cameraPosition);
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
            case MatchState.Gachapon:
                Gachapon(); 
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

        List<Door> doors = new List<Door>(FindObjectsOfType<Door>());
        foreach (DoorInfo doorInfo in doorInfo)
        {
            if(doorInfo.door == null)
            {
                doorInfo.door = doors.Find(door => door.id == doorInfo.id);
            }
            if (doorInfo.door != null) doorInfo.door.gameObject.SetActive(!doorInfo.unlocked);
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
            foreach (Zone zone in zones)
            {
                foreach (Question question in zone.questions)
                {
                    if (question.answered) answeredQuestions += 1.0f;

                    if (zone == currentZone)
                    {
                        if (!question.answered) allAnswered = false;
                    }
                }
                zone.ProgressUpdate();
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
                    matchState = MatchState.MiniGame;
                    StartCoroutine(StartMiniGameSetUp());
                }
            }
            else //Si aún le quedan preguntas por responder
            {
                matchState = MatchState.Walking;
            }
        }
    }
    
    IEnumerator MatchProgressUpdate(float current, float max)
    {
        mainProgressBarUpdating = true;
        float time = matchProgressBarUpdateTime;
        float start = progressBar.filler.fillAmount * max;
        while (time > 0.0f)
        {
            time -= Time.deltaTime;
            float progress = Mathf.Lerp(start, current, 1 - (time / matchProgressBarUpdateTime));

            progressBar.SimpleRefresh(progress, max, Bar.NumericType.Ratio, Bar.NumericFormat.Integer);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        mainProgressBarUpdating = false;
        yield return null;
    }
    IEnumerator StartMiniGameSetUp()
    {
        while (mainProgressBarUpdating) yield return null;
        InterfaceManager.instance.HideMainGameUI();
        character.gameObject.SetActive(false);
        LevelManager.instance.LoadRandomGame("Level1", 0);
        yield return null;
    }
    private void MiniGame()
    {
        if (miniGameCompleted && SceneManager.GetActiveScene().name == "Level1")
        {
            InterfaceManager.instance.inGameUI.SetActive(true);
            matchState = MatchState.Walking;
            currentZone.Open();
            miniGameCompleted = false;
            
        }
    }
    private void End()
    {
        matchState = MatchState.Walking;
        gameState = GameState.Menu;
        //Invoke("ShowEndHUD", 2f);
        InterfaceManager.instance.ShowPlayerStats();
    }
    
    private void Gachapon()
    {
        if (SceneManager.GetActiveScene().name == "Gachapon") gachaponEntered = true;
        
        //Cuando esta en la escena principal y viene desde el gachapon
        if (gachaponEntered && SceneManager.GetActiveScene().name == "Level1")
        {
            matchState = prevState;
            InterfaceManager.instance.inGameUI.SetActive(true);
            InterfaceManager.instance.ShowMainGameUI();
            character.gameObject.SetActive(true);
            gachaponEntered = false;
        }
    }
    //Se llama desde button (editor)
    public void SetGachaponState()
    {
        InterfaceManager.instance.HideMainGameUI();
        character.gameObject.SetActive(false);
        prevState = matchState;
        matchState = MatchState.Gachapon;
    }

    public void SetMiniGameState()
    {
        matchState = MatchState.MiniGame;
        InterfaceManager.instance.HideMainGameUI();
        character.gameObject.SetActive(false);
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
    
    public void SetCurrentArea(Zone zone)
    {
        if (currentZone != null && currentZone != zone) cameraTracking.GoTo(zone.cameraPosition);
        currentZone = zone;
        if (!points.Contains(currentZone.cameraPosition)) points.Add(currentZone.cameraPosition);
    }

    public void ChangeCharacter()
    {
        character.player.ChangeNewCharacter();
    }

    public void UnlockedAllDoors()
    {
        GameManager.instance.cameraTracking.StopAllCoroutines();
        GameManager.instance.cameraTracking.GoTo(currentZone.cameraIndicativePosition);

        List<Door> doors = new List<Door>(FindObjectsOfType<Door>());
        foreach (DoorInfo doorInfo in doorInfo)
        {
            if(doorInfo.door == null)
            {
                doorInfo.door = doors.Find(d => d.id == doorInfo.id);
            }
            if (!doorInfo.unlocked && doorInfo.door.id != 5)
            {
                doorInfo.door.unlocked = true;
                doorInfo.unlocked = true;
                doorInfo.door.Open();
            }
            
        }
    }

    public void SaveCoins()
    {
        PlayerPrefs.SetInt("coins", playerCoins);
    }

    public void TpToMinigames(Transform tpPoint)
    {
        character.transform.position = tpPoint.position;
        cameraTracking.StopAllCoroutines();

        List<Zone> zone = new List<Zone>(FindObjectsOfType<Zone>());
        Zone lastZone = zone.Find(d => d.id == 5);
        if (!points.Contains(lastZone.cameraPosition))
        {
            points.Reverse();
            points.Add(lastZone.cameraPosition);
        }

        cameraTracking.GoTo(points);
        
        List<Door> doors = new List<Door>(FindObjectsOfType<Door>());
        foreach (DoorInfo doorInfo in doorInfo)
        {
            if (doorInfo.door == null)
            {
                doorInfo.door = doors.Find(d => d.id == doorInfo.id);
            }
            if (doorInfo.door.id == 5)
            {
                doorInfo.door.unlocked = true;
                doorInfo.unlocked = true;
                if(doorInfo.door.gameObject.activeSelf) doorInfo.door.Open();
            }
        }
    }
}
