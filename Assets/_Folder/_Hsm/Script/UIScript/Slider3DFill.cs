using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slider3DFill : MonoBehaviour
{
    [Header("slider")]
    public Slider slider;

    [Header("3D Object")]
    public Transform fillObject;

    [Header("fill Height")]
    public float minlScaleY = 0.01f;
    public float maxScaleY = 1.8f;
    //public float originalScaleY = 0.2f;

    public Vector3 basePosition;
    public Vector3 topPosition;


  

    private void Update()
    {
        if (slider == null || fillObject == null) return;
        
        float t = slider.value;

        fillObject.position = Vector3.Lerp(basePosition, topPosition, t);

        Vector3 scale = fillObject.localScale;
        scale.y = Mathf.Lerp(minlScaleY, maxScaleY, t);
        fillObject.localScale = scale;

          
        
    }

}

