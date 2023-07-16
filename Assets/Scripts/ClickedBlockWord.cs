using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClickedBlockWord : MonoBehaviour
{
    Button button; // 블록
    TextMeshProUGUI buttonText; // 블록에 있는 단어  
    private TextMeshProUGUI  WriteSelectedWord; // 선택한 블록의 단어가 보여질 곳

    GameObject panelObject; // 패널

    void Start()
    {
        button = GetComponent<Button>();
        buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        panelObject = GameObject.Find("SelectedWord");

        if(panelObject!=null)
        {
            // // 클릭한 단어를 나타낼 곳을 찾는다
            // Transform firstChildTransform = panelObject.transform.GetChild(0);

            // Debug.Log("선택된 단어는 " + firstChildTransform.gameObject.name + "에 넣을겁니다.");
            Transform firstChildTransform = panelObject.transform.GetChild(0);
            WriteSelectedWord = firstChildTransform.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.Log("못찾음");
        }
    }

    public void ShowSelectedBlockText()
    {

        Debug.Log(buttonText.text + "를 넣을거에요"); 
        WriteSelectedWord.text = buttonText.text;

        // WriteSelectedWord.text = buttonText.text; // 집어넣는다.

        // // 녹음된 것이 기록되는 곳을 찾는다
        // recordTextObj = GameObject.Find("Canvas").GetComponentInChildren<TextMeshProUGUI>();

        // Debug.Log("녹음된 결과: " + recordTextObj.text + " " + "클릭한 단어: " + textComponent.text);
        // if (recordTextObj.text == ClickedWord.text) // 선택한 단어와 인식된 발음이 일치하면
        // {
        //     Destroy(hit.collider.gameObject, 3f); // 3초 뒤에 파괴
        // }
    }
}
