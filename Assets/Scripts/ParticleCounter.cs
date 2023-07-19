using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCounter : MonoBehaviour
{
    public int targetCount = 10; // 클리어를 위해 필요한 오브젝트의 수
    private int currentCount = 0; // 현재 오브젝트의 수

    public GameObject fluidCamera; 
    public GameObject waterBatch;
    public GameObject successPanel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WaterParticle"))
        {
            currentCount++; // 오브젝트가 특정 영역에 진입하면 카운트 증가
            Debug.Log(currentCount);

            if (currentCount >= targetCount)
            {
                GameClear(); // 클리어 조건 충족 시 게임 클리어 처리
            }
        }
    }

    private void GameClear()
    {
        // 게임 클리어 처리 작성
        Debug.Log("게임 클리어!");
        SoundManager.Instance.ToggleSFX();
        fluidCamera.SetActive(false);
        waterBatch.SetActive(false);
        SoundManager.Instance.ToggleSFX();
        successPanel.SetActive(true);
    }
}
