using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using TMPro;

[System.Serializable]
public class Accuracy
{
    public int status;
    public bool success;
    public string message;
    public AccuracyData data;
}

[System.Serializable]
public class AccuracyData
{
    public ABPPP accuracy_by_ph_pronunciation_position;
    public ABPPM accuracy_by_ph_pronunciation_method;
    public ABPV accuracy_by_ph_vowel;
}

[System.Serializable]
public class ABPPP
{
    public float velar_accuracy;
    public float alveolar_accuracy;
    public float bilabial_accuracy;
    public float alveopalatal_accuracy;
    public float glottal_accuracy;
}

[System.Serializable]
public class ABPPM
{
    public float stops_accuracy;
    public float nasals_accuracy;
    public float lateral_accuracy;
    public float fricatives_accuracy;
    public float affricates_accuracy;
}

[System.Serializable]
public class ABPV
{
    public float diphthong_accuracy;
    public float front_vowel_accuracy;
    public float back_vowel_accuracy;
    public float high_vowel_accuracy;
    public float mid_vowel_accuracy;
    public float low_vowel_accuracy;
    public float plain_vowel_accuracy;
    public float round_vowel_accuracy;
}

[System.Serializable]
public class Link
{
    public int user_id;
    public string img_url;
}

[System.Serializable]
public class JamoLink
{
    public int user_id;
    public ImgURL img_url;
    public ImgURL2 img_url2;
    public ImgURL3 img_url3;
}

[System.Serializable]
public class ImgURL
{
    public string image_url_ph_pronunciation_position;
}

[System.Serializable]
public class ImgURL2
{
    public string image_url_ph_pronunciation_method;
}

[System.Serializable]
public class ImgURL3
{
    public string image_url_ph_vowel;
}

public class ShowGraphAndPhoneme : MonoBehaviour
{
    public static ShowGraphAndPhoneme Instance;

    public GameObject[] objs; // 보여줄 내용을 저장할 배열
    private int currentIndex = 0; // 현재 보여지는 objs의 인덱스
    public GameObject graph, graph1, graph2, graph3, prev, next;

    // public TextMeshProUGUI[] phoneme;

    public RawImage rawImage, rawImage2, rawImage3, rawImage4;

    private string imageURL = "http://43.202.24.176:8080/api/accuracy/";
    
    string jamoURL = "http://43.202.24.176:8080/api/phoneme/accuracy/image/";
    Accuracy accuracy;


    string userIDFile;

    private string userIDFileName = "UserID.json"; // 유저 ID 저장되어 있는 json 파일 이름
    private string filePath;
    
    int userID;

    Link link;
    JamoLink jamoLink;


    List<float> consonantAccuracyArray, vowelAccuracyArray;
    
    private IEnumerator Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        link = new Link();
        jamoLink = new JamoLink();

        filePath = Path.Combine(Application.persistentDataPath, userIDFileName);
        userIDFile = File.ReadAllText(filePath);
        userID = JsonUtility.FromJson<ReceiveData>(userIDFile).user_id;
        yield return StartCoroutine(GetImageUrlFromServer());
        // yield return StartCoroutine(LoadPhoneme(url, userID));
        Debug.Log(link.img_url);

        /*
            결과 받은거 집어넣는 부분
        */

    }

    IEnumerator GetImageUrlFromServer()
    {
        UnityWebRequest request = UnityWebRequest.Get(imageURL);
        UnityWebRequest request2 = UnityWebRequest.Get(jamoURL);

        // 유저 id 헤더에 담기
        Debug.Log("유저 Id: "+ userID);
        request.SetRequestHeader("USER-ID", userID.ToString());
        request2.SetRequestHeader("USER-ID", userID.ToString());

        Debug.Log("시작 전");
        yield return request.SendWebRequest();
        yield return request2.SendWebRequest();
        Debug.Log("도착");

        if(request.result!=UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            link = JsonUtility.FromJson<Link>(request.downloadHandler.text);

            Debug.Log(link.user_id);
            Debug.Log(link.img_url);
        }

        if(request2.result!=UnityWebRequest.Result.Success)
        {
            Debug.Log(request2.error);
        }
        else
        {
            jamoLink = JsonUtility.FromJson<JamoLink>(request2.downloadHandler.text);

            Debug.Log(jamoLink.user_id);
            Debug.Log(jamoLink.img_url.image_url_ph_pronunciation_position);
            Debug.Log(jamoLink.img_url2.image_url_ph_pronunciation_method);
            Debug.Log(jamoLink.img_url3.image_url_ph_vowel);
        }

        yield return StartCoroutine(GetTexture(rawImage, link.img_url));
        yield return StartCoroutine(GetTexture(rawImage2, jamoLink.img_url.image_url_ph_pronunciation_position));
        yield return StartCoroutine(GetTexture(rawImage3, jamoLink.img_url2.image_url_ph_pronunciation_method));
        yield return StartCoroutine(GetTexture(rawImage4, jamoLink.img_url3.image_url_ph_vowel));
    }

    IEnumerator GetTexture(RawImage rawImage, string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        Debug.Log("사진을 가져올 url : " + url);
        yield return request.SendWebRequest();

        if(request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);

        }
        else
        {
            Debug.Log("넣습니다.");

            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture as Texture2D;
            rawImage.texture = texture;
        }
    }

    IEnumerator LoadPhoneme(string url, int id)
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
            // Debug.Log(request.downloadHandler.text);
            // ReceiveWordData_S tmp = JsonUtility.FromJson<ReceiveWordData_S>(request.downloadHandler.text);
            accuracy = JsonUtility.FromJson<Accuracy>(request.downloadHandler.text);
            // Debug.Log(accuracy.message);

            consonantAccuracyArray = new List<float>(); 
            vowelAccuracyArray = new List<float>();

            consonantAccuracyArray.Add(accuracy.data.accuracy_by_ph_pronunciation_position.velar_accuracy);
            consonantAccuracyArray.Add(accuracy.data.accuracy_by_ph_pronunciation_position.alveolar_accuracy);
            consonantAccuracyArray.Add(accuracy.data.accuracy_by_ph_pronunciation_position.bilabial_accuracy);
            consonantAccuracyArray.Add(accuracy.data.accuracy_by_ph_pronunciation_position.alveopalatal_accuracy);
            consonantAccuracyArray.Add(accuracy.data.accuracy_by_ph_pronunciation_position.glottal_accuracy);
            consonantAccuracyArray.Add(accuracy.data.accuracy_by_ph_pronunciation_method.stops_accuracy);
            consonantAccuracyArray.Add(accuracy.data.accuracy_by_ph_pronunciation_method.nasals_accuracy);
            consonantAccuracyArray.Add(accuracy.data.accuracy_by_ph_pronunciation_method.lateral_accuracy);
            consonantAccuracyArray.Add(accuracy.data.accuracy_by_ph_pronunciation_method.fricatives_accuracy);
            consonantAccuracyArray.Add(accuracy.data.accuracy_by_ph_pronunciation_method.affricates_accuracy);

            vowelAccuracyArray.Add(accuracy.data.accuracy_by_ph_vowel.diphthong_accuracy);
            vowelAccuracyArray.Add(accuracy.data.accuracy_by_ph_vowel.front_vowel_accuracy);
            vowelAccuracyArray.Add(accuracy.data.accuracy_by_ph_vowel.back_vowel_accuracy);
            vowelAccuracyArray.Add(accuracy.data.accuracy_by_ph_vowel.high_vowel_accuracy);
            vowelAccuracyArray.Add(accuracy.data.accuracy_by_ph_vowel.mid_vowel_accuracy);
            vowelAccuracyArray.Add(accuracy.data.accuracy_by_ph_vowel.low_vowel_accuracy);
            vowelAccuracyArray.Add(accuracy.data.accuracy_by_ph_vowel.plain_vowel_accuracy);
            vowelAccuracyArray.Add(accuracy.data.accuracy_by_ph_vowel.round_vowel_accuracy);

            // phoneme[0].text += accuracy.data.accuracy_by_ph_pronunciation_position.velar_accuracy.ToString();
            // phoneme[1].text += accuracy.data.accuracy_by_ph_pronunciation_position.alveolar_accuracy.ToString();
            // phoneme[2].text += accuracy.data.accuracy_by_ph_pronunciation_position.bilabial_accuracy.ToString();
            // phoneme[3].text += accuracy.data.accuracy_by_ph_pronunciation_position.alveopalatal_accuracy.ToString();
            // phoneme[4].text += accuracy.data.accuracy_by_ph_pronunciation_position.glottal_accuracy.ToString();
            // phoneme[5].text += accuracy.data.accuracy_by_ph_pronunciation_method.stops_accuracy.ToString();
            // phoneme[6].text += accuracy.data.accuracy_by_ph_pronunciation_method.nasals_accuracy.ToString();
            // phoneme[7].text += accuracy.data.accuracy_by_ph_pronunciation_method.lateral_accuracy.ToString();
            // phoneme[8].text += accuracy.data.accuracy_by_ph_pronunciation_method.fricatives_accuracy.ToString();
            // phoneme[9].text += accuracy.data.accuracy_by_ph_pronunciation_method.affricates_accuracy.ToString();
            // phoneme[10].text += accuracy.data.accuracy_by_ph_vowel.diphthong_accuracy.ToString();
            // phoneme[11].text += accuracy.data.accuracy_by_ph_vowel.front_vowel_accuracy.ToString();
            // phoneme[12].text += accuracy.data.accuracy_by_ph_vowel.back_vowel_accuracy.ToString();
            // phoneme[13].text += accuracy.data.accuracy_by_ph_vowel.high_vowel_accuracy.ToString();
            // phoneme[14].text += accuracy.data.accuracy_by_ph_vowel.mid_vowel_accuracy.ToString();
            // phoneme[15].text += accuracy.data.accuracy_by_ph_vowel.low_vowel_accuracy.ToString();
            // phoneme[16].text += accuracy.data.accuracy_by_ph_vowel.plain_vowel_accuracy.ToString();
            // phoneme[17].text += accuracy.data.accuracy_by_ph_vowel.round_vowel_accuracy.ToString();
            
            // Debug.Log("ABPPP 시작");
            // Debug.Log("연구개음: " + accuracy.data.accuracy_by_ph_pronunciation_position.velar_accuracy);
            // Debug.Log("치경음: " + accuracy.data.accuracy_by_ph_pronunciation_position.alveolar_accuracy);
            // Debug.Log("양순음: " + accuracy.data.accuracy_by_ph_pronunciation_position.bilabial_accuracy);
            // Debug.Log("경구개치음: " + accuracy.data.accuracy_by_ph_pronunciation_position.alveopalatal_accuracy);
            // Debug.Log("성문음: " + accuracy.data.accuracy_by_ph_pronunciation_position.glottal_accuracy);
            // Debug.Log("ABPPP 끝");

            // Debug.Log("ABPPM 시작");
            // Debug.Log("폐쇄음: " + accuracy.data.accuracy_by_ph_pronunciation_method.stops_accuracy);
            // Debug.Log("비음: " + accuracy.data.accuracy_by_ph_pronunciation_method.nasals_accuracy);
            // Debug.Log("축음: " + accuracy.data.accuracy_by_ph_pronunciation_method.lateral_accuracy);
            // Debug.Log("마찰음: " + accuracy.data.accuracy_by_ph_pronunciation_method.fricatives_accuracy);
            // Debug.Log("폐찰음: " + accuracy.data.accuracy_by_ph_pronunciation_method.affricates_accuracy);
            // Debug.Log("ABPPM 끝");

            // Debug.Log("ABPV 시작");
            // Debug.Log("전체모음: " + accuracy.data.accuracy_by_ph_vowel.diphthong_accuracy);
            // Debug.Log("전설모음: " + accuracy.data.accuracy_by_ph_vowel.front_vowel_accuracy);
            // Debug.Log("후설모음: " + accuracy.data.accuracy_by_ph_vowel.back_vowel_accuracy);
            // Debug.Log("고모음: " + accuracy.data.accuracy_by_ph_vowel.high_vowel_accuracy);
            // Debug.Log("중모음: " + accuracy.data.accuracy_by_ph_vowel.mid_vowel_accuracy);
            // Debug.Log("저모음: " + accuracy.data.accuracy_by_ph_vowel.low_vowel_accuracy);
            // Debug.Log("평순모음: " + accuracy.data.accuracy_by_ph_vowel.plain_vowel_accuracy);
            // Debug.Log("원순모음: " + accuracy.data.accuracy_by_ph_vowel.round_vowel_accuracy);
            // Debug.Log("ABPV 시작");
        }
    }



    public void ChangeObj()
    {
        if (objs.Length == 0)
            return;

        // targetImage.sprite = images[currentIndex];
        UpdateButtonsState();
    }

    // 뒤로 가기 버튼을 누를 때 호출되는 함수
    public void PreviousObj()
    {
        currentIndex = (currentIndex - 1 + objs.Length) % objs.Length;
        Debug.Log("현재 인덱스: " + currentIndex);
        ChangeObj();
    }

    // 앞으로 가기 버튼을 누를 때 호출되는 함수
    public void NextObj()
    {
        currentIndex = (currentIndex + 1) % objs.Length;
        Debug.Log("현재 인덱스: " + currentIndex);
        ChangeObj();
    }

    // 버튼 상태 업데이트 함수
    private void UpdateButtonsState()
    {
        graph.SetActive(currentIndex == 0);
        graph1.SetActive(currentIndex == 1); 
        graph2.SetActive(currentIndex == 2);
        graph3.SetActive(currentIndex == 3);
        prev.SetActive(currentIndex != 0);
        next.SetActive(currentIndex != 3);
    }
}
