using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UInotice : MonoBehaviour, H_UI
{
    public GameObject view;


    private void Start()
    {
        view.SetActive(false);
    }

    public void OnPress()
    {
        view.SetActive(true);
    }

    public void OnRelease()
    {
    }
}
