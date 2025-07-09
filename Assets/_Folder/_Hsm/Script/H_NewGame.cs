using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class H_NewGame : MonoBehaviour, H_UI
{
    public void OnPress()
    {
        Debug.Log(1);
        LobbyManager_J.Instance.OnClickNewGame();
    }

    public void OnRelease()
    {
        
    }
}
