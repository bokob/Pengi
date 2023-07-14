using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JourneyToEarthSceneGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DataManager.Instance.UpdateGameSetData(0);
        Debug.Log("이제 오프닝 다시 안나올거임");
    }
}
