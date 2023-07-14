using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BGMPlay : MonoBehaviour
{
    void Start()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        MusicManager.Instance.ChangeBGM(currentSceneName);
        Debug.Log("현재 씬의 이름은: " + currentSceneName);
    }
    // private AudioSource audioSource;
    // private GameObject[] bgms;
    // private void Awake()
    // {
    //     bgms = GameObject.FindGameObjectsWithTag("bgm"); // bgm 태그 가진 오브젝트 찾기

    //     // for(int i=0;i<bgms.Length;i++)
    //     //     Debug.Log(bgms[i].name);

    //     if(bgms.Length >=2) // 장면 전환 했을 때 전에 재생된 브금 파괴
    //     {
    //         Destroy(bgms[0]);
    //         //Destroy(this.gameObject);
    //     }
    //     DontDestroyOnLoad(transform.gameObject);
    //     audioSource = GetComponent<AudioSource>();
    // }

    // public void PlayBGM() // 재생
    // {
    //     if(audioSource.isPlaying)
    //         return;
    //     audioSource.Play();
    // }

    // public void StopBGM() // 멈춤
    // {
    //     audioSource.Stop();
    // }
}
