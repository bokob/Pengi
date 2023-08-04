/* 
녹음하고 네이버 클로바 API로 보내는 스크립트
+
정답 및 오답 처리 후 서버로 보내는 부분도 포함
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// UnityWebRequest 사용하기 위한 네임스페이스
using UnityEngine.Networking;

// 클로바 api 쓰기 위한 네임스페이스
using System;
using System.Net;
using System.Text;
using System.IO;
using System.Collections.Specialized;

[System.Serializable]
public class GameResult // 게임 결과
{
    public int user_id; // 유저 id
    public int chance=0; // 찬스 사용 횟수
    public string completed="0"; // "1": 성공, "0": 실패

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

public class STT : MonoBehaviour
{
    public static STT Instance;
    public GameResult gameResult;
    CorrectWord correctWord;
    WrongWord wrongWord;

    string userID;

    private string userIDFileName = "UserID.json"; // 유저 ID 저장되어 있는 json 파일 이름

    private string filePath;

    public TextMeshProUGUI recognitionWord, selectedWord;
    //bool flag = false;
    GameObject[] buttons;

    string sceneName;

    private string _microphoneID = null;
    private AudioClip _recording = null;
    private int _recordingLengthSec = 15;
    private int _recordingHZ = 22050;

    // Force save as 16-bit .wav
	const int BlockSize_16Bit = 2;

    // private TextMeshProUGUI textObj;

    /*
    핸드폰 마이크 정보, 블록 가져온다.
    정답, 오답 담을 클래스 준비
    */

    private void Start()
    {

        if(Instance == null)
        {
            Instance = this;
        }

        filePath = Path.Combine(Application.persistentDataPath, userIDFileName);

        userID = File.ReadAllText(filePath);

        gameResult = new GameResult();
        gameResult.user_id= JsonUtility.FromJson<ReceiveData>(userID).user_id;
        gameResult.correct_words = new List<CorrectWord>();
        gameResult.wrong_words = new List<WrongWord>();


        buttons = GameObject.FindGameObjectsWithTag("Block");

        sceneName = SceneManager.GetActiveScene().name;



        _microphoneID = Microphone.devices[0]; // 핸드폰 마이크 하나니까

        if(_microphoneID == null) // 마이크 못찾았을 때
        {
            Debug.Log("마이크 없음!");
        }
        else
        {
            Debug.Log("마이크 찾음!");
        }
    }

    private float clickTime; // 클릭 중인 시간
    public float minClickTime = 1; // 최소 클릭시간
    private bool isClick; // 클릭 중인지 판단
    public void ButtonDown()
    {
        isClick=true;
        startRecording();
    }

    public void ButtonUp()
    {
        isClick=false;
        // textObj=GameObject.Find("Canvas").GetComponentInChildren<TextMeshProUGUI>();
        // Debug.Log(textObj.text);
        if(clickTime>=minClickTime)
        {
            stopRecording();
        }
    }

    private void Update()
    {
        if(isClick) // 클릭하고 있다면
        {
            clickTime += Time.deltaTime;
        }
        else // 클릭중 아니면
        {
            clickTime = 0;
        }
    }

    public void startRecording() // 녹음 시작
    {
        Debug.Log("녹음 시작");
        _recording = Microphone.Start(_microphoneID, false, _recordingLengthSec, _recordingHZ);
    }

    public void stopRecording() // 녹음 중지
    {
        if(Microphone.IsRecording(_microphoneID))
        {
            Microphone.End(_microphoneID);

            Debug.Log("녹음 중지");
            if(_recording == null)
            {
                Debug.LogError("아무것도 녹음안됨...");
                return;
            }

            // 오디오 클립 바이트배열로 변환
            byte[] byteData = getByteFromAudioClip(_recording);

            // 녹음된 오디오 클립 api 서버로 보냄
            StartCoroutine(PostVoice(url, byteData));
        }
        return;
    }


    // 음성 파일을 보관할 필요가 없기에 바로 바이트 배열로 변환한다.
    private byte[] getByteFromAudioClip(AudioClip audioClip)
    {
        MemoryStream stream = new MemoryStream();
        const int headerSize = 44;
        ushort bitDepth =16;

        int fileSize = audioClip.samples * BlockSize_16Bit + headerSize;

        // 오디오 클립의 정보들을 file stream에 추가
        WriteFileHeader(ref stream, fileSize);
        WriteFileFormat(ref stream, audioClip.channels, audioClip.frequency, bitDepth);
        WriteFileData(ref stream, audioClip, bitDepth);

        // stream을 array 형태로 바꾼다.
        byte[] bytes = stream.ToArray();

        return bytes;
    }

    // WAV 파일 헤더 생성
    private static int WriteFileHeader (ref MemoryStream stream, int fileSize)
	{
		int count = 0;
		int total = 12;

		// riff chunk id
		byte[] riff = Encoding.ASCII.GetBytes ("RIFF");
		count += WriteBytesToMemoryStream (ref stream, riff, "ID");

		// riff chunk size
		int chunkSize = fileSize - 8; // total size - 8 for the other two fields in the header
		count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (chunkSize), "CHUNK_SIZE");

		byte[] wave = Encoding.ASCII.GetBytes ("WAVE");
		count += WriteBytesToMemoryStream (ref stream, wave, "FORMAT");

		// Validate header
		Debug.AssertFormat (count == total, "Unexpected wav descriptor byte count: {0} == {1}", count, total);

		return count;
	}

	private static int WriteFileFormat (ref MemoryStream stream, int channels, int sampleRate, UInt16 bitDepth)
	{
		int count = 0;
		int total = 24;

		byte[] id = Encoding.ASCII.GetBytes ("fmt ");
		count += WriteBytesToMemoryStream (ref stream, id, "FMT_ID");

		int subchunk1Size = 16; // 24 - 8
		count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (subchunk1Size), "SUBCHUNK_SIZE");

		UInt16 audioFormat = 1;
		count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (audioFormat), "AUDIO_FORMAT");

		UInt16 numChannels = Convert.ToUInt16 (channels);
		count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (numChannels), "CHANNELS");

		count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (sampleRate), "SAMPLE_RATE");

		int byteRate = sampleRate * channels * BytesPerSample (bitDepth);
		count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (byteRate), "BYTE_RATE");

		UInt16 blockAlign = Convert.ToUInt16 (channels * BytesPerSample (bitDepth));
		count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (blockAlign), "BLOCK_ALIGN");

		count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (bitDepth), "BITS_PER_SAMPLE");

		// Validate format
		Debug.AssertFormat (count == total, "Unexpected wav fmt byte count: {0} == {1}", count, total);

		return count;
	}

	private static int WriteFileData (ref MemoryStream stream, AudioClip audioClip, UInt16 bitDepth)
	{
		int count = 0;
		int total = 8;

		// Copy float[] data from AudioClip
		float[] data = new float[audioClip.samples * audioClip.channels];
		audioClip.GetData (data, 0);

		byte[] bytes = ConvertAudioClipDataToInt16ByteArray (data);

		byte[] id = Encoding.ASCII.GetBytes ("data");
		count += WriteBytesToMemoryStream (ref stream, id, "DATA_ID");

		int subchunk2Size = Convert.ToInt32 (audioClip.samples * BlockSize_16Bit); // BlockSize (bitDepth)
		count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (subchunk2Size), "SAMPLES");

		// Validate header
		Debug.AssertFormat (count == total, "Unexpected wav data id byte count: {0} == {1}", count, total);

		// Write bytes to stream
		count += WriteBytesToMemoryStream (ref stream, bytes, "DATA");

		// Validate audio data
		Debug.AssertFormat (bytes.Length == subchunk2Size, "Unexpected AudioClip to wav subchunk2 size: {0} == {1}", bytes.Length, subchunk2Size);

		return count;
	}

    private static int BytesPerSample (UInt16 bitDepth)
	{
		return bitDepth / 8;
	}

    // 바이트 배열을 MemoryStream에 작성하는 함수
    private static int WriteBytesToMemoryStream (ref MemoryStream stream, byte[] bytes, string tag = "")
	{
		int count = bytes.Length;
		stream.Write (bytes, 0, count);
		//Debug.LogFormat ("WAV:{0} wrote {1} bytes.", tag, count);
		return count;
	}

    // float 배열을 Int16 형식의 바이트 배열로 변환하는 함수
    private static byte[] ConvertAudioClipDataToInt16ByteArray (float[] data)
	{
		MemoryStream dataStream = new MemoryStream ();

		int x = sizeof(Int16);

		Int16 maxValue = Int16.MaxValue;

		int i = 0;
		while (i < data.Length) {
			dataStream.Write (BitConverter.GetBytes (Convert.ToInt16 (data [i] * maxValue)), 0, x);
			++i;
		}
		byte[] bytes = dataStream.ToArray ();

		// Validate converted bytes
		Debug.AssertFormat (data.Length * x == bytes.Length, "Unexpected float[] to Int16 to byte[] size: {0} == {1}", data.Length * x, bytes.Length);

		dataStream.Dispose ();

		return bytes;
	}


    // 받아온 값에 간편하게 접근하기 위한 JSON 선언
    [Serializable]
    public class VoiceRecognize
    {
        public string text;
    }

    // 사용할 언어(Kor)를 맨 뒤에 붙임
    string url = "https://naveropenapi.apigw.ntruss.com/recog/v1/stt?lang=Kor";
    private IEnumerator PostVoice(string url, byte[] data)
    {
        // request 생성
        WWWForm form = new WWWForm();
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        
        // 요청 헤더 설정 YOUR_CLIENT_ID, CLIENT_SECRET 확인한 인증키 넣는다.
        request.SetRequestHeader("X-NCP-APIGW-API-KEY-ID", "ackm9h2jgo"); // YOUR_CLIENT_ID
        request.SetRequestHeader("X-NCP-APIGW-API-KEY", "z734vX91V140oD2evdC22W03xsYIWDkE0haxDKcv"); // CLIENT_SECRET
        request.SetRequestHeader("Content-Type", "application/octet-stream");
        
        // 바디에 처리과정을 거친 Audio Clip data를 실어줌
        request.uploadHandler = new UploadHandlerRaw(data);
        
        // 요청을 보낸 후 response를 받을 때까지 대기
        yield return request.SendWebRequest();
        
        // 만약 response가 비어있다면 error
        if (request == null)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log("도착");
            string message = request.downloadHandler.text;
            VoiceRecognize voiceRecognize = JsonUtility.FromJson<VoiceRecognize>(message);

            if(voiceRecognize.text==null)
                Debug.Log("아무것도 없는게 도착");
            else
                Debug.Log("무언가 들어 있음");

            GameObject panelObject = GameObject.Find("RecognitionWord");

            if(panelObject!=null)
            {
                // textObj = panelObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                voiceRecognize.text = voiceRecognize.text.Replace(" ", ""); // 띄어쓰기 없애기
                Debug.Log("음성 인식 결과: " + voiceRecognize.text);
                recognitionWord.text = voiceRecognize.text;
            }
            //textObj.text = voiceRecognize.text;
            // Voice Server responded: 인식결과
        }
        request.Dispose(); // 메모리 누수 막기 위해

        
        // 정답 오답 로직 부분
        if(recognitionWord.text!="") // 빈문자열이 아니여야 비교 로직 작동
        {
            if(recognitionWord.text == selectedWord.text) // 정답
            {
                CorrectWord newCorrectWord = new CorrectWord();
                newCorrectWord.word_id = GetWordId(selectedWord.text); // id 넣기

                if(newCorrectWord.word_id==-1)
                {
                    yield break;
                }

                newCorrectWord.word_name = selectedWord.text;
                Debug.Log(newCorrectWord.word_name + " 이거를 넣을겁니다.");
                AddCoreectWord(newCorrectWord);
                Debug.Log(gameResult.correct_words[gameResult.correct_words.Count-1].word_id + " 번호가 있네요.");
                Debug.Log(gameResult.correct_words[gameResult.correct_words.Count-1].word_name + " 가 들어있네요");
                // Debug.Log("현재 사용한 찬스 : " + gameResult.chance);
                Debug.Log("맞은 단어의 개수: " + gameResult.correct_words.Count);
                Debug.Log("틀린 단어의 개수: " + gameResult.wrong_words.Count);

                RemoveBlock(); // 해당 단어 들어있는 블록 비활성화
                selectedWord.text = "성공"; // 이거 안해주면 중간에 브금 끊김
                Invoke("ClearRecognitionAndSelectedText", 2f); // 2초뒤에 인식, 선택 글씨 비우기
            }
            else // 오답
            {
                WrongWord newWrongWord = new WrongWord();
                newWrongWord.word_id=GetWordId(selectedWord.text);

                if(newWrongWord.word_id==-1)
                {
                    yield break;
                }
                newWrongWord.word_name=selectedWord.text; // 선택한 단어(정답)
                newWrongWord.spell_name=recognitionWord.text; // 틀리게 발음한 것
                // SendGameResult.Instance.AddWrongWord(newWrongWord);
                AddWrongWord(newWrongWord);
                Debug.Log(gameResult.wrong_words[gameResult.wrong_words.Count-1].word_name + " 가 들어있네요");
                // Debug.Log("현재 사용한 찬스 : " + gameResult.chance);
                Debug.Log("맞은 단어의 개수: " + gameResult.correct_words.Count);
                Debug.Log("틀린 단어의 개수: " + gameResult.wrong_words.Count);

                Invoke("ClearRecognitionText", 2f);
            }
        }
    }

    void RemoveBlock() // 블록 비활성화
    {
        GameObject[] buttonArray = BlockList.Instance.buttonList.ToArray(); // 리스트에 넣어놓은 버튼들을 인덱스로 접근하기 위해 배열로 바꾼다.

        Debug.Log("큐에 들어있는 버튼들 비활성화 시작!");
        for(int i=0;i<buttonArray.Length;i++) // 전부 비활성화
        {
            Debug.Log(buttonArray[i].name + " 비활성화");
            buttonArray[i].SetActive(false);
        }
        Debug.Log("큐 비활성화 종료!");
        BlockList.Instance.PopAllButton(); // 리스트 비우기

        SoundManager.Instance.PlaySFX("BreakBlock"); // '풍덩' 효과음
    }

    void ClearRecognitionAndSelectedText() // 모든 글자 비우기
    {
        recognitionWord.text = "";
        selectedWord.text = "";
    }

    void ClearRecognitionText()
    {
        recognitionWord.text="";
    }

    public int GetWordId(string word)
    {
        int id=-1;

        if(sceneName == "ForestPlayScene")
        {
            for(int i=0;i<LoadWord_F.Instance.answerList.Count;i++)
            {
                if(LoadWord_F.Instance.answerList[i].w_name == word)
                    id = LoadWord_F.Instance.answerList[i].w_id;
            }
        }
        else if(sceneName == "DesertPlayScene")
        {
            for(int i=0;i<LoadWord_D.Instance.answerList.Count;i++)
            {
                if(LoadWord_D.Instance.answerList[i].w_name == word)
                    id = LoadWord_D.Instance.answerList[i].w_id;
            }
        }
        else if(sceneName == "OceanPlayScene")
        {
            for(int i=0;i<LoadWord_O.Instance.answerList.Count;i++)
            {
                if(LoadWord_O.Instance.answerList[i].w_name == word)
                    id = LoadWord_O.Instance.answerList[i].w_id;
            }
        }
        else if(sceneName == "PasturePlayScene")
        {
            for(int i=0;i<LoadWord_P.Instance.answerList.Count;i++)
            {
                if(LoadWord_P.Instance.answerList[i].w_name == word)
                    id = LoadWord_P.Instance.answerList[i].w_id;
            }
        }
        else if(sceneName == "SpacePlayScene")
        {
            for(int i=0;i<LoadWord_S.Instance.answerList.Count;i++)
            {
                if(LoadWord_S.Instance.answerList[i].w_name == word)
                    id = LoadWord_S.Instance.answerList[i].w_id;
            }
        }
        else if(sceneName == "WrongPlayScene")
        {
            for(int i=0;i<LoadWord_W.Instance.answerList.Count;i++)
            {
                if(LoadWord_W.Instance.answerList[i].w_name == word)
                    id = LoadWord_W.Instance.answerList[i].w_id;
            }
        }

        return id;
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

        //gameResult.chance=1;
        //gameResult.completed="1";

        string jsonData = JsonUtility.ToJson(gameResult,true);

        Debug.Log("게임결과 보내기 시작");
        Debug.Log(jsonData);
        Debug.Log("이러한 내용을 보낼겁니다.");

       StartCoroutine(Upload("http://43.202.24.176:8080/api/game", jsonData));
    }

    IEnumerator Upload(string url, string json) // Post하기 위해 필요한 함수
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