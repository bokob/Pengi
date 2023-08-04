using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // 저장 등 파일 관리를 위해

public class GameData // json으로 저장할 클래스
{
    public bool last; // 엔딩 보여줄건지 체크 true: 보여줌, false: 안보여줌
    public int recentLevel; // 1: 숲, 2: 사막, 3: 바다, 4: 초원, 5: 우주, 각 레벨의 10번째 스테이지를 클리어하면 1 증가, 클리어해야 할 레벨
    public int recentStage; // 0~9 -> 1~10 스테이지, 클리어해야 할 스테이지
    public int forestStage; // 숲 스테이지
    public int desertStage; // 사막 스테이지
    public int oceanStage; // 해변 스테이지
    public int pastureStage; // 초원 스테이지
    public int spaceStage; // 우주 스테이지
    public float bgmVolume; // bgm 상태
    public float sfxVolume; // 효과 상태
    
    public bool[] forestClear; // 숲 클리어
    public bool[] desertClear; // 사막 클리어
    public bool[] oceanClear; // 해변 클리어
    public bool[] pastureClear; // 초원 클리어
    public bool[] spaceClear; // 우주 클리어
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
                container.name = "DataManager";
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
            SoundManager.Instance.musicSource.volume = gameData.bgmVolume;
            SoundManager.Instance.sfxSource.volume = gameData.sfxVolume;
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
        newGameData.last = true;
        newGameData.recentLevel = 1;
        newGameData.recentStage = 1;

        newGameData.forestStage = 1;
        newGameData.desertStage = 1;
        newGameData.oceanStage = 1;
        newGameData.pastureStage = 1;
        newGameData.spaceStage = 1;

        newGameData.bgmVolume = 1f; // bgm 크기 설정
        newGameData.sfxVolume = 1f; // 효과음 설정

        newGameData.forestClear = new bool[10];
        newGameData.desertClear = new bool[10];
        newGameData.oceanClear = new bool[10];
        newGameData.pastureClear = new bool[10];
        newGameData.spaceClear = new bool[10];
        for(int i=0;i<10;i++) // 스테이지 클리어 여부 초기화
        {
            newGameData.forestClear[i]=false;
            newGameData.desertClear[i]=false;
            newGameData.oceanClear[i]=false;
            newGameData.pastureClear[i]=false;
            newGameData.spaceClear[i]=false;
        }
        // 추가로 업데이트 할 필드 집어넣으면 됨
        return newGameData;
    }

    public void UpdateGameSetData(int action)
    {
        // 게임 정보 갱신 로직 예시
        // gameData.level++;
        // 기타 필드 갱신 로직도 여기에 작성
        switch(action)
        {
            case 0: 
                break;
            case 1: // bgm 소리 크기
                gameData.bgmVolume = SoundManager.Instance.musicSource.volume;
                Debug.Log("브금 크기가 " + gameData.bgmVolume + " 로 저장됩니다.");
                break;
            case 2: // 효과음 소리 크기
                gameData.sfxVolume = SoundManager.Instance.sfxSource.volume;
                Debug.Log("효과음 크기가 " + gameData.sfxVolume + " 로 저장됩니다.");
                break;
            case 3: // 최초 엔딩 시에만 보여주게끔
                gameData.last = false;
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
        switch(level)
        {
            case 1:
                // 안깼고 깨야할 스테이지와 스테이지 잠금해제에 쓰인 스테이지와 같다면
                if(gameData.forestClear[stage-1] == false && gameData.forestStage == stage)
                {
                    gameData.forestClear[stage-1] = true;
                    gameData.forestStage+=1;
                }
                break;
            case 2:
                if(gameData.desertClear[stage-1] == false && gameData.desertStage == stage)
                {
                    gameData.desertClear[stage-1] = true;
                    gameData.desertStage+=1;
                }
                break;
            case 3:
                if(gameData.oceanClear[stage-1] == false && gameData.oceanStage == stage)
                {
                    gameData.oceanClear[stage-1] = true;
                    gameData.oceanStage+=1;
                }
                break;
            case 4:
                if(gameData.pastureClear[stage-1] == false && gameData.pastureStage == stage)
                {
                    gameData.pastureClear[stage-1] = true;
                    gameData.pastureStage+=1;
                }
                break;
            case 5:
                if(gameData.spaceClear[stage-1] == false && gameData.spaceStage == stage)
                {
                    gameData.spaceClear[stage-1] = true;
                    gameData.spaceStage+=1;
                }
                break;
            default:
                break;
        }

        if(level==1 && stage==10) // 1레벨 다 깼으면
            gameData.recentLevel = 2 ;
        if(level==2 && stage==10)
            gameData.recentLevel = 3;
        if(level==3 && stage==10)
            gameData.recentLevel = 4;
        if(level==3 && stage==10)
            gameData.recentLevel = 5;
        SaveGameData();
    }

    // private void OnApplicationQuit() // 게임 종료 시
    // {
    //     SaveGameData();
    // }
}
