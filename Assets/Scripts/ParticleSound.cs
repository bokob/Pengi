using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSound : MonoBehaviour
{
    private Vector2 previousPosition;
    void Start()
    {
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 위치와 이전 위치를 비교하여 이동 벡터 계산
        Vector2 currentPosition = transform.position;
        Vector2 movementVector = currentPosition - previousPosition;

        // 이동 벡터 크기를 기준으로 움직임 감지
        float movementThreshold = 0.15f; // 이동 벡터 크기 임계값
        if (movementVector.magnitude > movementThreshold)
        {
            // 물 흐르는 소리 재생
            SoundManager.Instance.PlaySFX("FlowWater");
        }

        // 이전 위치 업데이트
        previousPosition = currentPosition;
    }
}
