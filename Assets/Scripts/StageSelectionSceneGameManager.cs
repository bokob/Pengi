using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StageSelectionSceneGameManager : MonoBehaviour // 스테이지 잠금 여부
{
    // Start is called before the first frame update
    void Start()
    {
        Button[] btns = GameObject.Find("StageSelection").GetComponentsInChildren<Button>();
        Debug.Log("가져옴");
        Debug.Log(btns.Length);
        for(int i=0;i<btns.Length;i++)
        {
            if(i<=DataManager.Instance.gameData.stage) // 열린 스테이지라면
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
