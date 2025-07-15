using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ch_FoodEvent : Ch_BehaviourSingleton<Ch_FoodEvent>
{
    protected override bool IsDontdestroy()
    {
        return true;
    }
    
    public readonly IDictionary<FoodTrayType,UnityAction> FoodTrayActions = new Dictionary<FoodTrayType, UnityAction>();
}
