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
        WriteSelectedWord.text += buttonText.text;

        BlockList.Instance.AddButton(gameObject); // 큐에 집어넣는다.
    
        // if(WriteSelectedWord.text == "") // 공백이라면
        // {
        //     if(buttonText.text == "") // 버튼 안의 글자가 공백이면 담지 않음
        //         return;

        //     Debug.Log(buttonText.text + "를 넣을거에요"); 
        //     BlockQueue.Instance.EnqueueButton(gameObject); // 큐에 집어넣는다.
        //     WriteSelectedWord.text += buttonText.text;
        // }
        // else if (WriteSelectedWord.text != "") // 공백이 아니면 무언가 들어가 있으니까 그 글자색을 기준으로 추가되어야 함
        // {

        //     BlockQueue.Instance.EnqueueButton(gameObject);
        //     Debug.Log("색깔이 일치해서 집어넣었어요");

        //     Debug.Log("큐의 맨 앞에 담긴 오브젝트의 이름은: " + BlockQueue.Instance.buttonQueue[0].name);
        //     GameObject firstButton = BlockQueue.Instance.buttonQueue[0];
        //     TextMeshProUGUI textMeshPro = firstButton.GetComponentInChildren<TextMeshProUGUI>();
        //     Color textColor = textMeshPro.color;
            
        //     if(textColor != BlockQueue.Instance.buttonQueue[BlockQueue.Instance.buttonQueue.Count-1].GetComponentInChildren<TextMeshProUGUI>().color) // 같은 색이 아니면
        //     {
        //         BlockQueue.Instance.DequeueButton(); // 무작정 넣어버린거 빼기
        //         Debug.Log("색이 달라 큐에서 뺐어요.");
        //     }
        //     else // 같은 색이면
        //     {
        //         Debug.Log(gameObject.name);
        //         if(!BlockQueue.Instance.buttonNameList.Contains(gameObject.name)) // 해당 버튼(버튼의 이름)이 안담겼다면
        //         {
        //             Debug.Log("여기가 작동됨1");
        //             WriteSelectedWord.text += buttonText.text;
        //         }
        //         else // 담기지 않았다면
        //         {
        //             Debug.Log("여기가 작동됨2");
        //             BlockQueue.Instance.DequeueButton(); // 위에서 무작정 담은거 빼버린다.
        //         }
        //     }
        // }

        // Debug.Log(buttonText.text + "를 넣을거에요"); 
        // WriteSelectedWord.text += buttonText.text;

        // BlockQueue.Instance.EnqueueButton(gameObject); // 큐에 집어넣는다.

        // WriteSelectedWord.text = buttonText.text; // 집어넣는다.

        // // 녹음된 것이 기록되는 곳을 찾는다
        // recordTextObj = GameObject.Find("Canvas").GetComponentInChildren<TextMeshProUGUI>();

        // Debug.Log("녹음된 결과: " + recordTextObj.text + " " + "클릭한 단어: " + textComponent.text);
        // if (recordTextObj.text == ClickedWord.text) // 선택한 단어와 인식된 발음이 일치하면
        // {
        //     Destroy(hit.collider.gameObject, 3f); // 3초 뒤에 파괴
        // }
    }

    public void DeleteSelectedBlockText() // 단어들이 쓰여진 곳 지워버리기
    {
        WriteSelectedWord.text = "";
        BlockList.Instance.PopAllButton();
    }
}
