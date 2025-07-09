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
        selectAction.action.canceled += context => unSelectUI();
    }

    private void OnDestroy()
    {
        selectAction.action.performed -= context => SelectUI();
        selectAction.action.canceled -= context => unSelectUI();
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

        notice noticeScript = FindObjectOfType<notice>();
        if(noticeScript == null)
        {
//Debug.LogError("⚠️ FindObjectOfType<notice>() 결과가 null임! 씬에 notice 스크립트 붙은 오브젝트가 없거나 비활성화!");
            noticeScript.viewGesipan(hit.collider.gameObject);
            noticeScript.ToggleGesipan(hit.collider.gameObject);
            return;
        }
        else
        {
            Debug.Log("noticeScript 찾음: " + noticeScript.gameObject.name);
        }
        noticeScript.viewGesipan(hit.collider.gameObject);
    }
    private void unSelectUI()
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
