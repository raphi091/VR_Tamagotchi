using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Ch_CuttedFood : XRGrabInteractable, Ch_BowlFood
{
    public bool isFillable { get; set; }
    private foodType foodtype;
    public virtual foodType FoodType { get => foodtype; set => foodtype = value; }
    public virtual GameObject gameObj => gameObject;
    [SerializeField] private Transform[] offset;
    private int index=0;
    private int fillableCount;

    void Awake()
    {
        offset=GetComponentsInChildren<Transform>();
        fillableCount=offset.Length;
        foodtype = foodType.Treat;
        isFillable = false;
    }
    
    void OnCutted(Ch_TreatFood cutted)
    {
        if (index < fillableCount)
        {
            cutted.transform.rotation = Quaternion.identity;
            cutted.transform.position = offset[index].position;
            cutted.transform.SetParent(transform);
            index++;
            this.colliders.Add(cutted.GetComponent<Collider>());
            if (index >= fillableCount)
            {
                isFillable = true;
            }
        }
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
