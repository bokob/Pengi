using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SwitchRecentStageSelection : MonoBehaviour // 깨야할 스테이지 있는 곳으로 이동
{
    public void RecentStageSelection()
    {

        if(DataManager.Instance.gameData.first) // 첫 플레이면 오프닝 씬 이동
        {
            SceneManager.LoadScene("JourneyToEarthScene");
        }
        else // 오프닝 안봐도 되고 깨야할 레벨의 스테이지 선택으로 바로 이동
        {
            /* 엔딩 유무에 따른 분기도 나중에 작성 예정 */

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
}
