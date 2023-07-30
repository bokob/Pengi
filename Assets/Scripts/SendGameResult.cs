using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class GameResult // 게임 결과
{
    public int user_id; // 유저 id
    public int chance; // 찬스 사용 횟수
    public string completed; // "1": 성공, "0": 실패

    public List<CorrectWord> correct_words; // 맞은 단어 배열
    public List<WrongWord> wrong_words; // 틀린 단어 배열

}

[System.Serializable]
public class CorrectWord
{
    public int word_id; // 단어 id
    public string word_name; // 단어
}

[System.Serializable]
public class WrongWord
{
    public int word_id; // 단어 id
    public string word_name; // 단어
    public string spell_name; // 틀리게 말한 내용
}

public class SendGameResult : MonoBehaviour
{
    public static SendGameResult Instance;
    GameResult gameResult;
    CorrectWord correctWord;
    WrongWord wrongWord;

    void Start()
    {
        gameResult = new GameResult();
        gameResult.correct_words = new List<CorrectWord>();
        gameResult.wrong_words = new List<WrongWord>();
        correctWord = new CorrectWord();
        wrongWord = new WrongWord();
    }

    public void AddCoreectWord(CorrectWord item)
    {
        gameResult.correct_words.Add(item);
    }

    public void AddWrongWord(WrongWord item)
    {
        gameResult.wrong_words.Add(item);
    }

    public void Send()
    {
        /*
            여기 써야할 코드는 
            틀린 단어와 맞았던 단어 담은 것을 
            보내야 됨
        */

        // gameResult.user_id=10;
        // gameResult.chance=1;
        // gameResult.completed="1";

        // correctWord.word_id=1;
        // correctWord.word_name="ㅋㅋ";

        // wrongWord.word_id = 2;
        // wrongWord.word_name="ㅋㅋㅋ";
        // wrongWord.spell_name="ㅎㅎㅎ";

        // gameResult.correct_words.Add(correctWord);
        // gameResult.wrong_words.Add(wrongWord);
        // string jsonData = JsonUtility.ToJson(gameResult,true);

    //     Debug.Log("게임결과 보내기 시작");
    //     Debug.Log(jsonData);
    //     Debug.Log("이러한 내용을 보낼겁니다.");

    //    StartCoroutine(Upload("http://43.202.24.176:8080/api/game", jsonData));
    }

    IEnumerator Upload(string url, string json)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(url, json))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json); // string으로 넘기면 json 구성이 깨지기 때문에 byte로 변환 후 파일로 업로드한다.
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json"); // HTTP 헤더 설정 "Content-Type"으로 설정해 JSON 데이터임을 서버에 알린다.

            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {   // 연결이 안되거나 프로토콜 오류인 경우
                Debug.Log(request.error);
            }
            else // 잘 온 경우
            {
                Debug.Log(request.downloadHandler.text);
                DataContainer dataContainer = JsonUtility.FromJson<DataContainer>(request.downloadHandler.text);
                Debug.Log(dataContainer.status);
            }
            request.Dispose(); // 메모리 누수 막기 위해
        }
    }
}
