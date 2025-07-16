using System.Collections;
using UnityEngine;

public class LunchDogDummy : MonoBehaviour
{
    [Header("위치 지정")]
    public Transform waitPosition;
    public Transform bowlPosition;

    [Header("모델링 프리팹")]
    public GameObject[] dogModelPrefabs; // 모델링 프리팹들 (modelIndex 기준)
    private GameObject currentModelInstance;

    [Header("임시 해결책")]
    public bool useBasicShape = true; // 임시로 기본 도형 사용
    public Material[] dogMaterials; // 강아지별 색상 구분용

    [Header("텍스트 프리팹")]
    public GameObject goodTextPrefab;
    public GameObject badTextPrefab;

    [Header("식사 설정")]
    public float eatingTime = 3f;

    [Header("데이터")]
    public PetStatusData_J petData;
    private foodType lunchFoodType;

    [Header("디버깅")]
    public bool debugMode = true;

    public void SetLunchFood(foodType type)
    {
        lunchFoodType = type;
        if (debugMode)
            Debug.Log($"[LunchDogDummy] 음식 타입 설정: {type}");
    }

    public void SetPetData(PetStatusData_J data)
    {
        if (data == null)
        {
            Debug.LogError($"[LunchDogDummy] Pet 데이터가 null입니다! 오브젝트: {name}");
            return;
        }

        petData = data;

        if (debugMode)
        {
            Debug.Log($"[LunchDogDummy] Pet 데이터 설정됨:");
            Debug.Log($"  - Pet 이름: {data.petName}");
            Debug.Log($"  - Model Index: {data.modelIndex}");
            Debug.Log($"  - Food Type: {data.foodType}");
        }

        LoadDogModel();
    }

    private void LoadDogModel()
    {
        // 기존 모델 제거
        if (currentModelInstance != null)
        {
            Destroy(currentModelInstance);
        }

        if (useBasicShape)
        {
            // 임시 해결책: 기본 Cube 사용
            CreateBasicShapeModel();
        }
        else
        {
            // 원래 방식: 모델 프리팹 사용
            CreateModelFromPrefab();
        }
    }

    private void CreateBasicShapeModel()
    {
        // 기본 Cube 생성
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.SetParent(transform);
        cube.transform.localPosition = Vector3.zero;
        cube.transform.localRotation = Quaternion.identity;
        cube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        // 강아지별 색상 구분
        if (dogMaterials != null && petData.modelIndex < dogMaterials.Length)
        {
            Renderer renderer = cube.GetComponent<Renderer>();
            if (renderer != null && dogMaterials[petData.modelIndex] != null)
            {
                renderer.material = dogMaterials[petData.modelIndex];
            }
        }

        

        currentModelInstance = cube;

        if (debugMode)
        {
            Debug.Log($"[LunchDogDummy] 기본 도형 모델 생성 완료: {petData.petName}");
        }
    }

    private void CreateModelFromPrefab()
    {
        if (dogModelPrefabs == null || petData.modelIndex >= dogModelPrefabs.Length)
        {
            Debug.LogError($"[LunchDogDummy] 잘못된 modelIndex: {petData.modelIndex}");
            return;
        }

        if (dogModelPrefabs[petData.modelIndex] == null)
        {
            Debug.LogError($"[LunchDogDummy] dogModelPrefabs[{petData.modelIndex}]가 null입니다!");
            return;
        }

        try
        {
            currentModelInstance = Instantiate(dogModelPrefabs[petData.modelIndex], transform);
            currentModelInstance.transform.localPosition = Vector3.zero;
            currentModelInstance.transform.localRotation = Quaternion.identity;
            currentModelInstance.transform.localScale = Vector3.one;

            // 모델이 올바르게 보이는지 확인
            CheckModelVisibility();

            if (debugMode)
            {
                Debug.Log($"[LunchDogDummy] 모델 프리팹 생성 완료: {currentModelInstance.name}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[LunchDogDummy] 모델 생성 중 오류: {e.Message}");
            // 실패 시 기본 도형으로 대체
            CreateBasicShapeModel();
        }
    }

    private void CheckModelVisibility()
    {
        if (currentModelInstance == null) return;

        Renderer[] renderers = currentModelInstance.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            Debug.LogWarning($"[LunchDogDummy] 모델에 Renderer가 없습니다!");
            return;
        }

        foreach (Renderer renderer in renderers)
        {
            if (!renderer.enabled)
            {
                Debug.LogWarning($"[LunchDogDummy] Renderer가 비활성화되어 있습니다: {renderer.name}");
                renderer.enabled = true;
            }

            if (renderer.material == null)
            {
                Debug.LogWarning($"[LunchDogDummy] Material이 없습니다: {renderer.name}");
                // 기본 Material 할당
                renderer.material = new Material(Shader.Find("Standard"));
            }
        }
    }

    public void InitPositionToWait()
    {
        if (waitPosition != null)
        {
            transform.position = waitPosition.position;
            if (debugMode)
                Debug.Log($"[LunchDogDummy] 대기 위치로 이동: {waitPosition.position}");
        }
    }

    public void StartLunch()
    {
        if (debugMode)
            Debug.Log($"[LunchDogDummy] 점심 시작! - {petData.petName}");
        StartCoroutine(LunchRoutine());
    }

    private IEnumerator LunchRoutine()
    {
        // 1. 대기 위치에서 시작
        transform.position = waitPosition.position;
        gameObject.SetActive(true);

        // 2. 밥그릇 앞으로 이동
        float speed = 1.5f;
        while (Vector3.Distance(transform.position, bowlPosition.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, bowlPosition.position, Time.deltaTime * speed);
            yield return null;
        }

        // 3. 먹는 시간 대기
        yield return new WaitForSeconds(eatingTime);

        // 4. 음식 비교 후 텍스트 생성
        bool isGood = petData.foodType == lunchFoodType;
        GameObject textPrefab = isGood ? goodTextPrefab : badTextPrefab;

        if (textPrefab != null)
        {
            Vector3 spawnPos = transform.position + new Vector3(0f, 0.8f, 0f);
            GameObject text = Instantiate(textPrefab, spawnPos, Quaternion.identity);
            text.AddComponent<BillboardFaceCamera>();
            Destroy(text, 1.5f);
        }

        // 5. 식사 완료 → 허기 회복
        petData.hungerper = 100f;

        // 6. 식사 완료 알림
        LunchSceneManager_Dummy.OnDogFinished();

        if (debugMode)
            Debug.Log($"[LunchDogDummy] 점심 루틴 완료! - {petData.petName}");
    }

    void OnDrawGizmosSelected()
    {
        if (waitPosition != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(waitPosition.position, 0.2f);
        }

        if (bowlPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(bowlPosition.position, 0.2f);
        }
    }
}