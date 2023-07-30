using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

[System.Serializable]
public class ReceiveWordData_F
{
    public int status;
    public bool success;
    public string message;
    public Data_F data;
    public Result_F result;
}

[System.Serializable]
public class Data_F
{
    public List<Answer_F> one_word;
    public List<Answer_F> two_word;
    public List<Answer_F> three_word;
    public List<Answer_F> four_word;
}

[System.Serializable]
public class Answer_F
{
    public int w_id;
    public string w_name;
    public string font_color;
}

[System.Serializable]
public class Result_F
{
    public List<PuzzleBlockWord_F> zero;
    public List<PuzzleBlockWord_F> one;
    public List<PuzzleBlockWord_F> two;
}

[System.Serializable]
public class PuzzleBlockWord_F
{
    public string word;
    public string color;
}

public class LoadWord_F : MonoBehaviour
{

    string url = "http://43.202.24.176:8080//api/wordlist/";
    int level;

    public static LoadWord_F Instance;

    public List<Answer_F> answerList;
    public List<PuzzleBlockWord_F> wordListToPlace;

    // "Block" 태그가 붙은 모든 버튼을 가져온다.
    GameObject[] buttons;

    int idx=0;

    private IEnumerator Start()
    {
        buttons = GameObject.FindGameObjectsWithTag("Block");

        level = LevelAndStageManager.Instance.currentLevel;
        answerList = new List<Answer_F>();
        wordListToPlace = new List<PuzzleBlockWord_F>();
        yield return StartCoroutine(LoadPuzzleWord(url, level));
        
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

    IEnumerator LoadPuzzleWord(string url, int level) // 맵에 배치할 단어를 위한 Get 요청 후 보관
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

            ReceiveWordData_F tmp = JsonUtility.FromJson<ReceiveWordData_F>(request.downloadHandler.text);

            // 정답인 단어들의 정보를 모아둔다.
            for(int i=0;i<tmp.data.one_word.Count;i++)
            {
                Answer_F newAnswer = new Answer_F();
                newAnswer.w_id = tmp.data.one_word[i].w_id;
                newAnswer.w_name = tmp.data.one_word[i].w_name;
                newAnswer.font_color = tmp.data.one_word[i].font_color;
                answerList.Add(newAnswer);
            }
            for(int i=0;i<tmp.data.two_word.Count;i++)
            {
                Answer_F newAnswer = new Answer_F();
                newAnswer.w_id = tmp.data.two_word[i].w_id;
                newAnswer.w_name = tmp.data.two_word[i].w_name;
                newAnswer.font_color = tmp.data.two_word[i].font_color;
                answerList.Add(newAnswer); 
            }
            for(int i=0;i<tmp.data.three_word.Count;i++)
            {
                Answer_F newAnswer = new Answer_F();
                newAnswer.w_id = tmp.data.three_word[i].w_id;
                newAnswer.w_name = tmp.data.three_word[i].w_name;
                newAnswer.font_color = tmp.data.three_word[i].font_color;
                answerList.Add(newAnswer);
            }
            for(int i=0;i<tmp.data.four_word.Count;i++)
            {
                Answer_F newAnswer = new Answer_F();
                newAnswer.w_id = tmp.data.four_word[i].w_id;
                newAnswer.w_name = tmp.data.four_word[i].w_name;
                newAnswer.font_color = tmp.data.four_word[i].font_color;
                answerList.Add(newAnswer);
            }
            

            // 맵에 배치할 글자 정보를 모아둔다.
            for(int i=0;i<tmp.result.zero.Count;i++) // 0행
            {
                PuzzleBlockWord_F newPuzzleBlockWord_F = new PuzzleBlockWord_F();
                newPuzzleBlockWord_F.word = tmp.result.zero[i].word;
                newPuzzleBlockWord_F.color = tmp.result.zero[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_F);
            }

            for(int i=0;i<tmp.result.one.Count;i++) // 1행
            {
                PuzzleBlockWord_F newPuzzleBlockWord_F = new PuzzleBlockWord_F();
                newPuzzleBlockWord_F.word = tmp.result.one[i].word;
                newPuzzleBlockWord_F.color = tmp.result.one[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_F);
            }
             
            for(int i=0;i<tmp.result.two.Count;i++) // 2행
            {
                PuzzleBlockWord_F newPuzzleBlockWord_F = new PuzzleBlockWord_F();
                newPuzzleBlockWord_F.word = tmp.result.two[i].word;
                newPuzzleBlockWord_F.color = tmp.result.two[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_F);
            }

            PuzzleBlockWord_F emptyPuzzleBlockWord_F = new PuzzleBlockWord_F();
            emptyPuzzleBlockWord_F.word="";
            emptyPuzzleBlockWord_F.color="";
            wordListToPlace.Insert(8, emptyPuzzleBlockWord_F);
            wordListToPlace.Add(emptyPuzzleBlockWord_F);
        }
    }

    // public int GetWordId(string word)
    // {
    //     for(int i=0;i<)

    //     return 
    // }
}