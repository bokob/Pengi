using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailParticleCounter : MonoBehaviour
{
    public int targetCount; // 실패 기준
    private int particleCount = 0; // 현재 오브젝트의 수

    public GameObject fluidCamera; 
    public GameObject waterBatch;
    public GameObject panel;

    public BoxCollider2D countingArea;

    bool flag = false;

    private void Update()
    {
        // 영역 내의 WaterParticle 태그를 가진 활성화된 오브젝트들을 파악하여 개수를 세기
        Collider2D[] collidersInsideArea = Physics2D.OverlapBoxAll(countingArea.bounds.center, countingArea.bounds.size, 0f);
        particleCount = 0;

        foreach (var collider in collidersInsideArea)
        {
            if (collider.CompareTag("WaterParticle") && collider.gameObject.activeSelf)
            {
                particleCount++;
            }
        }

        // 개수가 maxObjectsAllowed를 초과하면 "실패" 메시지 출력
        if (particleCount < targetCount)
        {
            if(flag) 
                return;
            
            flag=true;
            
            if(flag) 
                GameFail();
        }
    }
    private void GameFail() // 게임 실패
    {
        Debug.Log("실패!");
        SoundManager.Instance.ToggleSFX();
        fluidCamera.SetActive(false);
        waterBatch.SetActive(false);
        SoundManager.Instance.ToggleSFX();
        panel.SetActive(true);
        STT.Instance.gameResult.completed="0";
        STT.Instance.Send();
    }
}