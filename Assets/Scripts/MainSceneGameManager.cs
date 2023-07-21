using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DataManager.Instance.LoadGameData(); // 불러오기

        Debug.Log("첫 시작인지: " + DataManager.Instance.gameData.first);
        Debug.Log("최신 레벨: " + DataManager.Instance.gameData.recentLevel);
        Debug.Log("최신 스테이지: " + DataManager.Instance.gameData.recentStage);
        Debug.Log("bgm 소리 크기: " + DataManager.Instance.gameData.bgmVolume);
        Debug.Log("효과음 소리 크기: " + DataManager.Instance.gameData.sfxVolume);
    }
}
