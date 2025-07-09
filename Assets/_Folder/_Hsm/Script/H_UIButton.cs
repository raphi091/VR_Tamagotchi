using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;


public class H_UIButton : MonoBehaviour
{
    public Color pressedcolor = Color.blue;
    public Color originalColor;
    public MeshRenderer meshR;

    private void Start()
    {
        originalColor = meshR.material.color;
        Debug.Log("0");
    }

    public void OnPress()
    {
        meshR.material.color = pressedcolor;
        Debug.Log("click");

    }

    public void OnRelease()
    {
        meshR.material.color = originalColor;
    }

}
