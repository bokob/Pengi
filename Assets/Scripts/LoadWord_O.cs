using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

[System.Serializable]
public class ReceiveWordData_O
{
    public int status;
    public bool success;
    public string message;
    public Data_O data;
    public Result_O result;
}

[System.Serializable]
public class Data_O
{
    public List<Answer_O> one_word;
    public List<Answer_O> two_word;
    public List<Answer_O> three_word;
    public List<Answer_O> four_word;
}

[System.Serializable]
public class Answer_O
{
    public int w_id;
    public string w_name;
    public string font_color;
}

[System.Serializable]
public class Result_O
{
    public List<PuzzleBlockWord_O> zero;
    public List<PuzzleBlockWord_O> one;
    public List<PuzzleBlockWord_O> two;
    public List<PuzzleBlockWord_O> three;
    public List<PuzzleBlockWord_O> four;
}

[System.Serializable]
public class PuzzleBlockWord_O
{
    public string word;
    public string color;
}

public class LoadWord_O : MonoBehaviour
{

    string url = "http://43.202.24.176:8080//api/wordlist/";
    int level;

    public static LoadWord_O Instance;

    public List<Answer_O> answerList;
    public List<PuzzleBlockWord_O> wordListToPlace;

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

        answerList = new List<Answer_O>();
        wordListToPlace = new List<PuzzleBlockWord_O>();
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
                Answer_O newAnswer = new Answer_O();
                newAnswer.w_id = tmp.data.one_word[i].w_id;
                newAnswer.w_name = tmp.data.one_word[i].w_name;
                newAnswer.font_color = tmp.data.one_word[i].font_color;
                answerList.Add(newAnswer);
            }
            for(int i=0;i<tmp.data.two_word.Count;i++)
            {
                Answer_O newAnswer = new Answer_O();
                newAnswer.w_id = tmp.data.two_word[i].w_id;
                newAnswer.w_name = tmp.data.two_word[i].w_name;
                newAnswer.font_color = tmp.data.two_word[i].font_color;
                answerList.Add(newAnswer); 
            }
            for(int i=0;i<tmp.data.three_word.Count;i++)
            {
                Answer_O newAnswer = new Answer_O();
                newAnswer.w_id = tmp.data.three_word[i].w_id;
                newAnswer.w_name = tmp.data.three_word[i].w_name;
                newAnswer.font_color = tmp.data.three_word[i].font_color;
                answerList.Add(newAnswer);
            }
            for(int i=0;i<tmp.data.four_word.Count;i++)
            {
                Answer_O newAnswer = new Answer_O();
                newAnswer.w_id = tmp.data.four_word[i].w_id;
                newAnswer.w_name = tmp.data.four_word[i].w_name;
                newAnswer.font_color = tmp.data.four_word[i].font_color;
                answerList.Add(newAnswer);
            }
            

            // 맵에 배치할 글자 정보를 모아둔다.
            for(int i=0;i<tmp.result.zero.Count;i++) // 0행
            {
                PuzzleBlockWord_O newPuzzleBlockWord_O = new PuzzleBlockWord_O();
                newPuzzleBlockWord_O.word = tmp.result.zero[i].word;
                newPuzzleBlockWord_O.color = tmp.result.zero[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_O);
            }

            for(int i=0;i<tmp.result.one.Count;i++) // 1행
            {
                PuzzleBlockWord_O newPuzzleBlockWord_O = new PuzzleBlockWord_O();
                newPuzzleBlockWord_O.word = tmp.result.one[i].word;
                newPuzzleBlockWord_O.color = tmp.result.one[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_O);
            }
             
            for(int i=0;i<tmp.result.two.Count;i++) // 2행
            {
                PuzzleBlockWord_O newPuzzleBlockWord_O = new PuzzleBlockWord_O();
                newPuzzleBlockWord_O.word = tmp.result.two[i].word;
                newPuzzleBlockWord_O.color = tmp.result.two[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_O);
            }

            for(int i=0;i<tmp.result.three.Count;i++) // 3행
            {
                PuzzleBlockWord_O newPuzzleBlockWord_O = new PuzzleBlockWord_O();
                newPuzzleBlockWord_O.word = tmp.result.three[i].word;
                newPuzzleBlockWord_O.color = tmp.result.three[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_O);
            }

            for(int i=0;i<3;i++) // 4행
            {
                PuzzleBlockWord_O newPuzzleBlockWord_O = new PuzzleBlockWord_O();
                newPuzzleBlockWord_O.word = tmp.result.four[i].word;
                newPuzzleBlockWord_O.color = tmp.result.four[i].color;
                wordListToPlace.Add(newPuzzleBlockWord_O);
            }


            PuzzleBlockWord_O emptyPuzzleBlockWord_O = new PuzzleBlockWord_O();
            emptyPuzzleBlockWord_O.word="";
            emptyPuzzleBlockWord_O.color="";
            wordListToPlace.Insert(13, emptyPuzzleBlockWord_O);
            wordListToPlace.Insert(17, emptyPuzzleBlockWord_O);
        }
    }
}