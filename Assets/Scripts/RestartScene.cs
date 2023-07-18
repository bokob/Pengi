using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    public void Restart()
    {
        // 현재 씬을 다시 불러옴
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
