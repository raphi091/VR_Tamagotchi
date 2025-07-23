using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Ch_CuttedFood : MonoBehaviour, Ch_BowlFood
{
    public bool isFillable { get; set; }
    private foodType foodtype;
    public virtual foodType FoodType { get => foodtype; set => foodtype = value; }
    public virtual GameObject gameObj => gameObject;
    [SerializeField] private Transform[] offset;
    private int index=0;
    [SerializeField] private int fillableCount=4;
    private XRGrabInteractable interactable;

    void Awake()
    {
        offset=GetComponentsInChildren<Transform>().Where(t=>t!=this.transform).ToArray();
        interactable = GetComponent<XRGrabInteractable>();
        fillableCount=offset.Length;
        interactable.enabled = false;
        foodtype = foodType.Treat;
        isFillable = false;
    }
    
    public void OnCutted(Ch_TreatFood cutted)
    {
        if (index < fillableCount)
        {
            cutted.transform.rotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
            cutted.transform.position = offset[index].position;
            cutted.transform.SetParent(offset[index]);
            index++;
            interactable.colliders.Add(cutted.GetComponent<Collider>());
            Rigidbody a=cutted.GetComponent<Rigidbody>();
            a.isKinematic = true;
            a.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            if (index >= fillableCount)
            {
                this.GetComponent<MeshRenderer>().enabled = false;
                isFillable = true;
                interactable.enabled = true;
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
