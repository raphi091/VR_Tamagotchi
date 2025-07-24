using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager_LES : MonoBehaviour
{
    public static TimeManager_LES instance = null;
    public TextMeshPro timeText;
    public TextMeshPro dayText; //시간 표시용

    //시간 로직
    public float speed = 60f*5f; // 실제 1초 = 60 게임 시간초
    private float gameTimeSec; // 현재 게임 시간(초)

    // 요일 배열
    private string[] daysOfWeek = { "MON", "TUE", "WED", "THU", "FRI"};

    //temp
    public bool debug;
    //temp

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // 현재 씬 확인
        string currentScene = SceneManager.GetActiveScene().name;
        
        // 점심 씬이나 로비 씬에서는 시간 업데이트 중지
        if (currentScene == "H_Lunch" || currentScene == "H_Lobby")
        {
            return;
        }

        if (!TutorialManager_J.instance.Page.IsTutorial)
        {
            gameTimeSec += Time.deltaTime * speed;
            DisplayTime(gameTimeSec);
        }            
    }

    // 현재 요일을 반환하는 메서드
    private string GetDayOfWeek()
    {
        // DataManager에서 Day 값을 가져옴 (Day 1 = 월요일)
        int currentDay = DataManager_J.instance.gameData.Day;
        
        // Day는 1부터 시작하므로 1을 빼고, 7로 나눈 나머지로 요일 계산
        int dayIndex = (currentDay - 1) % 7;
        
        return daysOfWeek[dayIndex];
    }

    public void IndoorTime()
    {
        StartCoroutine(Indoor_co());
    }

    private IEnumerator Indoor_co()
    {
        gameTimeSec = 7 * 3600 + 30 * 60;

        // 아침 시간
        while (gameTimeSec < 8 * 3600 + 00 * 60)
        {
            gameTimeSec += Time.deltaTime * speed;
            DisplayTime(gameTimeSec);

            yield return null;
        }
        
        //오전 수업 시간.
        while (gameTimeSec <= 12 * 3600 + 00 * 60)
        {
            gameTimeSec += Time.deltaTime * speed;
            DisplayTime(gameTimeSec);

            yield return null;
        }

        GameManager.instance.GoToScene("H_Lunch");
    }

    public void OutdoorTime()
    {
        StartCoroutine(Outdoor_co());
    }

    private IEnumerator Outdoor_co()
    {
        gameTimeSec = 13 * 3600 + 00 * 60;

        //오후 수업시간.
        while (gameTimeSec <= 17 * 3600 + 00 * 60)
        {
            gameTimeSec += Time.deltaTime * speed;
            DisplayTime(gameTimeSec);

            yield return null;
        }

        // 하루 마무리 시간
        while (gameTimeSec < 17 * 3600 + 30 * 60)
        {
            gameTimeSec += Time.deltaTime * speed;
            DisplayTime(gameTimeSec);

            yield return null;
        }
        
        GameManager.instance.EndOfDay();
        GameManager.instance.GoToScene("H_Indoor");
    }

    public void DisplayTime(float gameTimeSec)
    {
        int hh = Mathf.FloorToInt(gameTimeSec / 3600f) % 24;
        int mm = Mathf.FloorToInt((gameTimeSec % 3600f) / 60f);

        // 요일과 시간을 함께 표시
        string dayOfWeek = GetDayOfWeek();
        timeText.text = $"{hh:00}:{mm:00}";
        dayText.text = $"{dayOfWeek}";
    }
}