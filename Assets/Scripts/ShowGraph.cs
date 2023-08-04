using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;


// [System.Serializable]
// public class Link
// {
//     public int user_id;
//     public string img_url;
// }

public class ShowGraph : MonoBehaviour
{
    public RawImage rawImage;

    private string imageURL = "http://43.202.24.176:8080/api/accuracy/";

    string userIDFile;

    private string userIDFileName = "UserID.json"; // 유저 ID 저장되어 있는 json 파일 이름
    private string filePath;
    
    int userID;

    Link link;
    void Start()
    {
        link = new Link();

        filePath = Path.Combine(Application.persistentDataPath, userIDFileName);
        userIDFile = File.ReadAllText(filePath);
        userID = JsonUtility.FromJson<ReceiveData>(userIDFile).user_id;
        StartCoroutine(GetImageUrlFromServer());
        Debug.Log(link.img_url);
    }

    IEnumerator GetImageUrlFromServer()
    {
        UnityWebRequest request = UnityWebRequest.Get(imageURL);

        // 유저 id 헤더에 담기
        Debug.Log("유저 Id: "+ userID);
        request.SetRequestHeader("USER-ID", userID.ToString());

        Debug.Log("시작 전");
        yield return request.SendWebRequest();
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

        yield return StartCoroutine(GetTexture(link.img_url));
    }

    IEnumerator GetTexture(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        Debug.Log("사진을 가져올 url : " + url);

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
}