using UnityEngine;

public class FoodBowl : MonoBehaviour
{

    public LunchDog assignedDog;
     // ✅ 현재 그릇에 담긴 사료 종류
    public foodType containedFood=foodType.None;

    // ✅ 실제 배치되는 사료 모델
    private GameObject currentBowlInstance;

    // ✅ 사료가 생성될 위치 (빈 GameObject를 transform으로 설정)

    private GameObject currentBowl;

    [HideInInspector] public bool IsFilled = false;

    [SerializeField] Transform dryfoodModel;
    [SerializeField] private float dryfoodUp;
    [SerializeField] private float foodThreshold = 0f;
    
    void OnEnable()
    {
        Ch_FoodEvent.I.AddFoodBowlEvent(this, FillBowl);
    }
    
    public void FillByPlayer(foodType selected)
    {
        IsFilled = true;
        containedFood = selected;
        // 나중에 UI나 VR 상호작용에서 이 함수 호출
    }

    /* 위에까지 테스트용도
     아래부터는 수정용도 */
    /// <summary>
    /// ⚠️ [2] 실제 플레이에서는 → 플레이어가 직접 이 메서드를 호출
    /// ex: 플레이어가 사료 아이템을 선택하여 넣을 경우
    /// </summary>
    public void FillBowl(Ch_BowlFood food)
    {
        
        containedFood = food.FoodType;
        if (containedFood == foodType.Dry)
        {
            currentBowlInstance = dryfoodModel.gameObject;
            Vector3 a= dryfoodModel.position;
            a.y+=dryfoodUp*Time.deltaTime;
            dryfoodModel.position = a;
            if (a.y >= foodThreshold)
            {
                IsFilled = true;
            }
        }
        else if(food.isFillable)
        {
            currentBowlInstance = food.gameObj;
            IsFilled=true;
        }
    }

    /// <summary>
    /// ✅ 식사 완료 후 빈 그릇으로 변경
    /// </summary>
    public void ClearBowl()
    {
        currentBowlInstance.SetActive(false);
        IsFilled = false;
    }

    public void SetDog(LunchDog dog)
    {
        assignedDog = dog;
    }
    
}
