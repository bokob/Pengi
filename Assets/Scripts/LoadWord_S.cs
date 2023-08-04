using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

[System.Serializable]
public class ReceiveWordData_S
{
    public int status;
    public bool success;
    public string message;
    public Data_S data;
    public Result_S result;
}

[System.Serializable]
public class Data_S
{
    public List<Answer_S> one_word;
    public List<Answer_S> two_word;
    public List<Answer_S> three_word;
    public List<Answer_S> four_word;
}

[System.Serializable]
public class Answer_S
{
    public int w_id;
    public string w_name;
    public string font_color;
}

[System.Serializable]
public class Result_S
{
    public List<PuzzleBlockWord_S> zero;
    public List<PuzzleBlockWord_S> one;
    public List<PuzzleBlockWord_S> two;
    public List<PuzzleBlockWord_S> three;
    public List<PuzzleBlockWord_S> four;
    public List<PuzzleBlockWord_S> five;
    public List<PuzzleBlockWord_S> six;
}

[System.Serializable]
public class PuzzleBlockWord_S
{
    public string word;
    public string color;
}

public class LoadWord_S : MonoBehaviour
{

    string url = "http://43.202.24.176:8080//api/wordlist/";
    int level;

    public static LoadWord_S Instance;

    public List<Answer_S> answerList;
    public List<PuzzleBlockWord_S> wordListToPlace;

    // "Block" 태그가 붙은 모든 버튼을 가져온다.
    GameObject[] buttons;

    int idx=0;

    private IEnumerator Start()
    {
        buttons = GameObject.FindGameObjectsWithTag("Block");

        level = LevelAndStageManager.Instance.currentLevel;

        if(Instance == null)
        {
            Instance = this;
        }

        answerList = new List<Answer_S>();
        wordListToPlace = new List<PuzzleBlockWord_S>();
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

            ReceiveWordData_S tmp = JsonUtility.FromJson<ReceiveWordData_S>(request.downloadHandler.text);

            // 정답인 단어들의 정보를 모아둔다.
            for(int i=0;i<tmp.data.one_word.Count;i++)
            {
                Answer_S newAnswer = new Answer_S();
                newAnswer.w_id = tmp.data.one_word[i].w_id;
                newAnswer.w_name = tmp.data.one_word[i].w_name;
                newAnswer.font_color = tmp.data.one_word[i].font_color;
                answerList.Add(newAnswer);
            }
            for(int i=0;i<tmp.data.two_word.Count;i++)
            {
                Answer_S newAnswer = new Answer_S();
                newAnswer.w_id = tmp.data.two_word[i].w_id;
                newAnswer.w_name = tmp.data.two_word[i].w_name;
                newAnswer.font_color = tmp.data.two_word[i].font_color;
                answerList.Add(newAnswer); 
            }
            for(int i=0;i<tmp.data.three_word.Count;i++)
            {
                Answer_S newAnswer = new Answer_S();
                newAnswer.w_id = tmp.data.three_word[i].w_id;
                newAnswer.w_name = tmp.data.three_word[i].w_name;
                newAnswer.font_color = tmp.data.three_word[i].font_color;
                answerList.Add(newAnswer);
            }
            for(int i=0;i<tmp.data.four_word.Count;i++)
            {
                Answer_S newAnswer = new Answer_S();
                newAnswer.w_id = tmp.data.four_word[i].w_id;
                newAnswer.w_name = tmp.data.four_word[i].w_name;
                newAnswer.font_color = tmp.data.four_word[i].font_color;
                answerList.Add(newAnswer);
            }
            

            // 맵에 배치할 글자 정보를 모아둔다.
            for(int i=0;i<tmp.result.zero.Count;i++) // 0행
            {
                PuzzleBlockWord_S newPuzzleBlockWord_S = new PuzzleBlockWord_S();
                newPuzzleBlockWord_S.word = tmp.result.zero[i].word;
                newPuzzleBlockWord_S.color = tmp.result.zero[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_S);
            }

            for(int i=0;i<tmp.result.one.Count;i++) // 1행
            {
                PuzzleBlockWord_S newPuzzleBlockWord_S = new PuzzleBlockWord_S();
                newPuzzleBlockWord_S.word = tmp.result.one[i].word;
                newPuzzleBlockWord_S.color = tmp.result.one[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_S);
            }
             
            for(int i=0;i<tmp.result.two.Count;i++) // 2행
            {
                PuzzleBlockWord_S newPuzzleBlockWord_S = new PuzzleBlockWord_S();
                newPuzzleBlockWord_S.word = tmp.result.two[i].word;
                newPuzzleBlockWord_S.color = tmp.result.two[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_S);
            }

            for(int i=0;i<tmp.result.three.Count;i++) // 3행
            {
                PuzzleBlockWord_S newPuzzleBlockWord_S = new PuzzleBlockWord_S();
                newPuzzleBlockWord_S.word = tmp.result.three[i].word;
                newPuzzleBlockWord_S.color = tmp.result.three[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_S);
            }

            for(int i=0;i<tmp.result.four.Count;i++) // 4행
            {
                PuzzleBlockWord_S newPuzzleBlockWord_S = new PuzzleBlockWord_S();
                newPuzzleBlockWord_S.word = tmp.result.four[i].word;
                newPuzzleBlockWord_S.color = tmp.result.four[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_S);
            }

            for(int i=0;i<tmp.result.five.Count;i++) // 5행
            {
                PuzzleBlockWord_S newPuzzleBlockWord_S = new PuzzleBlockWord_S();
                newPuzzleBlockWord_S.word = tmp.result.five[i].word;
                newPuzzleBlockWord_S.color = tmp.result.five[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_S);
            }

            for(int i=0;i<tmp.result.six.Count;i++) // 6행
            {
                PuzzleBlockWord_S newPuzzleBlockWord_S = new PuzzleBlockWord_S();
                newPuzzleBlockWord_S.word = tmp.result.six[i].word;
                newPuzzleBlockWord_S.color = tmp.result.six[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_S);
            }


            PuzzleBlockWord_S emptyPuzzleBlockWord_S = new PuzzleBlockWord_S();
            emptyPuzzleBlockWord_S.word="";
            emptyPuzzleBlockWord_S.color="";
            wordListToPlace.Insert(13, emptyPuzzleBlockWord_S);
            wordListToPlace.Insert(24, emptyPuzzleBlockWord_S);
        }
    }
}