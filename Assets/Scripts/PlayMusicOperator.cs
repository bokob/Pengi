using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMusicOperator : MonoBehaviour
{
    [System.Serializable]
    public struct BgmType
    {
        public string sceneName;
        public AudioClip audio;
    }

    public BgmType[] BGMList;
    public AudioClip defaultBGM; // 기본 BGM

    private AudioSource BGM;
    private string currentSceneName = "";

    private static PlayMusicOperator instance;
    private bool isMuted; // 음소거 상태 확인 변수
    public static PlayMusicOperator Instance
    {
        get{return instance;}
    }

    void Awake()
    {
        // if (instance == null)
        // {
        //     instance = this;
        //     DontDestroyOnLoad(gameObject);
        // }
        // else
        // {
        //     Destroy(gameObject);
        //     return;
        // }

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        //isMuted = !DataManager.Instance.gameData.isBGM; // 음소거 상태 불러오기

        BGM = gameObject.AddComponent<AudioSource>();
        BGM.loop = true;
        PlayBGMBySceneName(); // 씬에 맞는 bgm 플레이
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void PlayBGMBySceneName()
    {
        if (isMuted) // 무음
        {
            BGM.Pause();
            return;
        }

        string sceneName = SceneManager.GetActiveScene().name;

        // 씬 4에서 다른 BGM으로 변경
        if (sceneName == "ForestPlayScene")
        {
            BGM.clip = GetBGMBySceneName(sceneName);
            BGM.Play();
            currentSceneName = sceneName;
        }
        else if(sceneName == "DesertPlayScene")
        {
            BGM.clip = GetBGMBySceneName(sceneName);
            BGM.Play();
            currentSceneName = sceneName;
        }
        else if(sceneName == "OceanPlayScene")
        {
            BGM.clip = GetBGMBySceneName(sceneName);
            BGM.Play();
            currentSceneName = sceneName;
        }
        else if(sceneName == "PasturePlayScene")
        {
            BGM.clip = GetBGMBySceneName(sceneName);
            BGM.Play();
            currentSceneName = sceneName;
        }
        else if(sceneName == "SpacePlayScene")
        {
            BGM.clip = GetBGMBySceneName(sceneName);
            BGM.Play();
            currentSceneName = sceneName;
        } /* 추가로 특정 씬에 맞게 재생하게끔 여기에 추가 예정 */
        else 
        {
            if(BGM.clip!=null && BGM.clip.Equals(defaultBGM)) // 만약 다른 씬인데 같은 bgm이면 중복재생 방지
                return;

            BGM.clip = defaultBGM;
            BGM.Play();
            currentSceneName = "";
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMBySceneName();
    }

    private AudioClip GetBGMBySceneName(string sceneName)
    {
        for (int i = 0; i < BGMList.Length; i++)
        {
            if (BGMList[i].sceneName.Equals(sceneName))
            {
                return BGMList[i].audio;
            }
        }

        return null;
    }


    public void StopBGM()
    {
        BGM.Stop();
        currentSceneName = "";
    }

    // public void ToggleMute()
    // {
    //     isMuted = !isMuted;

    //     if (isMuted)
    //     {
    //         BGM.Pause();
    //     }
    //     else
    //     {
    //         if (BGM.clip != null)
    //         {
    //             BGM.Play();
    //         }
    //         else
    //         {
    //             BGM.clip = defaultBGM;
    //             BGM.Play();
    //         }
    //     }
    // }
}