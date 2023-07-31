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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<GameObject> buttonList = new List<GameObject>();


    public void AddButton(GameObject button) // 리스트 요소 집어넣기
    {
        buttonList.Add(button);
    }

    public void PopAllButton() // 큐의 모든 요소 삭제
    {
        buttonList.Clear();
    }
}
