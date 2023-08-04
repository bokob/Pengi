using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SwitchRecentStageSelection : MonoBehaviour // 깨야할 스테이지 있는 곳으로 이동
{
    public void RecentStageSelection()
    {
        switch(DataManager.Instance.gameData.recentLevel)
        {
            case 1:
                SceneManager.LoadScene("ForestStageSelectionScene");
                break;
            case 2:
                SceneManager.LoadScene("DesertStageSelectionScene");
                break;
            case 3:
                SceneManager.LoadScene("OceanStageSelectionScene");
                break;
            case 4:
                SceneManager.LoadScene("PastureStageSelectionScene");
                break;
            case 5:
                SceneManager.LoadScene("SpaceStageSelectionScene");
                break;
            default:
                break;
        }   
    }
}
