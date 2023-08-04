using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.IO;

[System.Serializable]
public class ReceiveWordData_W
{
    public int status;
    public bool success;
    public string message;
    public Data_S data;
    public Result_S result;
}

[System.Serializable]
public class Data_W
{
    public List<Answer_W> one_word;
    public List<Answer_W> two_word;
    public List<Answer_W> three_word;
    public List<Answer_W> four_word;
}

[System.Serializable]
public class Answer_W
{
    public int w_id;
    public string w_name;
    public string font_color;
}

[System.Serializable]
public class Result_W
{
    public List<PuzzleBlockWord_W> zero;
    public List<PuzzleBlockWord_W> one;
    public List<PuzzleBlockWord_W> two;
    public List<PuzzleBlockWord_W> three;
    public List<PuzzleBlockWord_W> four;
}

[System.Serializable]
public class PuzzleBlockWord_W
{
    public string word;
    public string color;
}

public class LoadWord_W : MonoBehaviour
{

    string url = "http://43.202.24.176:8080//api/reviewnotes/";

    public static LoadWord_W Instance;

    public List<Answer_W> answerList;
    public List<PuzzleBlockWord_W> wordListToPlace;

    // "Block" 태그가 붙은 모든 버튼을 가져온다.
    GameObject[] buttons;

    int userID, idx = 0;

    string filePath, userIDFileName = "UserID.json", userIDFile;

    private IEnumerator Start()
    {

        filePath = Path.Combine(Application.persistentDataPath, userIDFileName);
        userIDFile = File.ReadAllText(filePath);
        userID = JsonUtility.FromJson<ReceiveData>(userIDFile).user_id;

        buttons = GameObject.FindGameObjectsWithTag("Block");

        if(Instance == null)
        {
            Instance = this;
        }

        answerList = new List<Answer_W>();
        wordListToPlace = new List<PuzzleBlockWord_W>();
        yield return StartCoroutine(LoadPuzzleWord(url, userID));
        
        for(int i=0;i<wordListToPlace.Count;i++)
        {
            Debug.Log( i + "번째: " + wordListToPlace[i].word);
        }

        foreach(GameObject button in buttons)
        {
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

            if(wordListToPlace.Count == idx)
                break;

            buttonText.text = wordListToPlace[idx].word;
            SetTextColor(buttonText, wordListToPlace[idx].color);
            idx++;
        }
    }

    public void SetTextColor(TextMeshProUGUI buttonText,string colorCode)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(colorCode, out color))
        {
            buttonText.color = color;
        }
    }

    IEnumerator LoadPuzzleWord(string url, int id) // 오답노트 맵에 배치할 단어를 위한 Get 요청 후 보관
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        // id 변수를 "USER-ID"이라는 이름으로 헤더에 추가
        request.SetRequestHeader("USER-ID", id.ToString());

        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);

            ReceiveWordData_S tmp = JsonUtility.FromJson<ReceiveWordData_S>(request.downloadHandler.text);

            // 정답인 단어들의 정보를 모아둔다.
            for(int i=0;i<tmp.data.one_word.Count;i++)
            {
                Answer_W newAnswer = new Answer_W();
                newAnswer.w_id = tmp.data.one_word[i].w_id;
                newAnswer.w_name = tmp.data.one_word[i].w_name;
                newAnswer.font_color = tmp.data.one_word[i].font_color;
                answerList.Add(newAnswer);
            }
            for(int i=0;i<tmp.data.two_word.Count;i++)
            {
                Answer_W newAnswer = new Answer_W();
                newAnswer.w_id = tmp.data.two_word[i].w_id;
                newAnswer.w_name = tmp.data.two_word[i].w_name;
                newAnswer.font_color = tmp.data.two_word[i].font_color;
                answerList.Add(newAnswer); 
            }
            for(int i=0;i<tmp.data.three_word.Count;i++)
            {
                Answer_W newAnswer = new Answer_W();
                newAnswer.w_id = tmp.data.three_word[i].w_id;
                newAnswer.w_name = tmp.data.three_word[i].w_name;
                newAnswer.font_color = tmp.data.three_word[i].font_color;
                answerList.Add(newAnswer);
            }
            for(int i=0;i<tmp.data.four_word.Count;i++)
            {
                Answer_W newAnswer = new Answer_W();
                newAnswer.w_id = tmp.data.four_word[i].w_id;
                newAnswer.w_name = tmp.data.four_word[i].w_name;
                newAnswer.font_color = tmp.data.four_word[i].font_color;
                answerList.Add(newAnswer);
            }
            
            // 맵에 배치할 글자 정보를 모아둔다.
            for(int i=0;i<tmp.result.zero.Count;i++) // 0행
            {
                PuzzleBlockWord_W newPuzzleBlockWord_W = new PuzzleBlockWord_W();
                newPuzzleBlockWord_W.word = tmp.result.zero[i].word;
                newPuzzleBlockWord_W.color = tmp.result.zero[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_W);
            }

            for(int i=0;i<tmp.result.one.Count;i++) // 1행
            {
                PuzzleBlockWord_W newPuzzleBlockWord_W = new PuzzleBlockWord_W();
                newPuzzleBlockWord_W.word = tmp.result.one[i].word;
                newPuzzleBlockWord_W.color = tmp.result.one[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_W);
            }
             
            for(int i=0;i<tmp.result.two.Count;i++) // 2행
            {
                PuzzleBlockWord_W newPuzzleBlockWord_W = new PuzzleBlockWord_W();
                newPuzzleBlockWord_W.word = tmp.result.two[i].word;
                newPuzzleBlockWord_W.color = tmp.result.two[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_W);
            }

            for(int i=0;i<tmp.result.three.Count;i++) // 3행
            {
                PuzzleBlockWord_W newPuzzleBlockWord_W = new PuzzleBlockWord_W();
                newPuzzleBlockWord_W.word = tmp.result.three[i].word;
                newPuzzleBlockWord_W.color = tmp.result.three[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_W);
            }

            for(int i=0;i<tmp.result.four.Count;i++) // 4행
            {
                PuzzleBlockWord_W newPuzzleBlockWord_W = new PuzzleBlockWord_W();
                newPuzzleBlockWord_W.word = tmp.result.four[i].word;
                newPuzzleBlockWord_W.color = tmp.result.four[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_W);
            }
        }
    }
}