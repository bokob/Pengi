using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnOff : MonoBehaviour
{
    public AudioSource backgroundMusic;
    private bool isMusicOn=true;
    private float savedTime = 0f; // 음악이 재생되던 시간 보관
    public GameObject targetChildComponent; // 여기에 음소거 선을 넣어준다.
    private bool isChildVisible = true;

    public void SoundPlayStop()
    {

        // 음소거 선 가시/비가시
        isChildVisible = !isChildVisible;
        targetChildComponent.SetActive(isChildVisible);

        // 배경음악 on/off
        isMusicOn = !isMusicOn;
        if(isMusicOn)
        {
            backgroundMusic.time = savedTime;
            backgroundMusic.Play();
        }
        else
        {
            savedTime = backgroundMusic.time;
            backgroundMusic.Stop();
        }
    }
}
