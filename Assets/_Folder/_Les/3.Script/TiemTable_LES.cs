using TMPro;
using UnityEngine;

public class TiemTable_LES : MonoBehaviour
{
    public TextMeshProUGUI time; //시간 표시용

    //시간 로직
    public float speed = 60f; // 실제 1초 = 60 게임 시간초
    private float gameTimeSec; // 현재 게임 시간(초)

    private void Start()
    {
        gameTimeSec = 7 * 3600 + 30 * 60;    
    }

    private void Update()
    {
        gameTimeSec += Time.deltaTime * speed;
        DisplayTime(gameTimeSec);
    }


    public void DisplayTime(float gameTimeSec)
    {
        int hh = Mathf.FloorToInt(gameTimeSec / 3600f) % 24;
        int mm = Mathf.FloorToInt((gameTimeSec % 3600f) / 60f);
        string period = hh >= 12 ? "PM" : "AM";
        time.text = $"{hh:00}:{mm:00} {period}";
    }
}
