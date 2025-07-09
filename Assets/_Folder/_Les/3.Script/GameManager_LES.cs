using UnityEngine;
using UnityEngine.SceneManagement;

public enum timetable
{
    none,
    morning_report, //아침 보고
    period_1, //1교시
    period_2, //2교시
    lunch_time, //점심시간
    period_3, //3교시
    period_4, //4교시 : 개별관리
    final_report // 최종 보고
}

public class GameManager_LES : MonoBehaviour
{
    public static GameManager_LES Instance; // 싱글톤으로 키세팅매니저 저장

    private timetable currentTimetable;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public timetable CurrentTimetable => currentTimetable;

    public void Main_To_InSide()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void InSide_To_Main()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void InSide_To_OutSide()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    public void OutSide_To_InSide()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void InSide_To_DogCareRoom()
    {
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }

    public void DogCareRoom_To_Inside()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
