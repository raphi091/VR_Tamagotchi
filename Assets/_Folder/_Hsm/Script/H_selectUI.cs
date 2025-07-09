using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class H_selectUI : MonoBehaviour
{
    [Header("원거리 선택")]
    public Transform rightHandController;
    public InputActionReference selectAction;


    private void OnEnable()
    {
        selectAction.action.performed += SelectUI;
        selectAction.action.canceled += unSelectUI;
    }

    private void OnDisable()
    {
        selectAction.action.performed -= SelectUI;
        selectAction.action.canceled -= unSelectUI;
    }

    private void SelectUI(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if (Physics.Raycast(rightHandController.position, rightHandController.forward, out hit, 50f))
        {
            var ui = hit.collider.GetComponent<H_UI>();
            if (ui != null)
            {
                ui.OnPress();
            }
        }
    }

    private void unSelectUI(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if (Physics.Raycast(rightHandController.position, rightHandController.forward, out hit, 50f))
        {
            H_UIButton ui = hit.collider.GetComponent<H_UIButton>();
            if (ui != null)
            {
                ui.OnRelease();
            }
        }
       
    }
}
