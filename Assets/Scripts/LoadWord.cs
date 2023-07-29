using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class ReceiveWordData
{
    public int status;
    public bool success;
    public string message;
    public Data data;
    public List<PuzzleRow>result;
}

[System.Serializable]
public class Data
{
    public List<Answer> one_word;
    public List<Answer> two_word;
    public List<Answer> three_word;
    public List<Answer> four_word;
}

[System.Serializable]
public class Answer
{
    public int w_id;
    public string w_name;
    public string font_color;
}

[System.Serializable]
public class PuzzleRow
{
    List<PuzzleBlockWord> row;
}

[System.Serializable]
public class PuzzleBlockWord
{
    public string word;
    public string color;
}


public class LoadWord : MonoBehaviour
{

    string url = "http://43.202.24.176:8080//api/wordlist";
    int level;

    public static LoadWord Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


        level = LevelAndStageManager.Instance.currentLevel;
        StartCoroutine(LoadPuzzleWord(url, level));
    }

    IEnumerator LoadPuzzleWord(string url, int level) // 맵에 배치할 단어를 위한 Get 요청
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        // level 변수를 "GAME-LEVEL"이라는 이름으로 헤더에 추가
        request.SetRequestHeader("GAME-LEVEL", level.ToString());

        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);

            // ReceiveWordData tmp = JsonUtility.FromJson<ReceiveWordData>(request.downloadHandler.text);

            // Debug.Log("과연 될까요?");
            // Debug.Log(tmp);
        }
    }
}