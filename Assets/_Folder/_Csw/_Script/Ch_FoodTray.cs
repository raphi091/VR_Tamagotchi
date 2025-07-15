using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch_FoodTray : MonoBehaviour
{
    [SerializeField] private FoodTrayType trayType;
    [SerializeField] private Transform food;
    void Awake()
    {
        if (trayType == FoodTrayType.Dry || trayType == FoodTrayType.Wet)
        {
            food.localPosition = Vector3.down;
        }
        else if (trayType == FoodTrayType.Raw)
        {
            food.gameObject.SetActive(false);
        }
    }

    public void OnEnable()
    {
        
    }
}
