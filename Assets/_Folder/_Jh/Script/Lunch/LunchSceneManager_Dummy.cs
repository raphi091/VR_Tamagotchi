using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LunchSceneManager_Dummy : MonoBehaviour
{
    public List<LunchDogDummy> dogCubes;
    public List<FoodBowl> foodBowls;

    public AudioSource bellSource;  // ✅ 종소리 AudioSource
    public AudioClip bellClip;              // ✅ 종소리 클립

    private static int finishCount = 0;
    private static LunchSceneManager_Dummy instance;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        // 1. 모든 밥그릇 랜덤 음식 채우기 (테스트용)
        foreach (var bowl in foodBowls)
        {
            bowl.FillRandom(); // ⚠️ 테스트용
        }

        // 2. 각 큐브에게 오늘의 음식과 데이터 설정
        for (int i = 0; i < dogCubes.Count; i++)
        {
            var dog = dogCubes[i];
            dog.SetLunchFood(foodBowls[i].containedFood);

            // ✅ 실제 GameData에 있는 저장된 펫 데이터 사용
            var data = DataManager_J.instance.gameData.allPetData[i];
            dog.SetPetData(data);

            dog.InitPositionToWait();
        }

        // 3. 테스트용 자동 실행
        StartCoroutine(StartLunchDelayed());
    }

    IEnumerator StartLunchDelayed()
    {
        yield return new WaitForSeconds(3f); // 잠시 대기

        // ✅ 종소리 재생
        if (bellSource != null && bellClip != null)
        {
            Debug.Log($"벨소리");
            bellSource.PlayOneShot(bellClip);
        }

        foreach (var dog in dogCubes)
            dog.StartLunch();
    }

    public static void OnDogFinished()
    {
        finishCount++;
        if (finishCount >= 3)
        {
            Debug.Log("모든 강아지 식사 완료 → 씬 전환 등 처리");

            // ✅ 1초 후 외부 씬으로 전환
            instance.StartCoroutine(instance.LoadOutdoorScene());
        }
    }

    private IEnumerator LoadOutdoorScene()
    {
        yield return new WaitForSeconds(1f); // 약간의 여유 시간
        Debug.Log("모든 강아지 식사 완료 -> H_Outdoor로 이동");
        // ✅ GameManager 통해 외부 씬으로 전환
        GameManager.instance.GoToScene("H_Outdoor");
    }
}
