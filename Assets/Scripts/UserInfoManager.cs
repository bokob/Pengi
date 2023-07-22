using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class UserData
{
    public string code; //
}

public class UserInfoManager : MonoBehaviour
{
    private string userDataFileName = "UserInfo.json"; // json 파일 이름

    private string filePath;
    public UserData userData;

    public GameObject userInfoPannel;

    string inputCode;

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, userDataFileName);
        LoadUserData();
    }

    public void ExitUserInfoPanel() // 유저 정보 패널 닫을 때 실행하는 함수
    {
        inputCode = userInfoPannel.GetComponentInChildren<TextMeshProUGUI>().text;
        Debug.Log(inputCode);
        userData.code = inputCode;
        SaveUserData();
        userInfoPannel.SetActive(false);
    }

    public void LoadUserData() // 유저 정보 불러오기
    {
        if(File.Exists(filePath)) // 유저 정보가 존재하면
        {
            string jsonData = File.ReadAllText(filePath); // 불러온다.
            userData = JsonUtility.FromJson<UserData>(jsonData);

            if(userData.code == "") // 초기값과 똑같다면 -> 아무것도 입력안했단는 뜻
            {
                userInfoPannel.SetActive(true); // 활성화
            }
        }
        else // 파일이 없는 경우, 새로운 유저 정보 생성 후 저장
        {
            userInfoPannel.SetActive(true);
            userData = CreateNewUserData();
            SaveUserData();
        }   
    }

    public UserData CreateNewUserData() // 유저 정보 생성
    {
        UserData newUserData = new UserData();
        newUserData.code = "";

        /*
            나중에 이름, 성별 같은거 추가하면 됨
        */

        return newUserData;
    }

    public void SaveUserData() // 유저 정보 저장
    {   

        string jsonData = JsonUtility.ToJson(userData, true);

        File.WriteAllText(filePath, jsonData);

        Debug.Log("유저 정보 저장 완료");
    }
}
