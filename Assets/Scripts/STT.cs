using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// UnityWebRequest 사용하기 위한 네임스페이스
using UnityEngine.Networking;

// 클로바 api 쓰기 위한 네임스페이스
using System;
using System.Net;
using System.Text;
using System.IO;
using System.Collections.Specialized;

public class STT : MonoBehaviour
{
    private string _microphoneID = null;
    private AudioClip _recording = null;
    private int _recordingLengthSec = 15;
    private int _recordingHZ = 22050;

    // Force save as 16-bit .wav
	const int BlockSize_16Bit = 2;

    private TextMeshProUGUI textObj;

    private void Start() // 핸드폰 마이크 정보를 가져온다.
    {
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
            textObj=GameObject.Find("Canvas").GetComponentInChildren<TextMeshProUGUI>();
            
            // json 형태로 받음 {"text":"인식결과"}
            string message = request.downloadHandler.text;
            VoiceRecognize voiceRecognize = JsonUtility.FromJson<VoiceRecognize>(message);

            if(voiceRecognize.text==null)
                Debug.Log("아무것도 없는게 도착");
            else
                Debug.Log("무언가 들어 있음");

            Debug.Log("음성 인식 결과: " + voiceRecognize.text);
            textObj.text = voiceRecognize.text;
            //textObj.text = voiceRecognize.text;
            // Voice Server responded: 인식결과
        }
        request.Dispose(); // 메모리 누수 막기 위해
    }
}