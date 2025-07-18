using UnityEngine;

public interface Ch_BowlFood
{
    public GameObject gameObj { get; }
    public bool isFillable { get; set; }
    public foodType FoodType { get; set;}
}
