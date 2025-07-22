using System.Collections;
using TMPro;
using UnityEngine;

public class TimeManager_LES : MonoBehaviour
{
    public static TimeManager_LES instance = null;
    public TextMeshPro timeText; //시간 표시용

    public GameObject canvas0, canvas1;

    //시간 로직
    public float speed = 60f*5f; // 실제 1초 = 60 게임 시간초
    private float gameTimeSec; // 현재 게임 시간(초)

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
        gameTimeSec += Time.deltaTime * speed;
        DisplayTime(gameTimeSec);
    }

    public void IndoorTime()
    {
        StartCoroutine(Indoor_co());
    }

    private IEnumerator Indoor_co()
    {
        gameTimeSec = 7 * 3600 + 30 * 60;

        // 오전 보고 시작.
        if (canvas0 == null) yield return null;

        GameObject g = Instantiate(canvas0);

        // 아침 보고 및 오전시간.
        while (gameTimeSec < 8 * 3600 + 00 * 60)
        {
            gameTimeSec += Time.deltaTime * speed;
            DisplayTime(gameTimeSec);

            yield return null;
        }

        Destroy(g);
        
        //오전 수업 시간.
        while (gameTimeSec <= 12 * 3600 + 00 * 60)
        {
            gameTimeSec += Time.deltaTime * speed;
            DisplayTime(gameTimeSec);

            yield return null;
        }

        GameManager.instance.GoToScene("H_Lunch");
        //GameManager.instance.GoToScene("H_Outdoor");
    }

    public void LunchTime()
    {
        GameManager.instance.GoToScene("H_Outdoor");
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

        GameObject g = Instantiate(canvas0);

        // 다음날로 넘기는 로직 추가
        while (gameTimeSec < 17 * 3600 + 30 * 60)
        {
            gameTimeSec += Time.deltaTime * speed;
            DisplayTime(gameTimeSec);

            yield return null;
        }
        GameManager.instance.EndOfDay();

        Destroy(g);
        GameManager.instance.GoToScene("H_Indoor");
    }

    public void DisplayTime(float gameTimeSec)
    {
        int hh = Mathf.FloorToInt(gameTimeSec / 3600f) % 24;
        int mm = Mathf.FloorToInt((gameTimeSec % 3600f) / 60f);
        timeText.text = $"{hh:00}:{mm:00}";
    }
}
