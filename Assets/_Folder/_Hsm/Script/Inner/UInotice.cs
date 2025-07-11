using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UInotice : MonoBehaviour, H_UI
{
    public GameObject view;
    public InputActionReference closeAction;

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
    private void OnEnable()
    {
        if (closeAction != null)
            closeAction.action.performed += CloseView; //***
    }
    private void OnDisable()
    {
        if(closeAction != null)
            closeAction.action.performed -= CloseView;
    }
    private void CloseView(InputAction.CallbackContext context)
    {
        if(view.activeSelf)
        {
            view.SetActive(false);
        }
    }


}
