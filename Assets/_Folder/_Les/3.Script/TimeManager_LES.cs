using System.Collections;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager_LES : MonoBehaviour
{
    public static TimeManager_LES instance = null;
    public TextMeshProUGUI time; //시간 표시용

    public Canvas canvas;

    //시간 로직
    public float speed = 60f; // 실제 1초 = 60 게임 시간초
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
        canvas.gameObject.SetActive(true);

        // 아침 보고 및 오전시간.
        while (gameTimeSec < 8 * 3600)
        {
            gameTimeSec += Time.deltaTime * speed;
            DisplayTime(gameTimeSec);

            yield return null;
        }

        canvas.gameObject.SetActive(false);

        //오전 수업 시간.
        while (gameTimeSec >= 12 * 3600)
        {
            gameTimeSec += Time.deltaTime * speed;
            DisplayTime(gameTimeSec);

            yield return null;
        }

        // 점심시간
        // while (gameTimeSec < 13 * 3600)
        // {
        //     gameTimeSec += Time.deltaTime * speed;
        //     DisplayTime(gameTimeSec);

        //     yield return null;
        // }

        // 씬로드
        SceneManager.LoadScene("TesrScene 1", LoadSceneMode.Single);
    }

    public void OutdoorTime()
    {
        StartCoroutine(Outdoor_co());
    }

    private IEnumerator Outdoor_co()
    {
        gameTimeSec = 13 * 3600;

        //오후 수업시간.
        while (gameTimeSec >= 17 * 3600)
        {
            gameTimeSec += Time.deltaTime * speed;
            DisplayTime(gameTimeSec);

            yield return null;
        }

        // 씬로드(오후 보고 로직)
        
    }

    public void DisplayTime(float gameTimeSec)
    {
        int hh = Mathf.FloorToInt(gameTimeSec / 3600f) % 24;
        int mm = Mathf.FloorToInt((gameTimeSec % 3600f) / 60f);
        time.text = $"{hh:00}:{mm:00}";
    }
}
