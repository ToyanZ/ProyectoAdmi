using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public bool skipMinigames = false;
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

    public void LoadRandomGame(string linkedLevel, float time)
    {
        StartCoroutine(LoadSceneIE(linkedLevel, time));
    }
    IEnumerator LoadSceneIE(string linkedLevel, float time)
    {
        yield return new WaitForSeconds(time);

        if(!skipMinigames)
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
        else
        {
            GameManager.instance.MiniGameCompleted();
        }
    }





    public void LoadGame(string gameName, string nextScene)
    {
        this.nextScene = nextScene;
        SceneManager.LoadScene(gameName);
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
