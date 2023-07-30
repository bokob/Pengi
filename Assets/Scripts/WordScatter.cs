using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WordScatter : MonoBehaviour
{
    // Start is called before the first frame update

    string sceneName;

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        
        if(sceneName == "ForestPlayScene")
        {
            Scatter_F();
        }
        else if(sceneName == "DesertPlayScene")
        {

        }
        else if(sceneName == "OceanPlayScene")
        {
            
        }
        else if(sceneName == "PasturePlayScene")
        {
            
        }
        else if(sceneName == "SpacePlayScene")
        {
            
        }
    }

    public void Scatter_F() // 숲
    {
        Debug.Log("숲의 0행 0열에는 " + LoadWord_F.Instance.answerList[0]);
    }
}
