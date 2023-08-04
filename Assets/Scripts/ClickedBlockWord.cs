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

        // Debug.Log(buttonText.text + "를 넣을거에요"); 
        // WriteSelectedWord.text += buttonText.text;

        // BlockList.Instance.AddButton(gameObject); //  집어넣는다.
    
        if(WriteSelectedWord.text == "") // 공백이라면
        {
            if(buttonText.text == "") // 버튼 안의 글자가 공백이면 담지 않음
                return;

            Debug.Log(buttonText.text + "를 넣을거에요"); 
            BlockList.Instance.AddButton(gameObject); // 리스트에 집어넣는다.
            WriteSelectedWord.text += buttonText.text;
        }
        else if (WriteSelectedWord.text != "") // 공백이 아니면 무언가 들어가 있으니까 그 글자색을 기준으로 추가되어야 함
        {
            GameObject firstButton = BlockList.Instance.buttonList[0]; // 맨앞의 버튼에 접근
            TextMeshProUGUI textMeshPro = firstButton.GetComponentInChildren<TextMeshProUGUI>();
            Color textColor = textMeshPro.color; // 색을 알아낸다.
            
            if(textColor == gameObject.GetComponentInChildren<TextMeshProUGUI>().color) // 같은 색이면
            {
                Debug.Log("같은 색이네?");
                if (!BlockList.Instance.buttonNameList.Contains(gameObject.name)) // 중복되지 않았으면
                {
                    BlockList.Instance.AddButton(gameObject);
                    WriteSelectedWord.text += buttonText.text;
                }
            }
            else // 다른 색이면
            {
                Debug.Log("다른 색이네?");
            }
        }
    }

    public void DeleteSelectedBlockText() // 단어들이 쓰여진 곳 지워버리기
    {
        WriteSelectedWord.text = "";
        BlockList.Instance.PopAllButton();
    }
}
