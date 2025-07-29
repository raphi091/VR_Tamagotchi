using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // XR Interaction Toolkit 네임스페이스 추가

public class Ch_Player_Events : MonoBehaviour
{
    // XR Ray Interactor의 'Select Entered' 이벤트에 연결할 함수
    // SelectEnterEventArgs를 통해 어떤 오브젝트가 선택됐는지 정보를 받아옵니다.

    public float selectThreshold;
    
    private bool isCatchable = false;
    private Ch_Throwable throwableTarget=null;
    public bool IsCatchable {get => isCatchable; set => isCatchable = value; }
    public Ch_Throwable ThrowableTarget { get => throwableTarget; set => throwableTarget = value; }
    
    
    public void OnDogSelected(SelectEnterEventArgs args)
    {
        // 선택된 오브젝트(Interactable)의 게임 오브젝트를 가져옴
        GameObject selectedObject = args.interactableObject.transform.gameObject;

        // 태그를 확인해서 강아지가 맞는지 검사
        if (selectedObject.CompareTag("Dog"))
        {
            Debug.Log($"선택된 강아지: {selectedObject.name}");

            // 강아지의 FSM 스크립트를 가져옴
            DogFSM_K dogFSM = selectedObject.GetComponent<DogFSM_K>();

            if (dogFSM != null)
            {
                float dis = Vector3.Distance(transform.position, dogFSM.transform.position);

                if (dis >= selectThreshold)
                {
                    dogFSM.BeCalled();
                }
                else
                {
                    DogInteractionManager_K.instance.SelectDog(dogFSM);
                }
            }
        }
    }

    public void OnDogSit(SelectEnterEventArgs args)
    {
        GameObject selectedObject = args.interactableObject.transform.gameObject;
        if (selectedObject.CompareTag("Dog"))
        {
            DogFSM_K dogFSM = selectedObject.GetComponent<DogFSM_K>();
            Debug.Log($"{selectedObject.name}: 앉아");
            dogFSM.CommandSit();
        }

    }
    
    public void OnDogLie(SelectEnterEventArgs args)
    {
        GameObject selectedObject = args.interactableObject.transform.gameObject;
        if (selectedObject.CompareTag("Dog"))
        {
            DogFSM_K dogFSM = selectedObject.GetComponent<DogFSM_K>();
            dogFSM.CommandLiedown();
        }

    }

    public void OnThrowableGrabbed(SelectEnterEventArgs args)
    {
        GameObject throwableObject = args.interactableObject.transform.gameObject;

        if (throwableObject.CompareTag("Throwable")&&ThrowableTarget==null)
        {
            Debug.Log($"물건 잡힘:{throwableObject.name}");
            throwableTarget = throwableObject.GetComponent<Ch_Throwable>();
            throwableTarget.GetReady();
        }
    }

    public void OnThrowableReleased(SelectExitEventArgs args)
    {
        GameObject throwableObject = args.interactableObject.transform.gameObject;

        if (throwableObject.CompareTag("Throwable"))
        {
            Debug.Log($"물건 놓음:{throwableObject.name}");
            throwableTarget = null;
        }
    }
    
}
