using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchTutorial : MonoBehaviour
{
    public void PrevTutorial()
    {
        ShowTutorial.Instance.PreviousImage();
        SoundManager.Instance.PlaySFX("Click");
    }
    public void NextTutorial()
    {
        ShowTutorial.Instance.NextImage();
        SoundManager.Instance.PlaySFX("Click");
    }
    public void GotoMainScene()
    {
        SceneManager.LoadScene("MainScene");
        SoundManager.Instance.PlaySFX("Click");
    }
}
