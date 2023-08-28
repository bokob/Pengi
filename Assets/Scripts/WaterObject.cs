using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterObject : MonoBehaviour
{
    public int minMovingParticlesThreshold = 50; // 움직이는 입자의 최소 임계값
    private int movingParticlesCount = 0; // 움직이는 입자 개수

    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 움직이는 입자의 개수를 갱신
        CountMovingParticles();

        // 움직이는 입자 개수가 임계값 이하면 소리 멈춤
        if (movingParticlesCount < minMovingParticlesThreshold)
        {
            //Debug.Log("끌거야");
            audioSource.volume=0;
            //  SoundManager.Instance.PlaySFX("SlowWater");
        }
        else // 이상
        {
            //Debug.Log("켤거야");
            audioSource.volume = SoundManager.Instance.sfxSource.volume;
        }
    }

    private void CountMovingParticles()
    {
        movingParticlesCount = 0;
        WaterParticle[] particles = GetComponentsInChildren<WaterParticle>();

        foreach (WaterParticle particle in particles)
        {
            if (particle.IsMoving())
            {
                movingParticlesCount++;
            }
        }
    }

    // private System.Collections.IEnumerator TurnOffSFX(float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     audioSource.mute=false;
    // }

    // private System.Collections.IEnumerator TurnOnSFX(float delay)
    // {
    //     audioSource.mute=true;
    //     yield return new WaitForSeconds(delay);
    // }
}
