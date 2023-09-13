using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public List<string> miniGamesSceneNames;
    string nextScene;
    private void Awake()
    {
        instance = this;
    }

    public void LoadGame(string gameName, string nextScene)
    {
        this.nextScene = nextScene;
        SceneManager.LoadScene(gameName);
    }
    public void LoadRandomGame(string linkedLevel)
    {
        nextScene = linkedLevel;
        int index = Random.Range(0, miniGamesSceneNames.Count);
        SceneManager.LoadScene(miniGamesSceneNames[index]);
    }
    public void LoadLinkedLevel()
    {
        SceneManager.LoadScene(nextScene);
    }
}
