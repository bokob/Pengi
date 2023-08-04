using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchGraphAndPhoneme : MonoBehaviour
{
    public void Prev()
    {
        ShowGraphAndPhoneme.Instance.PreviousObj();
        //SoundManager.Instance.PlaySFX("Click");
    }
    public void Next()
    {
        ShowGraphAndPhoneme.Instance.NextObj();
        //SoundManager.Instance.PlaySFX("Click");
    }
    public void GotoMainScene()
    {
        SceneManager.LoadScene("MainScene");
        //SoundManager.Instance.PlaySFX("Click");
    }
}
