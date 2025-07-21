using System.Collections;
using UnityEngine;
public class LunchDog : MonoBehaviour
{
    [Header("위치 지정")]
    public Transform waitPosition;
    public Transform bowlPosition;
    [Header("텍스트 프리팹")]
    public GameObject goodTextPrefab;
    public GameObject badTextPrefab;
    [Header("식사 설정")]
    public float eatingTime = 3f;
    private PetController_J petcontroller;
    private FoodBowl foodBowl;
    private foodType lunchFoodType;       // 오늘 제공된 음식
    [Header("디버깅")]
    public bool debugMode = true;

    private void Awake()
    {
        if (!TryGetComponent(out petcontroller))
            Debug.LogWarning("LunchDogDummy ] PetController_J 없음");
    }
    public void SetLunchFood(FoodBowl bowl)
    {
        foodBowl = bowl;
        lunchFoodType = bowl.containedFood;
    }
    public void InitPositionToWait()
    {
        if (waitPosition != null)
        {
            transform.position = waitPosition.position;
        }
    }
    public void StartLunch()
    {
        StartCoroutine(LunchRoutine());
    }
    private IEnumerator LunchRoutine()
    {
        // ✅ 1. 대기 위치에서 시작
        transform.position = waitPosition.position;
        gameObject.SetActive(true);

        // ✅ bowlPosition 검증
        if (bowlPosition.Equals(null))
        {
            Debug.LogError($"[LunchDogDummy] bowlPosition이 null입니다!");
            yield break;
        }

        if (debugMode)
            Debug.Log($"[LunchDogDummy] 점심 루틴 시작 - 현재 위치: {transform.position}, 목표: {bowlPosition.position}");

        // ✅ 2. 밥그릇 앞으로 이동 (개선된 버전)
        float speed = 1.5f;
        float timeout = 10f; // 10초 타임아웃
        float timer = 0f;
        float logTimer = 0f;

        while (Vector3.Distance(transform.position, bowlPosition.position) > 0.5f && timer < timeout)
        {
            transform.position = Vector3.MoveTowards(transform.position, bowlPosition.position, Time.deltaTime * speed);
            timer += Time.deltaTime;
            logTimer += Time.deltaTime;

            // 1초마다 진행상황 로그
            if (debugMode && logTimer >= 1f)
            {
                Debug.Log($"[LunchDogDummy] 이동 중... 거리: {Vector3.Distance(transform.position, bowlPosition.position)}, 경과시간: {timer:F1}s");
                logTimer = 0f;
            }

            yield return null;
        }

        // 타임아웃 검사
        if (timer >= timeout)
        {
            Debug.LogWarning($"[LunchDogDummy] 이동 타임아웃! 현재 거리: {Vector3.Distance(transform.position, bowlPosition.position)}");
            // 강제로 목표 위치로 이동
            transform.position = bowlPosition.position;
        }

        if (debugMode)
            Debug.Log($"[LunchDogDummy] 밥그릇 앞 도착: {transform.position}, 최종 거리: {Vector3.Distance(transform.position, bowlPosition.position)}");

        // ✅ 3. 먹는 시간 대기
        yield return new WaitForSeconds(eatingTime);
        foodBowl.ClearBowl();

        // ✅ 4. 음식 비교 후 텍스트 생성
        bool isGood = petcontroller.petData.foodType == lunchFoodType;
        GameObject textPrefab = isGood ? goodTextPrefab : badTextPrefab;

        if (debugMode)
            Debug.Log($"[LunchDogDummy] 음식 비교 - Pet: {petcontroller.petData.foodType}, Lunch: {lunchFoodType}, 결과: {isGood}");

        if (textPrefab != null)
        {
            Vector3 spawnPos = transform.position + new Vector3(0f, 0.5f, 0f);
            GameObject text = Instantiate(textPrefab, spawnPos, Quaternion.identity);
            text.AddComponent<BillboardFaceCamera>();
            Destroy(text, 1.5f);
        }

        // ✅ 5. 식사 완료 → 허기 회복
        petcontroller.petData.hungerper = 100f;

        // ✅ 6. 식사 완료 알림
        LunchSceneManager_Dummy.OnDogFinished();

        if (debugMode)
            Debug.Log($"[LunchDogDummy] 점심 루틴 완료!");
    }
}