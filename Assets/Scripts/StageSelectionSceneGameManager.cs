using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StageSelectionSceneGameManager : MonoBehaviour // 스테이지 잠금 여부
{
    // Start is called before the first frame update
    int stage;
    void Start()
    {
        Button[] btns = GameObject.Find("StageSelection").GetComponentsInChildren<Button>();
        Debug.Log("가져옴");
        Debug.Log(btns.Length);

        string sceneName = SceneManager.GetActiveScene().name;
        
        if(sceneName == "ForestStageSelectionScene")
        {
            stage = DataManager.Instance.gameData.forestStage;
        }
        else if(sceneName == "DesertStageSelectionScene")
        {
            stage = DataManager.Instance.gameData.desertStage;
        }
        else if(sceneName == "OceanStageSelectionScene")
        {
            stage = DataManager.Instance.gameData.oceanStage;
        }
        else if(sceneName == "PastureStageSelectionScene")
        {
            stage = DataManager.Instance.gameData.pastureStage;
        }
        else if(sceneName == "SpaceStageSelectionScene")
        {
            stage = DataManager.Instance.gameData.spaceStage;
        }

        for(int i=0;i<btns.Length;i++)
        {
            if(i<=stage) // 열린 스테이지라면
            {
                Debug.Log(btns[i].name + "열립니다.");
                //btns[i].interactable=true;
            }
            else
            {
                Debug.Log(btns[i].name + "잠깁니다.");
                btns[i].interactable=false;
                btns[i].image.color = new Color(0.5f,0.5f,0.5f); 
            }
        }

        // 엔딩씬으로 넘어가기 위한 부분
        if(DataManager.Instance.gameData.last==true && DataManager.Instance.gameData.spaceClear[9])
        {
            DataManager.Instance.UpdateGameSetData(3);
            SceneManager.LoadScene("EndingScene");
        }
    }
}
