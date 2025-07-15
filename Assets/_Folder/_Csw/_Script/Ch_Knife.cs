using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using EzySlice;

public class Ch_Knife : XRGrabInteractable
{
    private List<Transform> slice_objs;
    public Material cross_m;
    public float cutForce = 2000f;
    private GameObject blade;
    
    private Vector3 previous_pos;
    public LayerMask layer;
    
    [Header("Knife")]
    [SerializeField] private Transform handPoint;
//     private void Start()
//     {
//         this.attachTransform = handPoint;
//         StartCoroutine(Update_Co());
//     }
//     
//     IEnumerator Update_Co()
//     {
//         while (true)
//         {
//             RaycastHit hit;
//             if (Physics.Raycast(transform.position, transform.forward, out hit, layer))
//             {
//                 if (Vector3.Angle(transform.position - previous_pos, hit.transform.up) >= 130f)
//                 {
//                     Slice_Objects(hit.transform.gameObject);
//                 }
//             }
//             previous_pos = transform.position;
//             yield return null;
//         }
//     }
//     
//     void Slice_Objects(GameObject target)
//     {
//         if (target.TryGetComponent(out Ch_TreatFood c))
//         {
//             slice_objs = c.slices;
//     
//             Vector3 slice_normal = Vector3.Cross(transform.position - previous_pos, transform.forward);
//     
//             SlicedHull hull = target.Slice(slice_obj.position, slice_normal);//자르기
//     
//             if (hull != null)
//             {
//                 GameObject upperhull = hull.CreateUpperHull(target, cross_m);
//                 GameObject lowerhull = hull.CreateLowerHull(target, cross_m);
//     
//                 if (target.transform.childCount > 0)
//                 {
//                     for (int i = 0; i < transform.childCount; i++)
//                     {
//                         GameObject g = target.transform.GetChild(i).gameObject;
//     
//                         if (g.transform.Equals(slice_obj)) continue;
//     
//                         SlicedHull hull_c = g.Slice(slice_obj.position, slice_normal);
//     
//     
//                         if (hull_c != null)
//                         {
//                             GameObject upper_c = hull_c.CreateUpperHull(g, cross_m, upperhull);
//                             GameObject lower_c = hull_c.CreateLowerHull(g, cross_m, lowerhull);//자른 오브젝트 만들기
//                         }
//                     }
//                 }
//                 Destroy(target);
//                 Setup_Slice_components(upperhull);
//                 Setup_Slice_components(lowerhull);
//                 Destroy(upperhull, 1f);
//                 Destroy(lowerhull, 1f);
//             }
//         }
//     }
//     
//     private void Setup_Slice_components(GameObject g)
//     {
//         Rigidbody rb = g.AddComponent<Rigidbody>();
//         MeshCollider c = g.AddComponent<MeshCollider>();
//         c.convex = true;
//         rb.AddExplosionForce(cutForce, g.transform.position, 1);
//     }
}
