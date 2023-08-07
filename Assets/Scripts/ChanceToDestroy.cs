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
        else if(sceneName == "SpacePlayScene" || sceneName == "WrongPlayScene" || sceneName == "DemoPlayScene")
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

    public void Chance() // 찬스는 틀린걸로 취급
    {
        GameObject[] buttonArray = BlockList.Instance.buttonList.ToArray(); // 리스트에 넣어놓은 버튼들을 인덱스로 접근하기 위해 배열로 바꾼다.

        Debug.Log("리스트에 들어있는 버튼들 비활성화 시작!");
        for(int i=0;i<buttonArray.Length;i++) // 전부 비활성화
        {
            Debug.Log(buttonArray[i].name + " 비활성화");
            buttonArray[i].SetActive(false);
        }
        Debug.Log("리스트 비활성화 종료!");

        //   ex) 원래 "수박"인데 "박수"로 고르고 찬스를 썼을 때 "박수"가 있는지 파악하는 것
        int wordID = STT.Instance.GetWordId(selectedWord.text); 

        if(wordID == -1) return;

        WrongWord newWrongWord = new WrongWord();
        newWrongWord.word_id = wordID;
        newWrongWord.word_name = selectedWord.text;
        newWrongWord.spell_name = "찬스를사용했습니다";
        STT.Instance.AddWrongWord(newWrongWord);

        BlockList.Instance.PopAllButton(); // 리스트 비우기
        selectedWord.text="";
        
        STT.Instance.gameResult.chance++; // 찬스 사용 횟수 증가

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

    public void ChanceDemo() // 찬스는 틀린걸로 취급
    {
        GameObject[] buttonArray = BlockList.Instance.buttonList.ToArray(); // 리스트에 넣어놓은 버튼들을 인덱스로 접근하기 위해 배열로 바꾼다.

        Debug.Log("리스트에 들어있는 버튼들 비활성화 시작!");
        for(int i=0;i<buttonArray.Length;i++) // 전부 비활성화
        {
            Debug.Log(buttonArray[i].name + " 비활성화");
            buttonArray[i].SetActive(false);
        }
        Debug.Log("리스트 비활성화 종료!");

        showChanceCount.text = (int.Parse(showChanceCount.text)-1).ToString();

        SoundManager.Instance.PlaySFX("BreakBlock"); // '풍덩' 효과음
    }
}
