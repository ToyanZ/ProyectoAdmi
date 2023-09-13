using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public List<MiniGameInfo> miniGamesInfo;
    string nextScene;
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

    public void LoadGame(string gameName, string nextScene)
    {
        this.nextScene = nextScene;
        SceneManager.LoadScene(gameName);
    }
    public void LoadRandomGame(string linkedLevel)
    {
        nextScene = linkedLevel;

        int index = Random.Range(0, miniGamesInfo.Count);
        MiniGameInfo miniGameInfo = miniGamesInfo[index];
        while (miniGameInfo.played)
        {
            index = Random.Range(0, miniGamesInfo.Count);
            miniGameInfo = miniGamesInfo[index];
        }
        miniGameInfo.played = true;
        SceneManager.LoadScene(miniGameInfo.name);
    }
    public void LoadLinkedLevel()
    {
        SceneManager.LoadScene(nextScene);
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
