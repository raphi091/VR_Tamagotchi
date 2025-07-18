using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Ch_CuttedFood : XRGrabInteractable, Ch_BowlFood
{
    public bool isFillable { get; set; }
    private foodType foodtype;
    public virtual foodType FoodType { get => foodtype; set => foodtype = value; }
    [SerializeField] private Transform[] offset;
    private int index=0;

    void Awake()
    {
        offset=GetComponentsInChildren<Transform>();
        foodtype = foodType.Treat;
        isFillable = false;
    }
    
    void OnCutted(Ch_TreatFood cutted)
    {
        cutted.transform.rotation = Quaternion.identity;
        cutted.transform.position = offset[index].position;
        cutted.transform.SetParent(transform);
        index++;
        this.colliders.Add(cutted.GetComponent<Collider>());
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
