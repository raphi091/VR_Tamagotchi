using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class handleFollower : MonoBehaviour
{
    public Slider slider;
    public Transform handle3D;
    public Transform trackTop;
    public Transform trackBottom;

    public float moveRange = 2f;
    private void Update()
    {
        if(slider != null && handle3D != null)
        {
            float t = slider.value;
            Vector3 newPos = Vector3.Lerp(trackBottom.position, trackTop.position, t);
            handle3D.position = newPos;
        }
    }
}
