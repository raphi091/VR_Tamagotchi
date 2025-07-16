using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ch_FoodEvent : Ch_BehaviourSingleton<Ch_FoodEvent>
{
    protected override bool IsDontdestroy()
    {
        return true;
    }
    
    private readonly IDictionary<foodType,UnityEvent> FoodTrayActions = new Dictionary<foodType, UnityEvent>();

    public void AddFoodTrayEvent(foodType foodType, UnityAction action)
    {
        UnityEvent foodTrayAction;

        if (FoodTrayActions.TryGetValue(foodType, out foodTrayAction))
        {
            foodTrayAction.AddListener(action);
        }
        else
        {
            foodTrayAction = new UnityEvent();
            foodTrayAction.AddListener(action);
            FoodTrayActions.Add(foodType, foodTrayAction);
        }
    }

    public void RemoveFoodTrayEvent(foodType foodType, UnityAction action)
    {
        UnityEvent foodTrayAction;
        if (FoodTrayActions.TryGetValue(foodType, out foodTrayAction))
        {
            foodTrayAction.RemoveListener(action);
        }
    }

    public void InvokeFoodTrayAction(foodType foodType)
    {
        UnityEvent foodTrayAction;

        if (FoodTrayActions.TryGetValue(foodType, out foodTrayAction))
        {
            Debug.Log(foodTrayAction);
            foodTrayAction.Invoke();
        }
    }
}
