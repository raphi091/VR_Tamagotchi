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

    private DogInteractionManager_K interactionManager;

    private void Awake()
    {
        // DogInteractionManager 인스턴스를 한번만 찾아와서 저장해 둡니다.
        interactionManager = DogInteractionManager_K.instance;

        // 각 입력 액션이 수행될 때(버튼이 눌릴 때) 해당하는 함수를 연결(구독)합니다.
        callAction.action.performed += context => CallDog();
        sitAction.action.performed += context => TryCommandSit();
        lieDownAction.action.performed += context => TryCommandLieDown();
    }

    private void OnDestroy()
    {
        // 오브젝트가 파괴될 때, 연결했던 함수들을 해제(구독 취소)합니다.
        callAction.action.performed -= context => CallDog();
        sitAction.action.performed -= context => TryCommandSit();
        lieDownAction.action.performed -= context => TryCommandLieDown();
    }

    private void CallDog()
    {
        RaycastHit hit;
        if (Physics.Raycast(rightHandController.position, rightHandController.forward, out hit, 50f))
        {
            DogFSM_K dog = hit.collider.GetComponent<DogFSM_K>();
            if (dog != null)
            {
                // 레이저에 맞은 강아지에게 '이리와' 명령을 전달합니다.
                dog.BeCalled(this.transform); // this.transform은 XR Origin의 위치
            }
        }
    }

    private void TryCommandSit()
    {
        // 매니저를 통해 현재 선택된 강아지가 있는지 확인합니다.
        DogFSM_K activeDog = interactionManager.GetActiveDog();
        if (activeDog != null)
        {
            // 선택된 강아지에게 '앉아' 명령을 전달합니다.
            activeDog.CommandSit();
        }
    }

    private void TryCommandLieDown()
    {
        DogFSM_K activeDog = interactionManager.GetActiveDog();
        if (activeDog != null)
        {
            // DogFSM_K 스크립트에 CommandLieDown() 함수를 만들어야 합니다.
            // activeDog.CommandLieDown();
            Debug.Log($"{activeDog.name}에게 엎드려 명령을 시도합니다. (함수 구현 필요)");
        }
    }
}
