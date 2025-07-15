using UnityEngine;

public class FoodBowl : MonoBehaviour
{
    /*
     수정용도 프리팹이 다 있을시에 수정
    public GameObject bowlDryPrefab;    // 건식 사료
    public GameObject bowlWetPrefab;    // 습식 사료
    public GameObject bowlTreatPrefab;  // 생식
    public GameObject bowlEmptyPrefab;  // 빈 그릇
     
     // ✅ 현재 그릇에 담긴 사료 종류
    public foodType containedFood;

    // ✅ 실제 배치되는 사료 모델
    private GameObject currentBowlInstance;

    // ✅ 사료가 생성될 위치 (빈 GameObject를 transform으로 설정)
    public Transform bowlSpawnPoint;
     
     */

    public GameObject emptyBowlPrefab;
    public GameObject filledBowlPrefab;

    private GameObject currentBowl;

    public bool IsFilled = false;
    public foodType containedFood; // 실제 담긴 음식 종류

    /// <summary>
    /// ⚠️ [1] 테스트용 자동 사료 채우기 (Dry/Wet 중 랜덤)
    /// 추후 → 플레이어가 사료를 넣는 로직으로 교체 예정
    /// </summary>
    public void FillRandom()
    {
        IsFilled = true;

        // ⚠️ 지금은 테스트용으로 랜덤 음식 할당
        containedFood = (foodType)Random.Range(0, 3);

        // TODO: 나중에 플레이어가 직접 음식 선택해서 넣게 바꿔야 함
        Debug.Log($"{name} 그릇에 음식: {containedFood}");
    }

    // 추후 플레이어 상호작용용 함수
    public void FillByPlayer(foodType selected)
    {
        IsFilled = true;
        containedFood = selected;

        // 나중에 UI나 VR 상호작용에서 이 함수 호출
    }

    /* 위에까지 테스트용도
     아래부터는 수정용도
    /// <summary>
    /// ⚠️ [2] 실제 플레이에서는 → 플레이어가 직접 이 메서드를 호출
    /// ex: 플레이어가 사료 아이템을 선택하여 넣을 경우
    /// </summary>
    public void FillManual(foodType type)
    {
        containedFood = type;
        GameObject selectedPrefab = GetFoodPrefabByType(type);

        if (currentBowlInstance != null)
            Destroy(currentBowlInstance);

        currentBowlInstance = Instantiate(selectedPrefab, bowlSpawnPoint.position, Quaternion.identity, transform);
    }

    /// <summary>
    /// ✅ 식사 완료 후 빈 그릇으로 변경
    /// </summary>
    public void ClearBowl()
    {
        if (currentBowlInstance != null)
            Destroy(currentBowlInstance);

        currentBowlInstance = Instantiate(bowlEmptyPrefab, bowlSpawnPoint.position, Quaternion.identity, transform);
    }

    /// <summary>
    /// ✅ 사료 타입에 따른 프리팹 반환
    /// </summary>
    private GameObject GetFoodPrefabByType(foodType type)
    {
        switch (type)
        {
            case foodType.Dry: return bowlDryPrefab;
            case foodType.Wet: return bowlWetPrefab;
            case foodType.Treat: return bowlTreatPrefab;
            default: return bowlEmptyPrefab;
        }
    } */
}
