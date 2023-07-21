using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionSceneGameManager : MonoBehaviour // 레벨 잠금 여부
{
    void Start()
    {
        Button[] btns = GameObject.Find("LevelSelection").GetComponentsInChildren<Button>();
        Debug.Log("가져옴");
        Debug.Log(btns.Length);

        // btns[0]은 뒤로가기 버튼
        // 숲(btns[1])은 항상 열려있게 한다.
        int forestClearCnt=0, desertClearCnt=0, oceanClearCnt=0, pastureClearCnt=0;
        
        // 사막 열게 하기
        for(int i=0;i<10;i++) // 숲 클리어 조사
        {
            if(DataManager.Instance.gameData.forestClear[i])
                forestClearCnt++;
        }
        if(forestClearCnt<10) // 숲이 전부 클리어 되지 않았다면  
        {
            Debug.Log(btns[2].name + "잠깁니다.");
            btns[2].interactable=false;
            btns[2].image.color = new Color(0.5f,0.5f,0.5f);
        }

        // 해변 열게 하기
        for(int i=0;i<10;i++) // 사막 클리어 조사
        {
            if(DataManager.Instance.gameData.desertClear[i])
                desertClearCnt++;
        }
        if(desertClearCnt<10) // 숲이 전부 클리어 되지 않았다면  
        {
            Debug.Log(btns[3].name + "잠깁니다.");
            btns[3].interactable=false;
            btns[3].image.color = new Color(0.5f,0.5f,0.5f);
        }

        // 초원 열게 하기
        for(int i=0;i<10;i++) // 해변 클리어 조사
        {
            if(DataManager.Instance.gameData.oceanClear[i])
                oceanClearCnt++;
        }
        if(oceanClearCnt<10) // 해변이 전부 클리어 되지 않았다면  
        {
            Debug.Log(btns[4].name + "잠깁니다.");
            btns[4].interactable=false;
            btns[4].image.color = new Color(0.5f,0.5f,0.5f);
        }

        // 우주 열게 하기
        for(int i=0;i<10;i++) // 초원 클리어 조사
        {
            if(DataManager.Instance.gameData.pastureClear[i])
                oceanClearCnt++;
        }
        if(pastureClearCnt<10) // 초원이 전부 클리어 되지 않았다면  
        {
            Debug.Log(btns[5].name + "잠깁니다.");
            btns[5].interactable=false;
            btns[5].image.color = new Color(0.5f,0.5f,0.5f);
        }

        // for(int i=0;i<btns.Length;i++)
        // {
        //     /* 
        //         json에 level 1로 기록되어 있으면  0 < 1 이므로 숲이 열림 이런식으로 작동
        //     */
        //     if(i<=DataManager.Instance.gameData.recentLevel) // 열린 레벨이라면
        //     {
        //         Debug.Log(btns[i].name + "열립니다.");
        //         //btns[i].interactable=true;
        //     }
        //     else
        //     {
        //         Debug.Log(btns[i].name + "잠깁니다.");
        //         btns[i].interactable=false;
        //         btns[i].image.color = new Color(0.5f,0.5f,0.5f); 
        //     }
        // }
    }
}
