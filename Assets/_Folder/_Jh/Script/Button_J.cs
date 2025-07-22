using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_J : MonoBehaviour, H_UI
{
    public void OnPress()
    {
        Debug.Log($"클릭됨");
        TutorialPageController.instance.ShowNextPage();
    }

    public void OnRelease()
    {
        
    }
}
