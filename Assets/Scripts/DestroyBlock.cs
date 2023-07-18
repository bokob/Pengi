using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DestroyBlock : MonoBehaviour
{
    /*
    아직 서버 또는 json을 이용하지 않은 것이기 때문에
    임시로 만든 퍼즐의 블록들의 정보를 들고 있고 파괴하는 식으로 구현함
    */
    TextMeshProUGUI recognitionWord, selectedWord;
    bool flag = false;

    GameObject[] buttons;

    void Start()
    {
        buttons = GameObject.FindGameObjectsWithTag("Block");

        // foreach (GameObject button in buttons)
        // {
        //     TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

        //     // TextMeshProUGUI[] textComponents = button.GetComponentsInChildren<TextMeshProUGUI>();

        //     // foreach (TextMeshProUGUI textComponent in textComponents)
        //     // {
        //     //     Debug.Log("Button Text: " + textComponent.text);
        //     // }
        //     Debug.Log(buttonText.text);

        //     // if (buttonText != null && buttonText.text == targetText)
        //     // {
        //     //     Destroy(button.gameObject);
        //     // }
        // }

        // 인식, 선택 단어가 적히는 곳을 찾는다.
        recognitionWord = GameObject.Find("RecognitionWord").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        selectedWord = GameObject.Find("SelectedWord").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(recognitionWord.text == selectedWord.text)
            flag = true;

        if(flag)
            RemoveBlock(selectedWord.text);
    }

    void RemoveBlock(string word)
    {
        foreach (GameObject button in buttons)
        {
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

            // TextMeshProUGUI[] textComponents = button.GetComponentsInChildren<TextMeshProUGUI>();

            // foreach (TextMeshProUGUI textComponent in textComponents)
            // {
            //     Debug.Log("Button Text: " + textComponent.text);
            // }

            if(buttonText.text == word) 
            {
                Debug.Log(buttonText.text+"가 비활성 됩니다."); // 파괴시키면 오류남
                button.SetActive(false);
                break;
            }
            // if (buttonText != null && buttonText.text == targetText)
            // {
            //     Destroy(button.gameObject);
            // }
        }
        flag=false;
    }
}
