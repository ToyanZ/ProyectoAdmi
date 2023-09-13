using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelArea : MonoBehaviour
{
    public enum LoadMode { Random, ByName}
    public string linkedSceneName;

    [Space(20)]
    public LoadMode gameLoadMode = LoadMode.Random;
    public string linkedGameName;

    public void Load()
    {
        switch (gameLoadMode)
        {
            case LoadMode.Random:
                LevelManager.instance.LoadRandomGame(linkedSceneName);
                break; 
            case LoadMode.ByName:
                LevelManager.instance.LoadGame(linkedGameName, linkedSceneName);
                break;
        }
        
    }
}
