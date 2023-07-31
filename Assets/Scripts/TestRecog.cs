using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestRecog : MonoBehaviour
{
    // Start is called before the first frame update
    

    string test = "테스트";
    string dtest = "테스트트";

    public TextMeshProUGUI textUI1;
    public TextMeshProUGUI textUI2;

    public void TestClick()
    {
        if(gameObject.name == "TestSameBtn")
        {
            textUI1.text = test;
            textUI2.text = test; 
        }
        else
        {
            textUI1.text = test;
            textUI2.text = dtest;
        }
    }
}
