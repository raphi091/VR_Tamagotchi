using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LunchSceneManager : MonoBehaviour
{
    public static LunchSceneManager instance = null;
    public Ch_Bell bell;

    public List<PetController_J> dogs;
    public List<LunchDog> lunchdogs;
    public FoodBowl[] foodBowls;
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

        for (int i = 0; i < GameManager.instance.petsInScene.Count; i++)
        {
            dogs.Add(GameManager.instance.petsInScene[i]);
            lunchdogs.Add(dogs[i].GetComponent<LunchDog>());
        }
        foodBowls = FindObjectsOfType<FoodBowl>();

        SetUp();
        
        // 3. 점심 자동 시작 (딜레이 후)
        StartCoroutine(StartLunchDelayed());
    }

    public void SetUp()
    {
        for (int i = 0; i < GameManager.instance.petsInScene.Count; i++)
        {
            lunchdogs[i].SetLunchFood(foodBowls[i]);
            foodBowls[i].SetDog(lunchdogs[i]);

            lunchdogs[i].PetController.namePlate = foodBowls[i].namePlate;
            lunchdogs[i].PetController.namePlate.Setup(dogs[i].petData.petName, DataManager_J.instance.gameData.selectedClassName);
        }
    }

    private IEnumerator StartLunchDelayed()
    {
        yield return new WaitUntil(()=>bell.ringged); // 대기 후 시작

        // 강아지들 점심 시작
        foreach (var dog in lunchdogs)
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
