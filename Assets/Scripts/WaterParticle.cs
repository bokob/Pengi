using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterParticle : MonoBehaviour
{
    private bool isMoving = false; // 움직임 여부를 저장하는 변수
    private Rigidbody2D rb; // 물 입자 오브젝트의 Rigidbody 컴포넌트

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 입자의 움직임 여부를 체크하여 isMoving 변수에 저장
        isMoving = rb.velocity.magnitude > 0.1f;
    }

    public bool IsMoving()
    {
        return isMoving;
    }
}
