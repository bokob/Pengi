using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SuccessParticleCounter : MonoBehaviour
{
    public int targetCount; // 클리어를 위해 필요한 오브젝트의 수
    private int enterCount = 0; // 현재 오브젝트의 수

    public GameObject fluidCamera; 
    public GameObject waterBatch;
    public GameObject panel;
    public GameObject allParticleCountZone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WaterParticle"))
        {
            enterCount++; // 오브젝트가 특정 영역에 진입하면 카운트 증가
            Debug.Log(enterCount);

            if (enterCount >= targetCount)
            {
                GameClear(); // 클리어 조건 충족 시 게임 클리어 처리
            }
        }
    }

    private void GameClear() // 게임 클리어
    {
        Debug.Log("성공!");
        SoundManager.Instance.ToggleSFX();
        fluidCamera.SetActive(false);
        allParticleCountZone.SetActive(false);
        waterBatch.SetActive(false);
        SoundManager.Instance.ToggleSFX();
        panel.SetActive(true);

        if(SceneManager.GetActiveScene().name == "DemoPlayScene")
        {
            DataManager.Instance.UpdateGameClear(1, 1);
        }
        else
        {
            DataManager.Instance.UpdateGameClear(LevelAndStageManager.Instance.currentLevel, LevelAndStageManager.Instance.currentStage);
            STT.Instance.gameResult.completed="1";
            STT.Instance.Send();
        }
    }
}