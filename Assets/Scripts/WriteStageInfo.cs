using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 성공했을 때 스테이지 정보를 나타내기 위해 사용하는 스크립트
public class WriteStageInfo : MonoBehaviour
{
    TextMeshProUGUI stageInfo; // 몇 레벨 몇 스테이지인지 표시하는 UI를 가리킬 변수
    void Start()
    {
        stageInfo = GetComponent<TextMeshProUGUI>();
        Debug.Log("레벨: " + LevelAndStageManager.Instance.currentLevel.ToString());
        Debug.Log("스테이지: " + LevelAndStageManager.Instance.currentStage.ToString());
        stageInfo.text += LevelAndStageManager.Instance.currentLevel.ToString() + " - " + LevelAndStageManager.Instance.currentStage.ToString();
    }
}
