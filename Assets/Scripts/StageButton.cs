using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* 
이 스크립트를 지닌 버튼에 대한 이름으로 몇 스테이지에 진입했는지
알기 위해 만들었습니다.
*/
public class StageButton : MonoBehaviour
{
    Button btn;
    void Start()
    {
        btn = GetComponent<Button>();
    }

    public void SwitchPlayScene()
    {
        int stageNum = int.Parse(btn.name);
        LevelAndStageManager.Instance.SaveCurrentStage(stageNum);

        string sceneName = SceneManager.GetActiveScene().name; // 현재 씬 이름 가져오기
        
        SoundManager.Instance.PlaySFX("Click");

        if(sceneName == "ForestStageSelectionScene")
        {
            SceneManager.LoadScene("ForestPlayScene");
        }
        else if(sceneName == "DesertStageSelectionScene")
        {
            SceneManager.LoadScene("DesertPlayScene");
        }
        else if(sceneName == "OceanStageSelectionScene")
        {
            SceneManager.LoadScene("OceanPlayScene");
        }
        else if(sceneName == "PastureStageSelectionScene")
        {
            SceneManager.LoadScene("PasturePlayScene");
        }
        else if(sceneName == "SpaceStageSelectionScene")
        {
            SceneManager.LoadScene("SpacePlayScene");
        }
    }
}
