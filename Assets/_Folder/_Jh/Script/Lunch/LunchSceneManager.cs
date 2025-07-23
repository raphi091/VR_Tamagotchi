using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LunchSceneManager : MonoBehaviour
{
    public static LunchSceneManager instance = null;
    public Ch_Bell bell;

    private LunchDog[] dogs;
    private FoodBowl[] foodBowls;
    private static int finishCount = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        finishCount = 0;
    }

    private void Start()
    {
        if (DataManager_J.instance.gameData.Day.Equals(1))
            SoundManager.Instance.PlayBGM(BGMTrackName.Tutorial);
        else
            SoundManager.Instance.PlayBGM(BGMTrackName.Lunch);

        dogs = FindObjectsOfType<LunchDog>();
        foodBowls = FindObjectsOfType<FoodBowl>();

        for (int i = 0; i < dogs.Length; i++)
        {
            dogs[i].SetLunchFood(foodBowls[i]);
            foodBowls[i].SetDog(dogs[i]);
        }

        // 3. 점심 자동 시작 (딜레이 후)
        StartCoroutine(StartLunchDelayed());
    }

    private IEnumerator StartLunchDelayed()
    {
        yield return new WaitUntil(()=>bell.ringged); // 대기 후 시작

        // 강아지들 점심 시작
        foreach (var dog in dogs)
        {
            dog.StartLunch();
        }
    }

    public void OnDogFinished()
    {
        finishCount++;
        Debug.Log($"식사 완료 수 : {finishCount}/3");
        if (finishCount >= 3)
        {
            Debug.Log("모든 강아지 식사 완료 → 외부 씬 전환");
            StartCoroutine(LoadOutdoorScene());
        }
    }

    private IEnumerator LoadOutdoorScene()
    {
        yield return new WaitForSeconds(1f);

        GameManager.instance.GoToScene("H_Outdoor");
    }
}
