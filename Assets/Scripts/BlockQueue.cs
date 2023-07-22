using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockQueue : MonoBehaviour
{
    // 싱글톤으로 만들기
    public static BlockQueue Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Queue<GameObject> buttonQueue = new Queue<GameObject>();


    public void EnqueueButton(GameObject button) // 큐에 요소 집어넣기
    {
        buttonQueue.Enqueue(button);
    }

    public void DequeueAllButton() // 큐의 모든 요소 삭제
    {
        buttonQueue.Clear();
    }




}
