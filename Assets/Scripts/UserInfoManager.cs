using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.Networking;
using System.Text.RegularExpressions; // 정규식

[System.Serializable]
public class UserData // 서버로 보내줄 유저 정보
{
    public string user_name; // 이름
    public string user_birth; // 생년월일
    public string user_disability; // 장애등급
    public string user_gender; // 성별
    public string user_code; // 개인코드
}

[System.Serializable]
public class ModifiedUserData // 수정된 정보
{
    public int user_id; // 유저 id
    public string user_name; // 이름
    public string user_birth; // 생년월일
    public string user_disability; // 장애등급
    public string user_gender; // 성별
    public string user_code; // 개인코드

}

[System.Serializable]
public class DataContainer // 서버로부터 처음 id 발급 받을 때 쓸 자료구조
{
    public int status;
    public bool success;
    public string message;
    public ReceiveData data;
}

[System.Serializable] 
public class ReceiveData // data가 감싸져서 오기 때문에 접근하기 위한 자료구조
{
    public int user_id;
}

public class UserInfoManager : MonoBehaviour
{
    private string userIDFileName = "UserID.json"; // 유저 ID 저장되어 있는 json 파일 이름

    private string filePath;
    public UserData userData;
    public ModifiedUserData modifiedUserData;

    public GameObject userInfoPannel;

    TextMeshProUGUI[] inputFileds;

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, userIDFileName);
        LoadUserID();
    }

    public void LoadUserID() // 유저 정보 불러오기
    {
        if(File.Exists(filePath)) // 유저 id가 존재하면
        {
            string jsonData = File.ReadAllText(filePath); // 불러온다.
            userData = JsonUtility.FromJson<UserData>(jsonData);
            modifiedUserData = CreateNewModifiedUserData();

            // if(userData.user_code == "") // 초기값과 똑같다면 -> 아무것도 입력안했단는 뜻
            // {
            //     userInfoPannel.SetActive(true); // 활성화
            // }
        }
        else // 유저 id가 없는 경우, 서버로 보낼 유저 정보를 만든다.
        {
            userInfoPannel.SetActive(true);
            userData = CreateNewUserData();
            //SaveUserData();
        }   
    }

    public UserData CreateNewUserData() // 유저 정보 생성
    {
        UserData newUserData = new UserData();
        newUserData.user_name="";
        newUserData.user_birth="";
        newUserData.user_disability="";
        newUserData.user_gender="";
        newUserData.user_code = "";
        /*
            나중에 필드 더 추가할 것 있으면 추가
        */
        return newUserData;
    }

    public ModifiedUserData CreateNewModifiedUserData() // 수정된 유저 정보 생성
    {
        ModifiedUserData newModifiedUserData = new ModifiedUserData();
        newModifiedUserData.user_name="";
        newModifiedUserData.user_birth="";
        newModifiedUserData.user_disability="";
        newModifiedUserData.user_gender="";
        newModifiedUserData.user_code="";
        /*
            나중에 필드 더 추가할 것 있으면 추가
        */
        return newModifiedUserData;
    }

    public void SendUserInfo() // 유저 정보 보내기
    {
        inputFileds = userInfoPannel.GetComponentsInChildren<TextMeshProUGUI>();
        for(int i=0;i<inputFileds.Length;i++) // placeholder, 실제값, 라벨 순으로 찾아온것을 확인함
        {
            Debug.Log(i + "번째 " + inputFileds[i].text);
        }        
        //SaveUserData();


        if(File.Exists(filePath)) // 유저 id가 존재하는 경우이므로 id를 발급 받은 상태, 즉 수정하려는 것이다.
        {
            string userIDjson = File.ReadAllText(filePath);
            modifiedUserData.user_id = JsonUtility.FromJson<ReceiveData>(userIDjson).user_id;
            modifiedUserData.user_name = inputFileds[1].text;
            modifiedUserData.user_birth = inputFileds[4].text;
            modifiedUserData.user_disability = inputFileds[7].text;
            modifiedUserData.user_gender = inputFileds[10].text;
            modifiedUserData.user_code = inputFileds[13].text;


            Debug.Log("수정된 데이터의 이름은 + " + modifiedUserData.user_name + " 입니다.");

            string jsonData = JsonUtility.ToJson(modifiedUserData, true);

            Debug.Log("유저 정보를 수정합니다.");
            
            Debug.Log(jsonData);

            StartCoroutine(ReUpload("http://43.202.24.176:8080/api/user/mypage", jsonData));


        }
        else
        {
            // 유저 정보를 클래스에 담는다.
            userData.user_name = inputFileds[1].text;
            userData.user_birth = inputFileds[4].text;
            userData.user_disability = inputFileds[7].text;
            userData.user_gender = inputFileds[10].text;
            userData.user_code = inputFileds[13].text;

            // json으로 변환
            string jsonData = JsonUtility.ToJson(userData, true);

            /*
            jsonData 보내면 응답으로 시퀀스 넘버(id)를 받고 그 id는 저장
            만약 id를 저장한 파일이 존재하면 jsonData에 id와 같이 보내야 됨
            */
            StartCoroutine(Upload("http://43.202.24.176:8080/api/user/signin", jsonData));
        }
    }

    IEnumerator ReUpload(string url, string json)
    {
        using (UnityWebRequest request = UnityWebRequest.Put(url, json))
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
                SaveUserID(dataContainer.data);
                Debug.Log(dataContainer.data.user_id);
            }
            request.Dispose(); // 메모리 누수 막기 위해
        }
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
                SaveUserID(dataContainer.data);
                Debug.Log(dataContainer.data.user_id);
            }
            request.Dispose(); // 메모리 누수 막기 위해
        }
    }

    public void SaveUserID(ReceiveData data) // 서버로부터 발급받은 유저 id 저장
    {   
        string jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, jsonData);
        Debug.Log("유저 ID 저장 완료");
    }

    public void ExitUserInfoPanel() // 유저 정보 패널 닫을 때 실행하는 함수 (창 비활성화)
    {
        userInfoPannel.SetActive(false);
    }
}
