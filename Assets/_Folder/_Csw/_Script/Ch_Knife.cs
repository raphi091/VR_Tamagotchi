using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using EzySlice;

public class Ch_Knife : XRGrabInteractable
{
    private List<Transform> slice_objs;
    private Transform sliceQuad;
    private Vector3 previous_pos;
    
    public Material cross_m;
    public float cutForce = 50f;
    public LayerMask layer;
    
    [Header("Knife")]
    [SerializeField] private Transform handPoint;
    [SerializeField] Ch_Blade blade;
    [SerializeField] Ch_CuttedFood cuttedFood;

    protected override void OnEnable()
    {
        blade.onSliceHit.AddListener(OnSlicehit);
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        blade.onSliceHit.RemoveListener(OnSlicehit);
        base.OnDisable();
    }

    private void Start()
     {
         this.attachTransform = handPoint;
         StartCoroutine(Update_Co());
     }
     
     IEnumerator Update_Co()
     {
         while (true)
         {
             RaycastHit hit;
             if (Physics.Raycast(transform.position, transform.forward, out hit, layer))
             {
                 if (Vector3.Angle(transform.position - previous_pos, hit.transform.up) >= 130f&&sliceQuad != null)
                 {
                     Slice_Objects(hit.transform.gameObject, sliceQuad);
                 }
             }
             previous_pos = transform.position;
             yield return null;
         }
     }
     
     void Slice_Objects(GameObject target, Transform quad)
     {
         if (target.TryGetComponent(out Ch_TreatFood c))
         {
             slice_objs = c.slices;
     
             Vector3 slice_normal = Vector3.Cross(transform.position - previous_pos, transform.forward);
     
             SlicedHull hull = target.Slice(quad.position, slice_normal);//자르기
     
             if (hull != null)
             {
                 GameObject upperhull = hull.CreateUpperHull(target, cross_m);
                 GameObject lowerhull = hull.CreateLowerHull(target, cross_m);
                 int indexOfSlice=slice_objs.IndexOf(quad);
                 for (int i = 0; i < slice_objs.Count; i++)
                 {
                     if (i < indexOfSlice)
                     {
                         slice_objs[i].SetParent(upperhull.transform);
                     }
                     else if(i==indexOfSlice)
                     {
                         
                     }
                     else
                     {
                         slice_objs[i].SetParent(lowerhull.transform);
                     }
                 }
                 Setup_Slice_components(upperhull);
                 Setup_Slice_components(lowerhull);
                 Destroy(target);
             }
         }
     }
     
     private void Setup_Slice_components(GameObject g)
     {
         Rigidbody rb = g.AddComponent<Rigidbody>();
         MeshCollider c = g.AddComponent<MeshCollider>();
         c.convex = true;
         Ch_TreatFood t =g.AddComponent<Ch_TreatFood>();
         rb.AddExplosionForce(cutForce, g.transform.position, 1);
         t.cuttedFood=cuttedFood;
         t.OnCutted();
     }

     public void OnSlicehit(Transform hit)
     {
         sliceQuad = hit;
     }
}
