using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnClickNextScene : MonoBehaviour // 버튼 누르면 다음 씬으로 전환
{
    public string SceneName; // 넘어갈 씬 이름

    public void SwtichNextScene() 
    {
        SceneManager.LoadScene(SceneName);
        SoundManager.Instance.PlaySFX("Click");
    }
}
