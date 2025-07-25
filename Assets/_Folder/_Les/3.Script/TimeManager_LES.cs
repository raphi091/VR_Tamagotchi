using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AzureSky;
using UnityEngine.SceneManagement;

public class TimeManager_LES : MonoBehaviour
{
    public static TimeManager_LES instance = null;
    public TextMeshPro timeText;
    public TextMeshPro dayText; //시간 표시용

    public AudioClip sceneChange; //씬 전환전 알림소리
    public AudioClip doorBall; //시작시 문열림 소리
    public AudioClip door; // 다음 씬 전화 되는 소리

    //시간 로직
    public float speed = 60f * 5f; // 실제 1초 = 60 게임 시간초
    private float gameTimeSec; // 현재 게임 시간(초)
    private string currentScene;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬 로드가 완료되면 이 함수가 실행됩니다.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "H_Lobby")
            StopAllCoroutines();
    }

    // 현재 요일을 반환하는 메서드
    private string GetDayOfWeek()
    {
        // DataManager에서 Day 값을 가져옴 (Day 1 = 월요일)
        int currentDay = DataManager_J.instance.gameData.Day;
        
        // Day는 1부터 시작하므로 1을 빼고, 5로 나눈 나머지로 요일 계산
        int dayIndex = (currentDay - 1) % 5;
        
        return daysOfWeek[dayIndex];
    }

    // 인도어 초기 시간 설정 메소드
    public void SetIndoorInitialTime()
    {
        gameTimeSec = 7 * 3600 + 30 * 60; // 7:30 설정
        DisplayTime(gameTimeSec);
    }

    // 아웃도어 초기 시간 설정 메소드
    public void SetOutdoorInitialTime()
    {
        gameTimeSec = 13 * 3600 + 00 * 60; // 13:00 설정
        DisplayTime(gameTimeSec);
    }

    public void IndoorTime()
    {
        StartCoroutine(Indoor_co());
    }

    AzureCoreSystem azureCoreSystem;
    private IEnumerator Indoor_co()
    {
        SoundManager.Instance.PlaySFX(doorBall);
        gameTimeSec = 7 * 3600 + 30 * 60;

        // 아침 시간
        while (gameTimeSec < 8 * 3600 + 00 * 60)
        {
            gameTimeSec += Time.deltaTime * speed;
            DisplayTime(gameTimeSec);

            yield return null;
        }

        while (gameTimeSec <= 11 * 3600 + 50 * 60)
        {
            gameTimeSec += Time.deltaTime * speed;
            DisplayTime(gameTimeSec);

            yield return null;
        }

        SoundManager.Instance.PlaySFX(sceneChange);

        //오전 수업 시간.
        while (gameTimeSec <= 12 * 3600 + 00 * 60)
        {
            gameTimeSec += Time.deltaTime * speed;
            DisplayTime(gameTimeSec);

            yield return null;
        }

        SoundManager.Instance.PlaySFX(door);
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

        while (gameTimeSec <= 17 * 3600 + 20 * 60)
        {
            gameTimeSec += Time.deltaTime * speed;
            DisplayTime(gameTimeSec);

            yield return null;
        }

        SoundManager.Instance.PlaySFX(sceneChange);

        // 하루 마무리 시간
        while (gameTimeSec < 17 * 3600 + 30 * 60)
        {
            gameTimeSec += Time.deltaTime * speed;
            DisplayTime(gameTimeSec);

            yield return null;
        }

        SoundManager.Instance.PlaySFX(doorBall);
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