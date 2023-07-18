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
        for(int i=0;i<btns.Length;i++)
        {
            /* 
                json에 level 1로 기록되어 있으면  0 < 1 이므로 숲이 열림 이런식으로 작동
            */
            if(i<=DataManager.Instance.gameData.level) // 열린 레벨이라면
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
    }
}
