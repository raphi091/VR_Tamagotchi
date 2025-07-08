using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;


public class UIButton : MonoBehaviour
{
    public Color pressedcolor = Color.blue;
    private Color originalColor;
    private MeshRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        originalColor = renderer.material.color;
    }
    public void Onpress()
    {
        renderer.material.color = pressedcolor;
        Debug.Log("click");

    }
    public void OnRelease()
    {
        renderer.material.color = originalColor;
    }

}
