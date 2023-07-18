using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupUI : MonoBehaviour
{
    public GameObject obj;

    public void Popup()
    {
        obj.SetActive(true);
    }

    public void Exit()
    {
        obj.SetActive(false);
    }
}
