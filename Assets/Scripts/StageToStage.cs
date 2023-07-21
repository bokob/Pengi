using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/*
성공 창이나 실패 창에서

스테이지 선택창으로 가거나
이전 스테이지, 다음 스테이졸 가기 위한 스크립트
*/

public class StageToStage : MonoBehaviour
{
    // Start is called before the first frame update

    int currentLevel, currentStage;
    string sceneName;

    void Start()
    {
        currentLevel = LevelAndStageManager.Instance.currentLevel;
        currentStage = LevelAndStageManager.Instance.currentStage;

        if(GetComponent<Button>().name == "PreviousStageButton" && currentStage == 1)
            gameObject.SetActive(false);

        if(GetComponent<Button>().name == "NextStageButton" && currentStage == 10)
            gameObject.SetActive(false);
    }

    public void SwitchStageSelection() // 각 레벨에 맞는 스테이지 선택창으로 이동
    {
        switch(currentLevel)
        {
            case 1:
                sceneName = "ForestStageSelectionScene";
                break;
            case 2:
                sceneName = "DesertStageSelectionScene";
                break;
            case 3:
                sceneName = "OceanStageSelectionScene";
                break;
            case 4:
                sceneName = "PastureStageSelectionScene";
                break;
            case 5:
                sceneName = "SpaceStageSelectionScene";
                break;
        }
        SceneManager.LoadScene(sceneName);
    }

    public void PreviousStage() // 이전 스테이지로 이동(단, 1 스테이지면 버튼 비활성화)
    {
        if(GetComponent<Button>().name == "PreviousStageButton")
        {
            if(currentStage == 1)
            {
                return;
            }
            else
            {
                LevelAndStageManager.Instance.currentStage -= 1;
                switch(currentLevel)
                {
                    case 1:
                        sceneName = "ForestPlayScene";
                        break;
                    case 2:
                        sceneName = "DesertPlayScene";
                        break;
                    case 3:
                        sceneName = "OceanPlayScene";
                        break;
                    case 4:
                        sceneName = "PasturePlayScene";
                        break;
                    case 5:
                        sceneName = "SpacePlayScene";
                        break;
                }
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    public void NextStage() // 다음 스테이지로 이동(단, 10 스테이지면 버튼 비활성화)
    {
        if(GetComponent<Button>().name == "NextStageButton")
        {
            if(currentStage == 10)
            {
                return;
            }
            else
            {
                LevelAndStageManager.Instance.currentStage += 1;
                switch(currentLevel)
                {
                    case 1:
                        sceneName = "ForestPlayScene";
                        break;
                    case 2:
                        sceneName = "DesertPlayScene";
                        break;
                    case 3:
                        sceneName = "OceanPlayScene";
                        break;
                    case 4:
                        sceneName = "PasturePlayScene";
                        break;
                    case 5:
                        sceneName = "SpacePlayScene";
                        break;
                }
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}