using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LavaAndAcid : MonoBehaviour
{
    void Start()
    {
        //Destroy(this,delay); // 스크립트 해제
        //StartCoroutine(DelayedFunction());
    }

    // private IEnumerator DelayedFunction()
    // {
    //     yield return new WaitForSeconds(delay);

    //     ChangeToLavaOrAcidImage();
    // }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // Debug.Log(other.name + " 가 닿았습니다.");

        if(other.CompareTag("WaterParticle"))
        {
            // Debug.Log(other.name + " 이 비활성화 됩니다.");
            other.gameObject.SetActive(false);
        }    
    }
}
