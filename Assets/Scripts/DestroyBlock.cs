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
        if(recognitionWord.text == selectedWord.text && selectedWord.text!="") // 제대로 말 했을 때
            flag = true;

        if(flag)
        {
            // RemoveBlock(selectedWord.text);

            /*
                정답쪽에 넣어야 됨

                CorrectWord 를 매번 '새로' 만들어서
                담은 다음에 
                AddCoreectWord()로 추가해줘야 한다.

            */



            RemoveBlock();
            selectedWord.text = "성공"; // 이거 안해주면 중간에 브금 끊김
            Invoke("ClearText", 2f); // 2초뒤에 인식, 선택 글씨 비우기
            flag=false;
        }
        else // 실패
        {
                /*
                    공백이면 담지 않고
                    뭐라도 들어있는데 틀린거면 담는다.

                    

                */
        }
    }

    void RemoveBlock()
    {
        GameObject[] buttonArray = BlockQueue.Instance.buttonQueue.ToArray(); // 큐에 넣어놓은 버튼들을 인덱스로 접근하기 위해 배열로 바꾼다.

        Debug.Log("큐에 들어있는 버튼들 비활성화 시작!");
        for(int i=0;i<buttonArray.Length;i++) // 전부 비활성화
        {
            Debug.Log(buttonArray[i].name + " 비활성화");
            buttonArray[i].SetActive(false);
        }
        Debug.Log("큐 비활성화 종료!");
        BlockQueue.Instance.DequeueAllButton(); // 큐 비우기

        SoundManager.Instance.PlaySFX("BreakBlock"); // '풍덩' 효과음
    }

    void ClearText() // 글자 비우기
    {
        recognitionWord.text = "";
        selectedWord.text = "";
    }
}
