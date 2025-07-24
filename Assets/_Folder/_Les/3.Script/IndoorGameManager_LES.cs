using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class IndoorGameManager_LES : MonoBehaviour
{
    public static IndoorGameManager_LES Instance;

    [SerializeField] private TextMeshPro num;
    [SerializeField] private TextMeshPro day;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Start()
    {
        if (DataManager_J.instance.gameData.Day.Equals(1))
            SoundManager.Instance.PlayBGM(BGMTrackName.Tutorial);
        else
            SoundManager.Instance.PlayBGM(BGMTrackName.Indoor);

        TimeManager_LES.instance.timeText = num;
        TimeManager_LES.instance.dayText = day;

        // 인도어, 아웃도어 초기 시간 및 날짜 설정 추가
        TimeManager_LES.instance.SetIndoorInitialTime();

        yield return new WaitUntil(() => TutorialManager_J.instance.Page.IsTutorial == false);

        TimeManager_LES.instance.IndoorTime();
    }
}
