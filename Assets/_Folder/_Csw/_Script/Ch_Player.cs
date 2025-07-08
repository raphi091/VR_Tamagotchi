using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // XR Interaction Toolkit 네임스페이스 추가

public class Ch_VRDogSelector : MonoBehaviour
{
    // XR Ray Interactor의 'Select Entered' 이벤트에 연결할 함수
    // SelectEnterEventArgs를 통해 어떤 오브젝트가 선택됐는지 정보를 받아옵니다.
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
                // 매니저에게 이 강아지를 선택했다고 알림
                DogInteractionManager_K.instance.SelectDog(dogFSM);
            }
        }
    }
}
