using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Ch_CannedFood : MonoBehaviour,  Ch_BowlFood
{
    public GameObject gameObj => gameObject;
    public bool isFillable { get; set; }
    private foodType foodType;
    public virtual foodType FoodType { get => foodType; set => foodType = value; }
    
    private XRGrabInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<XRGrabInteractable>();
        interactable.enabled = false;
    }
}
