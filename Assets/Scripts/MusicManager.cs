using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    static GameObject container;
    static MusicManager  instance;
    public static MusicManager  Instance // 싱글톤으로 만든 후에 게임에서 계속 존재하게 하기
    {
        get
        {
            if (!instance)
            {
                container = new GameObject();
                container.name = "MusicManager";
                instance = container.AddComponent(typeof(MusicManager )) as MusicManager ;
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }

    private AudioSource audioSource;
    private Dictionary<string, AudioClip> bgmDictionary; // 씬에 따른 bgm 딕셔너리 생성

    private void Awake()
    {
        InitializeBGM();
        audioSource = GetComponent<AudioSource>();
        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.loop = true; // 반복재생
        audioSource.clip = Resources.Load<AudioClip>("BGM/MainBGM");

        if(audioSource.clip == null)
            Debug.Log("음악 담긴게 없는데...");

        SceneManager.sceneLoaded += (scene, mode) =>
        {
            OnSceneLoaded(scene, mode);
        };
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void InitializeBGM()
    {
        bgmDictionary = new Dictionary<string, AudioClip>();

        // 각 씬에 따른 BGM 등록
        bgmDictionary.Add("MainScene", Resources.Load<AudioClip>("BGM/MainBGM"));           // 메인
        bgmDictionary.Add("ForestPlayScene", Resources.Load<AudioClip>("BGM/ForestBGM"));   // 숲
        bgmDictionary.Add("DesertPlayScene", Resources.Load<AudioClip>("BGM/DesertBGM"));   // 사막
        bgmDictionary.Add("OceanPlayScene", Resources.Load<AudioClip>("BGM/OceanBGM"));     // 해변
        bgmDictionary.Add("PasturePlayScene", Resources.Load<AudioClip>("BGM/PastureBGM")); // 초원
        bgmDictionary.Add("SpacePlayScene", Resources.Load<AudioClip>("BGM/SpaceBGM"));     // 우주
        // 추가적인 씬에 대한 설정 적는 곳
        bgmDictionary.Add("EndingScene", Resources.Load<AudioClip>("BGM/EndingBGM"));       // 엔딩
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ChangeBGM(scene.name);
    }

    public void ChangeBGM(string sceneName)
    {
        AudioClip bgmClip = null;

        if (bgmDictionary.ContainsKey(sceneName)) // 씬 이름을 키로 사용하여 BGM을 가져온다
        {
            bgmClip = bgmDictionary[sceneName];
            Debug.Log(sceneName + "브금 재생");
        }
        else // 지정된 씬에 대한 BGM이 없는 경우, 기본 BGM을 설정
        {
            bgmClip = Resources.Load<AudioClip>("BGM/DefaultBGM");
        }

        if (bgmClip != null)
        {
            audioSource.clip = bgmClip;
            audioSource.Play();
        }
    }
}
