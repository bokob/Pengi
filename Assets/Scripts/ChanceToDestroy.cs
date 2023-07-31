using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ChanceToDestroy : MonoBehaviour
{
    int chanceCount;
    public TextMeshProUGUI recognitionWord, selectedWord, showChanceCount;

    void Start()
    {

        Debug.Log(recognitionWord.name);

        string sceneName = SceneManager.GetActiveScene().name;

        if(sceneName == "ForestPlayScene" || sceneName == "DesertPlayScene")
        {
            chanceCount = 1;
        }
        else if(sceneName == "OceanPlayScene" || sceneName == "PasturePlayScene")
        {
            chanceCount = 2;
        }
        else if(sceneName == "SpacePlayScene")
        {
            chanceCount = 3;
        }
        showChanceCount.text = chanceCount.ToString();
    }

    void Update()
    {
        if(showChanceCount.text == "0")
            gameObject.GetComponent<Button>().interactable=false;
    }

    public void Chance()
    {
        GameObject[] buttonArray = BlockList.Instance.buttonList.ToArray(); // 리스트에 넣어놓은 버튼들을 인덱스로 접근하기 위해 배열로 바꾼다.

        Debug.Log("큐에 들어있는 버튼들 비활성화 시작!");
        for(int i=0;i<buttonArray.Length;i++) // 전부 비활성화
        {
            Debug.Log(buttonArray[i].name + " 비활성화");
            buttonArray[i].SetActive(false);
        }
        Debug.Log("큐 비활성화 종료!");
        BlockList.Instance.PopAllButton(); // 리스트 비우기

        showChanceCount.text = (int.Parse(showChanceCount.text)-1).ToString();

        SoundManager.Instance.PlaySFX("BreakBlock"); // '풍덩' 효과음
    }

    void ClearText() // 글자 비우기
    {
        recognitionWord.text = "";
        selectedWord.text = "";
    }

    public void Onccclick()
    {
        Debug.Log("눌리는뎁쇼?");
    }
}
