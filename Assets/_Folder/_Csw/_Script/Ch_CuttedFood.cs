using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Ch_CuttedFood : XRGrabInteractable
{
    [SerializeField] private Transform offset;

    void Update()
    {
        
    }
    void OnCutted(GameObject cutted)
    {
        Vector3 newPosition = Random.insideUnitSphere;
        newPosition.y=offset.localPosition.y;
        cutted.transform.position = newPosition;
        this.colliders.Add(cutted.GetComponent<Collider>());
    }
}
