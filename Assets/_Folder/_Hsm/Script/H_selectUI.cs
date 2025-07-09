using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class H_selectUI : MonoBehaviour
{
    public void OnUISelected(SelectEnterEventArgs args)
    {
        GameObject selectedObject = args.interactableObject.transform.gameObject;
        if(selectedObject.CompareTag("UI"))
        {
            var u = selectedObject.GetComponent<H_UIButton>();
            if(u != null)
            {
                u.OnPress();
            }

        }
    }
}
