// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.Networking;
// using TMPro;
// using System.IO;

// // [System.Serializable]
// // public class Accuracy
// // {
// //     public int status;
// //     public bool success;
// //     public string message;
// //     public AccuracyData data;
// // }

// // [System.Serializable]
// // public class AccuracyData
// // {
// //     public ABPPP accuracy_by_ph_pronunciation_position;
// //     public ABPPM accuracy_by_ph_pronunciation_method;
// //     public ABPV accuracy_by_ph_vowel;
// // }

// // [System.Serializable]
// // public class ABPPP
// // {
// //     public float research_velar_accuracy;
// //     public float dental_accuracy;
// //     public float bilabial_accuracy;
// //     public float velar_accuracy;
// //     public float glottal_accuracy;
// // }

// // [System.Serializable]
// // public class ABPPM
// // {
// //     public float stop_consonant_accuracy;
// //     public float nasal_consonant_accuracy;
// //     public float liquid_consonant_accuracy;
// //     public float fricative_consonant_accuracy;
// //     public float affricate_consonant_accuracy;
// // }

// // [System.Serializable]
// // public class ABPV
// // {
// //     public float diphthong_accuracy;
// //     public float front_vowel_accuracy;
// //     public float back_vowel_accuracy;
// //     public float high_vowel_accuracy;
// //     public float mid_vowel_accuracy;
// //     public float low_vowel_accuracy;
// //     public float plain_vowel_accuracy;
// //     public float round_vowel_accuracy;
// // }


// public class ShowPhoneme : MonoBehaviour
// {
//     public static ShowPhoneme Instance;

//     string url = "http://43.202.24.176:8080//api/phoneme/accuracy/";

//     Accuracy accuracy;

//     int userID;
//     string filePath, userIDFileName = "UserID.json", userIDFile;

//     // Start is called before the first frame update
//     private IEnumerator Start()
//     {
//         filePath = Path.Combine(Application.persistentDataPath, userIDFileName);
//         userIDFile = File.ReadAllText(filePath);
//         userID = JsonUtility.FromJson<ReceiveData>(userIDFile).user_id;
    
//         if(Instance == null)
//         {
//             Instance = this;
//         }

//         yield return StartCoroutine(LoadPhoneme(url, userID));

//         /*
//             결과 받은거 집어넣는 부분
//         */

//     }

//     IEnumerator LoadPhoneme(string url, int id)
//     {
//         UnityWebRequest request = UnityWebRequest.Get(url);

//         // id 변수를 "USER-ID"이라는 이름으로 헤더에 추가
//         request.SetRequestHeader("USER-ID", id.ToString());

//         yield return request.SendWebRequest();

//         if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
//         {
//             Debug.Log(request.error);
//         }
//         else
//         {
//             Debug.Log(request.downloadHandler.text);
//             // ReceiveWordData_S tmp = JsonUtility.FromJson<ReceiveWordData_S>(request.downloadHandler.text);
//             accuracy = JsonUtility.FromJson<Accuracy>(request.downloadHandler.text);
//             Debug.Log(accuracy.message);
            
//             Debug.Log("ABPPP 시작");
//             Debug.Log(accuracy.data.accuracy_by_ph_pronunciation_position.bilabial_accuracy);
//             Debug.Log(accuracy.data.accuracy_by_ph_pronunciation_position.dental_accuracy);
//             Debug.Log(accuracy.data.accuracy_by_ph_pronunciation_position.glottal_accuracy);
//             Debug.Log(accuracy.data.accuracy_by_ph_pronunciation_position.research_velar_accuracy);
//             Debug.Log(accuracy.data.accuracy_by_ph_pronunciation_position.velar_accuracy);
//             Debug.Log("ABPPP 끝");

//             Debug.Log("ABPPM 시작");
//             Debug.Log(accuracy.data.accuracy_by_ph_pronunciation_method.affricate_consonant_accuracy);
//             Debug.Log(accuracy.data.accuracy_by_ph_pronunciation_method.fricative_consonant_accuracy);
//             Debug.Log(accuracy.data.accuracy_by_ph_pronunciation_method.liquid_consonant_accuracy);
//             Debug.Log(accuracy.data.accuracy_by_ph_pronunciation_method.nasal_consonant_accuracy);
//             Debug.Log(accuracy.data.accuracy_by_ph_pronunciation_method.stop_consonant_accuracy);
//             Debug.Log("ABPPM 끝");

//             Debug.Log("ABPV 시작");
//             Debug.Log(accuracy.data.accuracy_by_ph_vowel.back_vowel_accuracy);
//             Debug.Log(accuracy.data.accuracy_by_ph_vowel.diphthong_accuracy);
//             Debug.Log(accuracy.data.accuracy_by_ph_vowel.front_vowel_accuracy);
//             Debug.Log(accuracy.data.accuracy_by_ph_vowel.high_vowel_accuracy);
//             Debug.Log(accuracy.data.accuracy_by_ph_vowel.low_vowel_accuracy);
//             Debug.Log(accuracy.data.accuracy_by_ph_vowel.mid_vowel_accuracy);
//             Debug.Log(accuracy.data.accuracy_by_ph_vowel.plain_vowel_accuracy);
//             Debug.Log(accuracy.data.accuracy_by_ph_vowel.round_vowel_accuracy);
//             Debug.Log("ABPV 시작");

//         }
//     }


// }
