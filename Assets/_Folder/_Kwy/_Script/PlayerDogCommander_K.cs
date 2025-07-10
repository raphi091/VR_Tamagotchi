using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDogCommander_K : MonoBehaviour
{
    [Header("근거리 명령 (선택된 강아지 대상)")]
    public InputActionReference sitAction; // '앉아' 명령에 사용할 버튼
    public InputActionReference lieDownAction; // '엎드려' 명령에 사용할 버튼

    private DogInteractionManager_K interactionManager;

    private void Awake()
    {
        // DogInteractionManager 인스턴스를 한번만 찾아와서 저장해 둡니다.
        interactionManager = DogInteractionManager_K.instance;
    }

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
        if (interactionManager == null) return;
        DogFSM_K activeDog = interactionManager.GetActiveDog();
        if (activeDog != null)
        {
            activeDog.CommandSit();
        }
    }

    private void TryCommandLieDown(InputAction.CallbackContext context)
    {
        if (interactionManager == null) return;
        DogFSM_K activeDog = interactionManager.GetActiveDog();
        if (activeDog != null)
        {
            activeDog.CommandLiedown();
        }
    }
}
