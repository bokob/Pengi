using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupSettingUI : MonoBehaviour
{
    public GameObject obj;

    public void Popup()
    {
        obj.SetActive(true);
    }

    public void Exit()
    {
        if(GetComponent<Button>().name == "ExitButton")
        {
            DataManager.Instance.UpdateGameSetData(1); // 브금 크기 저장
            DataManager.Instance.UpdateGameSetData(2); // 효과음 크기 저장
        }

        obj.SetActive(false);
    }
}
