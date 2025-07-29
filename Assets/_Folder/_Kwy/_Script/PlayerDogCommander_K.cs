using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDogCommander_K : MonoBehaviour
{
    [Header("근거리 명령 (선택된 강아지 대상)")]
    public InputActionReference sitAction; // '앉아' 명령에 사용할 버튼
    public InputActionReference lieDownAction; // '엎드려' 명령에 사용할 버튼

    private void OnEnable()
    {
        sitAction.action.performed += TryCommandSit;
        lieDownAction.action.performed += TryCommandLieDown;
    }

    private void OnDisable()
    {
        sitAction.action.performed -= TryCommandSit;
        lieDownAction.action.performed -= TryCommandLieDown;
    }

    private void TryCommandSit(InputAction.CallbackContext context)
    {
        Debug.Log("TryCommandSit");
        if (DogInteractionManager_K.instance == null)
        {
            Debug.Log("interactionManager == null");
            return;
        }
        DogFSM_K activeDog = DogInteractionManager_K.instance.GetActiveDog();
        if (activeDog != null)
        {
            activeDog.CommandSit();
        }
        else
        {
            Debug.Log("activeDog == null");
        }
    }

    private void TryCommandLieDown(InputAction.CallbackContext context)
    {
        Debug.Log("TryCommandLieDown");
        if (DogInteractionManager_K.instance == null)
        {
            Debug.Log("interactionManager == null");
            return;
        }
        DogFSM_K activeDog = DogInteractionManager_K.instance.GetActiveDog();
        if (activeDog != null)
        {
            activeDog.CommandLiedown();
        }
        else
        {
            Debug.Log("activeDog == null");
        }
    }
}
