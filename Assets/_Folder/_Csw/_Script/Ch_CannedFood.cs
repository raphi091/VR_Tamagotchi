using System;
using System.Net;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Ch_CannedFood : MonoBehaviour,  Ch_BowlFood
{
    public GameObject gameObj => gameObject;
    public bool isFillable { get; set; }
    private foodType foodType;
    public virtual foodType FoodType { get => foodType; set => foodType = value; }

    public Transform endPoint;
    
    private XRGrabInteractable interactable;

    void Awake()
    {
        foodType = foodType.Wet;
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<XRGrabInteractable>().enabled = false;
        isFillable = false;
    }

    public void OnOut()
    {
        transform.DOKill();
        transform.SetParent(null);
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<XRGrabInteractable>().enabled = true;
        isFillable = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tray")&&isFillable)
        {
            collision.gameObject.TryGetComponent(out FoodBowl bowl);
            Ch_FoodEvent.I.InvokeFoodBowlAction(bowl, this);
        }
    }
}
