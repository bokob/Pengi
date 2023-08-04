using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
블록 파괴, 선택 같은 행위 때문에 쓸 스크립트
*/

public class BlockList : MonoBehaviour
{
    // 싱글톤으로 만들기
    public static BlockList Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public List<GameObject> buttonList = new List<GameObject>();
    public List<string> buttonNameList = new List<string>();


    public void AddButton(GameObject button) // 리스트 요소 집어넣기
    {   

        if (!buttonList.Contains(button))
        {
            buttonList.Add(button);
        }

        if(!buttonNameList.Contains(button.name))
        {
            buttonNameList.Add(button.name);
        }

        foreach(var name in buttonNameList)
        {
            Debug.Log("이름리스트에 " + name + " 들어있는뎁쇼");
        }


    }

    public void PopButton()
    {
        if (buttonList.Count > 0)
        {
            int lastIndex = buttonList.Count - 1;
            buttonList.RemoveAt(lastIndex); // 마지막 요소를 제거

            int lastIdx = buttonNameList.Count-1;
            buttonNameList.RemoveAt(lastIdx);
        }
        else
        {
            buttonList.Clear();
            buttonNameList.Clear();
        }
    }

    public void PopAllButton() // 큐의 모든 요소 삭제
    {
        buttonList.Clear();
        buttonNameList.Clear();
    }
}
