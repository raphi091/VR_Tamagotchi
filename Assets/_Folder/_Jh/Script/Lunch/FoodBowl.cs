using System;
using UnityEngine;

public class FoodBowl : MonoBehaviour
{
    public LunchDog assignedDog;

     // 현재 그릇에 담긴 사료 종류
    public foodType containedFood = foodType.None;

    // 실제 배치되는 사료 모델
    private GameObject currentBowlInstance;

    // 사료가 생성될 위치 (빈 GameObject를 transform으로 설정)
    private GameObject currentBowl;

    [HideInInspector] public bool IsFilled = false;

    [SerializeField] Transform dryfoodModel;
    [SerializeField] private float dryfoodUp;
    [SerializeField] private float foodThreshold = 0f;
    
    
    private void OnEnable()
    {
        if (Ch_FoodEvent.I == null)
        {
            Debug.Log("FoodEvent Null");
        }
        else
        {
            Debug.Log("FoodEvent Found");
        }

        Ch_FoodEvent.I.AddFoodBowlEvent(this, FillBowl);
    }

    private void OnDisable()
    {
        Ch_FoodEvent.I.RemoveFoodBowlEvent(this, FillBowl);
    }
    
    public void FillByPlayer(foodType selected)
    {
        IsFilled = true;
        containedFood = selected;
        // 나중에 UI나 VR 상호작용에서 이 함수 호출
    }

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

    private void OnParticleCollision(GameObject other)
    {
        Ch_BowlFood food=other.GetComponentInParent<Ch_BowlFood>();
        if (food != null && food.FoodType==foodType.Dry && !IsFilled)
        {
            FillBowl(food);
        }
    }

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
