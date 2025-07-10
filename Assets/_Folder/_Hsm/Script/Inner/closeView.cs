using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeView : MonoBehaviour, H_UI
{
    public void OnPress()
    {
        this.gameObject.SetActive(false);
    }

    public void OnRelease()
    {
    }
}
