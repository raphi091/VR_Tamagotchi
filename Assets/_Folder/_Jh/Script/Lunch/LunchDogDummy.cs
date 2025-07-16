using System.Collections;
using UnityEngine;

public class LunchDogDummy : MonoBehaviour
{
    public Transform waitPosition;
    public Transform bowlPosition;
    public GameObject goodTextPrefab;
    public GameObject badTextPrefab;
    public float eatingTime = 3f;

    public PetStatusData_J petData; // 이 강아지의 데이터
    private foodType lunchFoodType; // 오늘 제공된 음식

    public static int finishedCount = 0;

    public void SetLunchFood(foodType type)
    {
        lunchFoodType = type;
    }

    public void SetPetData(PetStatusData_J data)
    {
        petData = data;
    }

    public void InitPositionToWait()
    {
        if(waitPosition != null)
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
        // ✅ 0. 대기 위치에서 시작
        transform.position = waitPosition.position;
        gameObject.SetActive(true);

        // (이 타이밍에서 종소리 재생 → LunchSceneManager에서 처리)

        // ✅ 2. 밥그릇 '앞쪽' 위치로 이동
        float speed = 1.5f;
        while (Vector3.Distance(transform.position, bowlPosition.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, bowlPosition.position, Time.deltaTime * speed);
            yield return null;
        }

        // ✅ 3. 먹는 시간 대기
        yield return new WaitForSeconds(eatingTime);

        // ✅ 4. 음식 비교 → Good or Bad 텍스트 생성
        bool isGood = petData.foodType == lunchFoodType;
        GameObject textPrefab = isGood ? goodTextPrefab : badTextPrefab;

        if (textPrefab != null)
        {
            // ✅ 텍스트는 강아지 머리 위 살짝 위에서 생성 (위치 조정)
            Vector3 spawnPos = transform.position + new Vector3(0f, 0.5f, 0f); // ← 너무 높지 않게 조절

            // ✅ 텍스트 생성
            GameObject text = Instantiate(textPrefab, spawnPos, Quaternion.identity);

            // ✅ 카메라 바라보도록 Billboard 스크립트 붙이기
            text.AddComponent<BillboardFaceCamera>();

            // ✅ 1.5초 뒤에 사라지도록
            Destroy(text, 1.5f);

        }

        // ✅ 5. 식사 완료 후 허기 수치 회복 (항상 100으로 설정)
        // ⚠️ 점심식사 후 hungerper은 항상 최대치로 회복됨
        petData.hungerper = 100f;

        LunchSceneManager_Dummy.OnDogFinished(); // 정적 함수든 인스턴스든 카운트 처리
    }
}

