using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTutorial : MonoBehaviour
{
    public Image targetImage; // Canvas의 Image 컴포넌트
    public Sprite[] images; // 이미지들을 저장할 배열

    public static ShowTutorial Instance; 

    public GameObject prev, next, back;

    private int currentIndex = 0; // 현재 보여지는 이미지의 인덱스

    // Start 또는 Awake 등에서 초기 이미지 설정
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        if (targetImage == null)
        {
            targetImage = GetComponent<Image>();
        }
    }

    // 이미지 변경 함수
    public void ChangeImage()
    {
        if (images.Length == 0)
            return;

        targetImage.sprite = images[currentIndex];
        UpdateButtonsState();
    }

    // 앞으로 가기 버튼을 누를 때 호출되는 함수
    public void NextImage()
    {
        currentIndex = (currentIndex + 1) % images.Length;
        Debug.Log("현재 인덱스: " + currentIndex);
        ChangeImage();
    }

    // 뒤로 가기 버튼을 누를 때 호출되는 함수
    public void PreviousImage()
    {
        currentIndex = (currentIndex - 1 + images.Length) % images.Length;
        Debug.Log("현재 인덱스: " + currentIndex);
        ChangeImage();
    }

    // 버튼 상태 업데이트 함수
    private void UpdateButtonsState()
    {
        prev.SetActive(currentIndex != 0);
        next.SetActive(currentIndex != images.Length - 1);
        back.SetActive(currentIndex == images.Length - 1);
    }
}