using UnityEngine;

public class FoodBowl : MonoBehaviour
{
    public bool IsFilled = false;

    public void Fill()
    {
        IsFilled = true;
        // 음식 비주얼이 있다면 활성화
    }
}
