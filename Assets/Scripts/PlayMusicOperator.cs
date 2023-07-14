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

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        BGM = gameObject.AddComponent<AudioSource>();
        BGM.loop = true;
        PlayBGMBySceneName();
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
        }
        else
        {
            if(BGM.clip!=null && BGM.clip.Equals(defaultBGM))
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
}