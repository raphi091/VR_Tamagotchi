using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ch_FoodEvent : MonoBehaviour
{
    public static Ch_FoodEvent I;
    
    private readonly IDictionary<FoodBowl,UnityEvent<Ch_BowlFood>> FoodBowlActions = new Dictionary<FoodBowl, UnityEvent<Ch_BowlFood>>();


    void Awake()
    {
        I = this;
    }
    
    public void AddFoodBowlEvent(FoodBowl bowl, UnityAction<Ch_BowlFood> action)
    {
        UnityEvent<Ch_BowlFood> foodBowlAction;

        if (FoodBowlActions.TryGetValue(bowl, out foodBowlAction))
        {
            foodBowlAction.AddListener(action);
        }
        else
        {
            foodBowlAction = new UnityEvent<Ch_BowlFood>();
            foodBowlAction.AddListener(action);
            FoodBowlActions.Add(bowl, foodBowlAction);
        }
    }

    public void RemoveFoodBowlEvent(FoodBowl bowl, UnityAction<Ch_BowlFood> action)
    {
        UnityEvent<Ch_BowlFood> foodBowlAction;
        if (FoodBowlActions.TryGetValue(bowl, out foodBowlAction))
        {
            foodBowlAction.RemoveListener(action);
        }
    }

    public void InvokeFoodBowlAction(FoodBowl bowl, Ch_BowlFood food)
    {
        UnityEvent<Ch_BowlFood> foodBowlAction;

        if (FoodBowlActions.TryGetValue(bowl, out foodBowlAction))
        {
            foodBowlAction.Invoke(food);
        }
    }
}
