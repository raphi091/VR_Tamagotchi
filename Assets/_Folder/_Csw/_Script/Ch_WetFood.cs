using UnityEngine;

public class Ch_WetFood : MonoBehaviour
{
    private foodType foodtype;
    public foodType FoodTrayType { get => foodtype; set => foodtype = value; }
    private bool isOpened = false;
    

    [Range(0f, -1f), SerializeField] private float upsideDownRange=-0.7f;
    [SerializeField] private Transform startPoint;

    void Awake()
    {
        foodtype = foodType.Wet;
    }
    
    private void Update()
    {
        if (!isOpened)
        {
            
        }
        
        if (transform.up.y < upsideDownRange&&isOpened)
        {
                
        }
    }
}
