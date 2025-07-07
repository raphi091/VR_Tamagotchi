using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch_LobbyManager : MonoBehaviour
{
    void OnStartButton()
    {
          
    }

    void OnLoadButton()
    {
        Ch_SaveFileHandler.I.LoadData();
    }
}
