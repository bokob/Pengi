using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

[System.Serializable]
public class ReceiveWordData_P
{
    public int status;
    public bool success;
    public string message;
    public Data_P data;
    public Result_P result;
}

[System.Serializable]
public class Data_P
{
    public List<Answer_P> one_word;
    public List<Answer_P> two_word;
    public List<Answer_P> three_word;
    public List<Answer_P> four_word;
}

[System.Serializable]
public class Answer_P
{
    public int w_id;
    public string w_name;
    public string font_color;
}

[System.Serializable]
public class Result_P
{
    public List<PuzzleBlockWord_P> zero;
    public List<PuzzleBlockWord_P> one;
    public List<PuzzleBlockWord_P> two;
    public List<PuzzleBlockWord_P> three;
    public List<PuzzleBlockWord_P> four;
    public List<PuzzleBlockWord_P> five;
}

[System.Serializable]
public class PuzzleBlockWord_P
{
    public string word;
    public string color;
}

public class LoadWord_P : MonoBehaviour
{

    string url = "http://43.202.24.176:8080//api/wordlist/";
    int level;

    public static LoadWord_P Instance;

    public List<Answer_P> answerList;
    public List<PuzzleBlockWord_P> wordListToPlace;

    // "Block" 태그가 붙은 모든 버튼을 가져온다.
    GameObject[] buttons;

    int idx=0;

    private IEnumerator Start()
    {
        buttons = GameObject.FindGameObjectsWithTag("Block");

        level = LevelAndStageManager.Instance.currentLevel;
        answerList = new List<Answer_P>();
        wordListToPlace = new List<PuzzleBlockWord_P>();
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

            ReceiveWordData_P tmp = JsonUtility.FromJson<ReceiveWordData_P>(request.downloadHandler.text);

            // 정답인 단어들의 정보를 모아둔다.
            for(int i=0;i<tmp.data.one_word.Count;i++)
            {
                Answer_P newAnswer = new Answer_P();
                newAnswer.w_id = tmp.data.one_word[i].w_id;
                newAnswer.w_name = tmp.data.one_word[i].w_name;
                newAnswer.font_color = tmp.data.one_word[i].font_color;
                answerList.Add(newAnswer);
            }
            for(int i=0;i<tmp.data.two_word.Count;i++)
            {
                Answer_P newAnswer = new Answer_P();
                newAnswer.w_id = tmp.data.two_word[i].w_id;
                newAnswer.w_name = tmp.data.two_word[i].w_name;
                newAnswer.font_color = tmp.data.two_word[i].font_color;
                answerList.Add(newAnswer); 
            }
            for(int i=0;i<tmp.data.three_word.Count;i++)
            {
                Answer_P newAnswer = new Answer_P();
                newAnswer.w_id = tmp.data.three_word[i].w_id;
                newAnswer.w_name = tmp.data.three_word[i].w_name;
                newAnswer.font_color = tmp.data.three_word[i].font_color;
                answerList.Add(newAnswer);
            }
            for(int i=0;i<tmp.data.four_word.Count;i++)
            {
                Answer_P newAnswer = new Answer_P();
                newAnswer.w_id = tmp.data.four_word[i].w_id;
                newAnswer.w_name = tmp.data.four_word[i].w_name;
                newAnswer.font_color = tmp.data.four_word[i].font_color;
                answerList.Add(newAnswer);
            }
            

            // 맵에 배치할 글자 정보를 모아둔다.
            for(int i=0;i<tmp.result.zero.Count;i++) // 0행
            {
                PuzzleBlockWord_P newPuzzleBlockWord_P = new PuzzleBlockWord_P();
                newPuzzleBlockWord_P.word = tmp.result.zero[i].word;
                newPuzzleBlockWord_P.color = tmp.result.zero[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_P);
            }

            for(int i=0;i<tmp.result.one.Count;i++) // 1행
            {
                PuzzleBlockWord_P newPuzzleBlockWord_P = new PuzzleBlockWord_P();
                newPuzzleBlockWord_P.word = tmp.result.one[i].word;
                newPuzzleBlockWord_P.color = tmp.result.one[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_P);
            }
             
            for(int i=0;i<tmp.result.two.Count;i++) // 2행
            {
                PuzzleBlockWord_P newPuzzleBlockWord_P = new PuzzleBlockWord_P();
                newPuzzleBlockWord_P.word = tmp.result.two[i].word;
                newPuzzleBlockWord_P.color = tmp.result.two[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_P);
            }

            for(int i=0;i<tmp.result.three.Count;i++) // 3행
            {
                PuzzleBlockWord_P newPuzzleBlockWord_P = new PuzzleBlockWord_P();
                newPuzzleBlockWord_P.word = tmp.result.three[i].word;
                newPuzzleBlockWord_P.color = tmp.result.three[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_P);
            }

            for(int i=0;i<tmp.result.four.Count;i++) // 4행
            {
                PuzzleBlockWord_P newPuzzleBlockWord_P = new PuzzleBlockWord_P();
                newPuzzleBlockWord_P.word = tmp.result.four[i].word;
                newPuzzleBlockWord_P.color = tmp.result.four[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_P);
            }

            for(int i=0;i<tmp.result.five.Count;i++) // 5행
            {
                PuzzleBlockWord_P newPuzzleBlockWord_P = new PuzzleBlockWord_P();
                newPuzzleBlockWord_P.word = tmp.result.five[i].word;
                newPuzzleBlockWord_P.color = tmp.result.five[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_P);
            }


            PuzzleBlockWord_P emptyPuzzleBlockWord_P = new PuzzleBlockWord_P();
            emptyPuzzleBlockWord_P.word="";
            emptyPuzzleBlockWord_P.color="";
            wordListToPlace.Insert(16, emptyPuzzleBlockWord_P);
            wordListToPlace.Add(emptyPuzzleBlockWord_P);
        }
    }
}