using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AzureSky;

public class Weather_K : MonoBehaviour
{
    public static Weather_K instance = null;
    private AzureCoreSystem azureCore;

    [Header("날씨 설정")]
    public bool enableRandomWeather = false;

    [Range(0f, 1f)] public float rainChance = 0.25f;
    [Range(0f, 1f)] public float snowChance = 0.05f;


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

        if (!TryGetComponent(out azureCore))
            Debug.LogError("Weather_K ] AzureCoreSystem 없음");
    }

    public void SetDailyWeather()
    {
        if (!enableRandomWeather)
        {
            azureCore.weatherSystem.SetGlobalWeather(0);
            Debug.Log("오늘의 날씨: 맑음 (랜덤 날씨 비활성화됨)");
            return;
        }

        float randomValue = Random.value;

        if (randomValue < snowChance)
        {
            azureCore.weatherSystem.SetGlobalWeather(8);
            Debug.Log("오늘의 날씨: 눈");
        }
        else if (randomValue < snowChance + rainChance)
        {
            azureCore.weatherSystem.SetGlobalWeather(6);
            Debug.Log("오늘의 날씨: 비");
        }
        else
        {
            azureCore.weatherSystem.SetGlobalWeather(0);
            Debug.Log("오늘의 날씨: 맑음");
        }
    }

    public void SyncTime(float gameTimeSec)
    {
        if (azureCore == null) return;

        float timelineHour = gameTimeSec / 3600.0f;
        azureCore.timeSystem.timeline = timelineHour;
    }
}
