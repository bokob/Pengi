using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/* 
게임 클리어 여부와 
어디 레벨의 몇 스테이지의 정보를 가지고 있기 위해 만들었음
*/

public class LevelAndStageManager : MonoBehaviour 
{
    public static LevelAndStageManager Instance;

    public int currentLevel;
    public int currentStage;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SaveCurrentLevel();
    }

    public void SaveCurrentLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if(sceneName == "ForestStageSelectionScene")
        {
            currentLevel = 1;
        }
        else if(sceneName == "DesertStageSelectionScene")
        {
            currentLevel = 2;
        }
        else if(sceneName == "OceanStageSelectionScene")
        {
            currentLevel = 3;
        }
        else if(sceneName == "PastureStageSelectionScene")
        {
            currentLevel = 4;
        }
        else if(sceneName == "SpaceStageSelectionScene")
        {
            currentLevel = 5;
        }
    }

    public void SaveCurrentStage(int stageNum)
    {
        currentStage = stageNum;
    }
}
