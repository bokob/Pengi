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
        Debug.Log("레벨: " + DataManager.Instance.gameData.level);
        Debug.Log("스테이지: " + DataManager.Instance.gameData.stage);
        Debug.Log("bgm 켤지: " + DataManager.Instance.gameData.isBGM);
        Debug.Log("효과음 켤지: " + DataManager.Instance.gameData.isEffectSound);
    }
}
