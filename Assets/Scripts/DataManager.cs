using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // 저장 등 파일 관리를 위해

public class GameData // json으로 저장할 클래스
{
    public bool first;
    public int level; // 0: 숲, 1: 사막, 2: 바다, 3: 초원, 4: 우주, 각 레벨의 10번째 스테이지를 클리어하면 1 증가, 클리어해야 할 레벨
    public int stage; // 0~9 -> 1~10 스테이지, 클리어해야 할 스테이지
    public bool isBGM; // bgm 상태
    public bool isEffectSound; // 효과 상태
    
    public bool[] isClear; // 길이가 50, 5레벨 10 스테이지의 클리어 여부 저장
}

public class DataManager  : MonoBehaviour
{
    // 불러온 데이터 계속 들고 있게 하기
    static GameObject container;
    static DataManager  instance;
    public static DataManager  Instance // 싱글톤으로 만든 후에 게임에서 계속 존재하게 하기
    {
        get
        {
            if (!instance)
            {
                container = new GameObject();
                container.name = "DataManager ";
                instance = container.AddComponent(typeof(DataManager )) as DataManager ;
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }

    private string gameDataFileName = "GameData.json"; // .json 파일 이름
    
    // 저장할 파일 경로 담을 변수
    private string filePath; 

    public GameData gameData;

    private void Awake() // 게임 시작시 불러오기
    {
        filePath = Path.Combine(Application.persistentDataPath, gameDataFileName);
        LoadGameData();
        // Debug.Log("불러왔음요");
        // Debug.Log("level: " + gameData.level);
        // Debug.Log(filePath);
        // Debug.Log(gameData.isclear[1,0]);
        // UpdateGameData();
    }

     public void LoadGameData() // 게임 불러오기
    {
        if (File.Exists(filePath)) // json이 존재하면
        {
            string jsonData = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(jsonData);
        }
        else // 저장된 파일이 없는 경우, 새로운 게임 정보 생성 후 저장
        {
            gameData = CreateNewGameData();
            SaveGameData();
        }
    }

    public void SaveGameData() // 게임 정보 저장
    {
        string jsonData = JsonUtility.ToJson(gameData, true); // json으로 변환
        
        // 저장된 파일 있으면 덮어쓰고, 없으면 새로 만들어서 저장
        File.WriteAllText(filePath, jsonData);

        Debug.Log("저장 완료");
    }

    public GameData CreateNewGameData() // 새 게임 정보 생성
    {
        // 새로운 게임 정보 생성 로직 예시
        GameData newGameData = new GameData();
        newGameData.first = true;
        newGameData.level = 0;
        newGameData.stage = 0;
        newGameData.isBGM = true; // bgm 켜기
        newGameData.isEffectSound = true; // 효과 켜기

        newGameData.isClear = new bool[50];
        for(int i=0;i<50;i++)
            newGameData.isClear[i]=false;

        // 추가로 업데이트 할 필드 집어넣으면 됨

        return newGameData;
    }

    public void UpdateGameSetData(int action)
    {
        // 게임 정보 갱신 로직 예시
        //gameData.level++;
        // 기타 필드 갱신 로직도 여기에 작성
        switch(action)
        {
            case 0: // 최초 플레이 시에만 오프닝
                gameData.first=false;
                break;
            case 1: // bgm 여부
                gameData.isBGM = !gameData.isBGM;
                break;
            case 2: // 효과음 여부
                gameData.isEffectSound = !gameData.isEffectSound;
                break;
            default:
                break;
        }
        SaveGameData();
    }

    // public void UpdateRecentGameLevel()
    // {
    //     // 게임 정보 갱신 로직 예시
    //     //gameData.level++;
    //     // 기타 필드 갱신 로직도 여기에 작성
    // }

    public void UpdateGameClear(int level, int stage) // 클리어 여부 업데이트
    {
        
    }



    // private void OnApplicationQuit() // 게임 종료 시
    // {
    //     SaveGameData();
    // }
}
