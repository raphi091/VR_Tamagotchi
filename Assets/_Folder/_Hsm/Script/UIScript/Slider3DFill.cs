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

    private void Update()
    {
        if (slider == null || fillObject == null) return;

        Vector3 scale = new Vector3(slider.value, fillObject.localScale.y, fillObject.localScale.z);
        fillObject.localScale = scale;
    }
}

