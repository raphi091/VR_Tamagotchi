using System.Collections;
using UnityEngine;
using TMPro;

public class OutdoorGameManager_LES : MonoBehaviour
{
    public static OutdoorGameManager_LES Instance;

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
            SoundManager.Instance.PlayBGM(BGMTrackName.Outdoor);

        TimeManager_LES.instance.timeText = num;
        TimeManager_LES.instance.dayText = day;

        yield return new WaitUntil(() => TutorialManager_J.instance.Page.IsTutorial == false);

        TimeManager_LES.instance.OutdoorTime();
    }
}
