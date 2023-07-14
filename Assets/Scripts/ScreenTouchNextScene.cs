using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ScreenTouchNextScene : MonoBehaviour
{
    public string SceneName; 
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}
