using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LunchSceneManager : MonoBehaviour
{
    public List<LunchDog> dogCubes;          // 점심용 강아지 프리팹들
    public List<FoodBowl> foodBowls;              // 밥그릇들
    public Ch_Bell bell;

    public AudioSource bellSource;                // 종소리 AudioSource
    public AudioClip bellClip;                    // 종소리 클립

    private static int finishCount = 0;
    private static LunchSceneManager instance;

    void Awake()
    {
        instance = this;
        finishCount = 0; // 씬 재시작 대비 초기화
    }

    void Start()
    {
        // 2. 강아지 데이터 및 위치 초기화
        for (int i = 0; i < dogCubes.Count; i++)
        {
            var dog = dogCubes[i];

            // ✅ 오늘 제공할 음식 전달
            dog.SetLunchFood(foodBowls[i]);
            foodBowls[i].SetDog(dog);

            // ✅ 실제 저장된 데이터 가져오기
            var data = DataManager_J.instance?.gameData?.allPetData;
            if (data == null || i >= data.Count)
            {
                Debug.LogError($"[LunchSceneManager] PetData가 없거나 인덱스 초과: {i}");
                continue;
            }
            dog.InitPositionToWait();
        }

        // 3. 점심 자동 시작 (딜레이 후)
        StartCoroutine(StartLunchDelayed());

        if (DataManager_J.instance.gameData.Day.Equals(1))
            SoundManager.Instance.PlayBGM(BGMTrackName.Tutorial);
        else
            SoundManager.Instance.PlayBGM(BGMTrackName.Lunch);
    }

    IEnumerator StartLunchDelayed()
    {
        yield return new WaitUntil(()=>bell.ringged); // 대기 후 시작

        // ✅ 종소리 재생
        if (bellSource != null && bellClip != null)
        {
            Debug.Log("벨소리 재생");
            bellSource.PlayOneShot(bellClip);
        }

        // ✅ 강아지들 점심 시작
        foreach (var dog in dogCubes)
        {
            dog.StartLunch();
        }
    }

    public static void OnDogFinished()
    {
        finishCount++;
        Debug.Log($"식사 완료 수 : {finishCount}/3");
        if (finishCount >= 3)
        {
            Debug.Log("모든 강아지 식사 완료 → 외부 씬 전환");
            instance.StartCoroutine(instance.LoadOutdoorScene());
        }
    }

    private IEnumerator LoadOutdoorScene()
    {
        yield return new WaitForSeconds(1f);
        GameManager.instance.GoToScene("H_Outdoor");
    }
}
