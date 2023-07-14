using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PressObjectDebug : MonoBehaviour
{
    private TextMeshProUGUI recordTextObj;
    private TextMeshProUGUI ClickedWord;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 왼쪽 마우스 버튼 클릭
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("클릭한 오브젝트의 이름: " + hit.collider.gameObject.name);

                // 클릭한 블록의 단어에 접근
                TextMeshPro textComponent = GameObject.Find(hit.collider.gameObject.name).GetComponentInChildren<TextMeshPro>();

                // 클릭한 단어를 나타낼 곳을 찾는다
                ClickedWord = GameObject.Find("SelectedWordOfBlock").GetComponent<TextMeshProUGUI>();

                ClickedWord.text = textComponent.text; // 집어넣는다.

                // 녹음된 것이 기록되는 곳을 찾는다
                recordTextObj = GameObject.Find("Canvas").GetComponentInChildren<TextMeshProUGUI>();

                Debug.Log("녹음된 결과: " + recordTextObj.text + " " + "클릭한 단어: " + textComponent.text);
                if (recordTextObj.text == ClickedWord.text) // 선택한 단어와 인식된 발음이 일치하면
                {
                    Destroy(hit.collider.gameObject, 3f); // 3초 뒤에 파괴
                }
            }
        }
    }


}
