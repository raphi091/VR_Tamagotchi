using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDogCommander_K : MonoBehaviour
{
    [Header("원거리 명령 (호출)")]
    public Transform rightHandController; // 레이저를 쏠 컨트롤러
    public InputActionReference callAction; // '이리와' 명령에 사용할 버튼

    [Header("근거리 명령 (선택된 강아지 대상)")]
    public InputActionReference sitAction; // '앉아' 명령에 사용할 버튼
    public InputActionReference lieDownAction; // '엎드려' 명령에 사용할 버튼
    public InputActionReference catchAction; // '엎드려' 명령에 사용할 버튼

    private DogInteractionManager_K interactionManager;

    private void Awake()
    {
        // DogInteractionManager 인스턴스를 한번만 찾아와서 저장해 둡니다.
        interactionManager = DogInteractionManager_K.instance;
    }

    private void OnEnable()
    {
        // 각 입력 액션이 수행될 때(버튼이 눌릴 때) 해당하는 함수를 연결(구독)합니다.
        callAction.action.performed += CallDog;
        sitAction.action.performed += TryCommandSit;
        lieDownAction.action.performed += TryCommandLieDown;
        // catchAction.action.performed += TryCommandCatch;
    }

    private void OnDisable()
    {
        // 오브젝트가 파괴될 때, 연결했던 함수들을 해제(구독 취소)합니다.
        callAction.action.performed -= CallDog;
        sitAction.action.performed -= TryCommandSit;
        lieDownAction.action.performed -= TryCommandLieDown;
        // catchAction.action.performed -= TryCommandCatch;
    }

    private void CallDog(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if (Physics.Raycast(rightHandController.position, rightHandController.forward, out hit, 50f))
        {
            DogFSM_K dog = hit.collider.GetComponent<DogFSM_K>();
            if (dog != null)
            {
                dog.BeCalled(this.transform);
            }
        }
    }

    private void TryCommandSit(InputAction.CallbackContext context)
    {
        DogFSM_K activeDog = interactionManager.GetActiveDog();
        if (activeDog != null)
        {
            activeDog.CommandSit();
        }
    }

    private void TryCommandLieDown(InputAction.CallbackContext context)
    {
        DogFSM_K activeDog = interactionManager.GetActiveDog();
        if (activeDog != null)
        {
            activeDog.CommandLiedown();
        }
    }

    private void TryCommandCatch(InputAction.CallbackContext context)
    {
        // if (손에 막대기가 없으면) return;

        DogFSM_K activeDog = interactionManager.GetActiveDog();
        if (activeDog != null)
        {
            activeDog.CommandCatch();
        }
    }
}
