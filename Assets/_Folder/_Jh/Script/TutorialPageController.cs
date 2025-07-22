using System.Collections.Generic;
using UnityEngine;

public class TutorialPageController : MonoBehaviour
{
    [Header("튜토리얼 페이지들 (Image1, Image2 등)")]
    public List<GameObject> tutorialPages;

    private int currentPage = 0;
    public static TutorialPageController instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializePages();
    }
    public GameObject tutorialRoot; // 전체 튜토리얼 UI를 감싸는 루트 오브젝트 (Canvas 포함)

    public void ShowNextPage()
    {
        if (tutorialPages == null || tutorialPages.Count == 0) return;

        if (currentPage < tutorialPages.Count)
        {
            tutorialPages[currentPage].SetActive(false);
        }

        currentPage++;

        if (currentPage < tutorialPages.Count)
        {
            tutorialPages[currentPage].SetActive(true);
        }
        else
        {
            // 전체 튜토리얼 루트를 비활성화
            if (tutorialRoot != null)
                tutorialRoot.SetActive(false);
            else
                Debug.LogWarning("튜토리얼 루트가 설정되지 않았습니다.");
        }
    }


    void InitializePages()
    {
        for (int i = 0; i < tutorialPages.Count; i++)
        {
            tutorialPages[i].SetActive(i == 0);
        }
    }
}
