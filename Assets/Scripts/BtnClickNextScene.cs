using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnClickNextScene : MonoBehaviour // 버튼 누르면 다음 씬으로 전환
{
    public string SceneName; // 넘어갈 씬 이름


    public void MainToJourney()
    {
        SceneManager.LoadScene("JourneyToEarthScene");
    }

    public void SwtichNextScene() 
    {

        if(DataManager.Instance.gameData.first) // 첫 플레이면 오프닝 이동
        {
            MainToJourney();
        }
        else // 레벨 선택 화면으로 이동
        {
            SceneManager.LoadScene(SceneName);
            SoundManager.Instance.PlaySFX("Click");
        }
    }
}
