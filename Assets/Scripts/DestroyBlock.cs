// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using UnityEngine.SceneManagement;
// using UnityEngine.Networking;
// using System.IO;

// // [System.Serializable]
// // public class GameResult // 게임 결과
// // {
// //     public int user_id; // 유저 id
// //     public int chance=0; // 찬스 사용 횟수
// //     public string completed; // "1": 성공, "0": 실패

// //     public List<CorrectWord> correct_words; // 맞은 단어 배열
// //     public List<WrongWord> wrong_words; // 틀린 단어 배열

// // }

// // [System.Serializable]
// // public class CorrectWord
// // {
// //     public int word_id; // 단어 id
// //     public string word_name; // 단어
// // }

// // [System.Serializable]
// // public class WrongWord
// // {
// //     public int word_id; // 단어 id
// //     public string word_name; // 단어
// //     public string spell_name; // 틀리게 말한 내용
// // }

// public class DestroyBlock : MonoBehaviour
// {
//     /*
//     아직 서버 또는 json을 이용하지 않은 것이기 때문에
//     임시로 만든 퍼즐의 블록들의 정보를 들고 있고 파괴하는 식으로 구현함
//     */

//     public static DestroyBlock Instance;
//     public GameResult gameResult;
//     CorrectWord correctWord;
//     WrongWord wrongWord;

//     private string userIDFileName = "UserID.json"; // 유저 ID 저장되어 있는 json 파일 이름

//     private string filePath;

//     TextMeshProUGUI recognitionWord, selectedWord;
//     bool flag = false;

//     bool wrongAnswerProcessed = false;
//     GameObject[] buttons;

//     string sceneName;

//     void Start()
//     {

//         filePath = Path.Combine(Application.persistentDataPath, userIDFileName);

//         gameResult = new GameResult();
//         gameResult.correct_words = new List<CorrectWord>();
//         gameResult.wrong_words = new List<WrongWord>();


//         buttons = GameObject.FindGameObjectsWithTag("Block");

//         sceneName = SceneManager.GetActiveScene().name;

//         // foreach (GameObject button in buttons)
//         // {
//         //     TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

//         //     // TextMeshProUGUI[] textComponents = button.GetComponentsInChildren<TextMeshProUGUI>();

//         //     // foreach (TextMeshProUGUI textComponent in textComponents)
//         //     // {
//         //     //     Debug.Log("Button Text: " + textComponent.text);
//         //     // }
//         //     Debug.Log(buttonText.text);

//         //     // if (buttonText != null && buttonText.text == targetText)
//         //     // {
//         //     //     Destroy(button.gameObject);
//         //     // }
//         // }

//         // 인식, 선택 단어가 적히는 곳을 찾는다.
//         recognitionWord = GameObject.Find("RecognitionWord").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
//         selectedWord = GameObject.Find("SelectedWord").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if(recognitionWord.text == selectedWord.text && selectedWord.text!="") // 제대로 말 했을 때
//             flag = true;

//         if(flag)
//         {
//             // RemoveBlock(selectedWord.text);

//             /*
//                 정답쪽에 넣어야 됨

//                 CorrectWord 를 매번 '새로' 만들어서
//                 담은 다음에 
//                 AddCoreectWord()로 추가해줘야 한다.

//             */
            
//             CorrectWord newCorrectWord = new CorrectWord();
//             // newCorrectWord.word_id=GetWordId(selectedWord.text);

//             if(newCorrectWord.word_id == -1) // 없는 단어 선택했을 때
//             {
//                 ClearText();
//                 return;
//             }

//             newCorrectWord.word_name=selectedWord.text;
//             Debug.Log(newCorrectWord.word_name + " 이거를 넣을겁니다.");
//             AddCoreectWord(newCorrectWord);
//             Debug.Log(gameResult.correct_words[0].word_name + " 가 들어있네요");
//             Debug.Log("현재 사용한 찬스 : " + gameResult.chance);
//             Debug.Log("맞은 단어의 개수: " + gameResult.correct_words.Count);
//             Debug.Log("틀린 단어의 개수: " + gameResult.wrong_words.Count);

//             RemoveBlock();
//             selectedWord.text = "성공"; // 이거 안해주면 중간에 브금 끊김
//             Invoke("ClearText", 2f); // 2초뒤에 인식, 선택 글씨 비우기
//             flag=false;
//         }
//         else if(flag==false && selectedWord.text!="") // 실패
//         {
//             Debug.Log("실패야");
//             // if(recognitionWord.text != "Start!")
//             // {
//             //     if(recognitionWord.text=="") // 공백이면 담지 않고
//             //         return;
//             //     else // 말한게 있으면 담는다.
//             //     {
//             //         if(!wrongAnswerProcessed)
//             //         {
//             //             WrongWord newWrongWord = new WrongWord();
//             //             //newWrongWord.word_id=GetWordId(selectedWord.text);

//             //             if(newWrongWord.word_id == -1) // 없는 단어 선택했을 때
//             //             {
//             //                 ClearText();
//             //                 return;
//             //             }
//             //             newWrongWord.word_name=selectedWord.text; // 선택한 단어(정답)
//             //             newWrongWord.spell_name=recognitionWord.text; // 틀리게 발음한 것
//             //             // SendGameResult.Instance.AddWrongWord(newWrongWord);
//             //             wrongAnswerProcessed = true;
//             //         }
//             //     }
//             // }
//         }
//     }

//     void RemoveBlock()
//     {
//         GameObject[] buttonArray = BlockQueue.Instance.buttonQueue.ToArray(); // 큐에 넣어놓은 버튼들을 인덱스로 접근하기 위해 배열로 바꾼다.

//         Debug.Log("큐에 들어있는 버튼들 비활성화 시작!");
//         for(int i=0;i<buttonArray.Length;i++) // 전부 비활성화
//         {
//             Debug.Log(buttonArray[i].name + " 비활성화");
//             buttonArray[i].SetActive(false);
//         }
//         Debug.Log("큐 비활성화 종료!");
//         BlockQueue.Instance.DequeueAllButton(); // 큐 비우기

//         SoundManager.Instance.PlaySFX("BreakBlock"); // '풍덩' 효과음
//     }

//     void ClearText() // 글자 비우기
//     {
//         recognitionWord.text = "";
//         selectedWord.text = "";
//     }

//     public int GetWordId(string word)
//     {
//         int id=-1;

//         if(sceneName == "ForestPlayScene")
//         {
//             for(int i=0;i<LoadWord_F.Instance.answerList.Count;i++)
//             {
//                 if(LoadWord_F.Instance.answerList[i].w_name == word)
//                     id = LoadWord_F.Instance.answerList[i].w_id;
//             }
//         }
//         else if(sceneName == "DesertPlayScene")
//         {
//             for(int i=0;i<LoadWord_D.Instance.answerList.Count;i++)
//             {
//                 if(LoadWord_D.Instance.answerList[i].w_name == word)
//                     id = LoadWord_D.Instance.answerList[i].w_id;
//             }
//         }
//         else if(sceneName == "OceanPlayScene")
//         {
//             for(int i=0;i<LoadWord_O.Instance.answerList.Count;i++)
//             {
//                 if(LoadWord_O.Instance.answerList[i].w_name == word)
//                     id = LoadWord_O.Instance.answerList[i].w_id;
//             }
//         }
//         else if(sceneName == "PasturePlayScene")
//         {
//             for(int i=0;i<LoadWord_P.Instance.answerList.Count;i++)
//             {
//                 if(LoadWord_P.Instance.answerList[i].w_name == word)
//                     id = LoadWord_P.Instance.answerList[i].w_id;
//             }
//         }
//         else if(sceneName == "SpacePlayScene")
//         {
//             for(int i=0;i<LoadWord_S.Instance.answerList.Count;i++)
//             {
//                 if(LoadWord_S.Instance.answerList[i].w_name == word)
//                     id = LoadWord_S.Instance.answerList[i].w_id;
//             }
//         }

//         return id;
//     }

//     public void AddCoreectWord(CorrectWord item)
//     {
//         gameResult.correct_words.Add(item);
//     }

//     public void AddWrongWord(WrongWord item)
//     {
//         gameResult.wrong_words.Add(item);
//     }

//     public void Send()
//     {
//         /*
//             여기 써야할 코드는 
//             틀린 단어와 맞았던 단어 담은 것을 
//             보내야 됨
//         */

//         gameResult.user_id= JsonUtility.FromJson<ReceiveData>(filePath).user_id;
//         //gameResult.chance=1;
//         //gameResult.completed="1";

//         // gameResult.correct_words.Add(correctWord);
//         // gameResult.wrong_words.Add(wrongWord);
//         string jsonData = JsonUtility.ToJson(gameResult,true);

//         Debug.Log("게임결과 보내기 시작");
//         Debug.Log(jsonData);
//         Debug.Log("이러한 내용을 보낼겁니다.");

//     //    StartCoroutine(Upload("http://43.202.24.176:8080/api/game", jsonData));
//     }

//     IEnumerator Upload(string url, string json)
//     {
//         using (UnityWebRequest request = UnityWebRequest.Post(url, json))
//         {
//             byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json); // string으로 넘기면 json 구성이 깨지기 때문에 byte로 변환 후 파일로 업로드한다.
//             request.uploadHandler = new UploadHandlerRaw(jsonToSend);
//             request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
//             request.SetRequestHeader("Content-Type", "application/json"); // HTTP 헤더 설정 "Content-Type"으로 설정해 JSON 데이터임을 서버에 알린다.

//             yield return request.SendWebRequest();

//             if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
//             {   // 연결이 안되거나 프로토콜 오류인 경우
//                 Debug.Log(request.error);
//             }
//             else // 잘 온 경우
//             {
//                 Debug.Log(request.downloadHandler.text);
//                 DataContainer dataContainer = JsonUtility.FromJson<DataContainer>(request.downloadHandler.text);
//                 Debug.Log(dataContainer.status);
//             }
//             request.Dispose(); // 메모리 누수 막기 위해
//         }
//     }






// }
