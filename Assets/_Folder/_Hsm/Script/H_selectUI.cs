using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class H_selectUI : MonoBehaviour
{
    [Header("원거리 선택")]
    public Transform rightHandController;
    public InputActionReference selectAction;


    private void Awake()
    {
        selectAction.action.performed += context => SelectUI();
    }

    private void OnDestroy()
    {
        selectAction.action.performed -= context => SelectUI();
    }
    private void SelectUI()
    {
        RaycastHit hit;
        if (Physics.Raycast(rightHandController.position, rightHandController.forward, out hit, 50f))
        {
            H_UIButton ui = hit.collider.GetComponent<H_UIButton>();
            if (ui != null)
            {
                ui.OnPress();
            }
        }
    }
}
