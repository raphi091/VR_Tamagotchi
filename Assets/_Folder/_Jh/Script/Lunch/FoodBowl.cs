using System;
using UnityEngine;

public class FoodBowl : MonoBehaviour
{
    public LunchDog assignedDog;
    public NamePlate_K namePlate;
    public Transform WaitPoint;

     // 현재 그릇에 담긴 사료 종류
    public foodType containedFood = foodType.None;

    // 실제 배치되는 사료 모델
    private GameObject currentBowlInstance;

    // 사료가 생성될 위치 (빈 GameObject를 transform으로 설정)
    private GameObject currentBowl;

    private Vector3 initPos;
    private AudioSource audioSource;

    [HideInInspector] public bool IsFilled = false;

    [SerializeField] Transform dryfoodModel;
    [SerializeField] private float dryfoodUp;
    [SerializeField] private float foodThreshold = 0f;
    [SerializeField] private Transform foodPoint;

    private void Awake()
    {
        audioSource=GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        initPos = dryfoodModel.position;
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
    
    public void FillBowl(Ch_BowlFood food)
    {
        if (IsFilled)
        {
            return;
        }

        if (food.FoodType!=foodType.Dry && containedFood == foodType.Dry && !IsFilled)
        {
            dryfoodModel.position = initPos;
        }
        
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
        else if(containedFood==foodType.Wet && food.isFillable)
        {
            currentBowlInstance = food.gameObj;
            Collider c = currentBowlInstance.GetComponent<Collider>();
            Rigidbody r = currentBowlInstance.GetComponent<Rigidbody>();
            c.enabled = false;
            r.isKinematic = true;
            currentBowlInstance.transform.position = foodPoint.position;
            currentBowlInstance.transform.rotation = Quaternion.Euler(new Vector3(90f, 0, 0));
            IsFilled=true;
        }
        else if(food.isFillable)
        {
            currentBowlInstance = food.gameObj;
            Collider c = currentBowlInstance.GetComponent<Collider>();
            Rigidbody r = currentBowlInstance.GetComponent<Rigidbody>();
            c.enabled = false;
            r.isKinematic = true;
            currentBowlInstance.transform.position = foodPoint.position;
            IsFilled=true;
        }

        if (!audioSource.isPlaying)
        {
            audioSource.clip = food.FoodSound;
            audioSource.Play();
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
        currentBowlInstance?.SetActive(false);
        IsFilled = false;
    }

    public void SetDog(LunchDog dog)
    {
        assignedDog = dog;
    }
    
}
