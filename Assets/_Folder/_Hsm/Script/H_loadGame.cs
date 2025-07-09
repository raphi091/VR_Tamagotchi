using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class H_loadGame : MonoBehaviour, H_UI
{
    public void OnPress()
    {
        LobbyManager_J.Instance.OnClickContinue();
    }

    public void OnRelease()
    {
        
    }
}
