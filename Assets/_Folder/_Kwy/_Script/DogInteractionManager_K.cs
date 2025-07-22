using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogInteractionManager_K : MonoBehaviour
{
    public static DogInteractionManager_K instance;

    private List<DogFSM_K> requestingDogs = new List<DogFSM_K>();

    private DogFSM_K activeDog = null;

    public DogFSM_K ActiveDog => activeDog;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 강아지가 상호작용을 요청할 때 호출하는 함수
    public void RequestInteraction(DogFSM_K dog)
    {
        if (!requestingDogs.Contains(dog))
        {
            Debug.Log($"{dog.name}이(가) 상호작용을 요청합니다.");
            requestingDogs.Add(dog);
        }
    }

    // 강아지가 상호작용 요청을 취소할 때
    public void CancelRequest(DogFSM_K dog)
    {
        if (requestingDogs.Contains(dog))
        {
            requestingDogs.Remove(dog);
        }
    }

    // 플레이어가 특정 강아지를 '선택'했을 때 호출하는 함수
    public void SelectDog(DogFSM_K selectedDog)
    {
        Debug.Log(requestingDogs.Count);
        if (!requestingDogs.Contains(selectedDog)&&selectedDog.GetCurrentState() != DogFSM_K.State.Hunger) return;

        DogFSM_K.State dogState = selectedDog.GetCurrentState();

        switch (dogState)
        {
            case DogFSM_K.State.Hunger:
                // 강아지가 배고픈 상태라면, 바로 '간식 주기' 로직을 실행
                Debug.Log($"[Manager] {selectedDog.name}은(는) 배고픈 상태입니다. 간식을 줍니다.");
                selectedDog.ReceiveSnack();
                // 간식을 주면 상호작용이 끝났으므로, 다른 강아지들도 Wander 상태로 돌려보냄
                DeselectAllDogs();
                break;

            case DogFSM_K.State.InteractionRequest:
                // 일반적인 상호작용 요청이라면, 이전과 동일하게 선택 절차를 진행
                Debug.Log($"[Manager] {selectedDog.name}을(를) 상호작용 대상으로 선택합니다.");
                ConfirmAndDeselectOthers(selectedDog);
                break;

            default:
                // 그 외 다른 상태라면 일단 선택 절차 진행
                ConfirmAndDeselectOthers(selectedDog);
                break;
        }
    }

    public DogFSM_K GetActiveDog()
    {
        if (requestingDogs.Count == 0) return null;

        return activeDog;
    }

    private void ConfirmAndDeselectOthers(DogFSM_K selectedDog)
    {
        activeDog = selectedDog;
        activeDog.ConfirmSelection();

        for (int i = requestingDogs.Count - 1; i >= 0; i--)
        {
            if (requestingDogs[i] != selectedDog)
            {
                requestingDogs[i].ReturnToWander();
            }
        }
        requestingDogs.Clear();
    }

    // 모든 강아지를 Wander 상태로 되돌리는 함수
    private void DeselectAllDogs()
    {
        for (int i = requestingDogs.Count - 1; i >= 0; i--)
        {
            requestingDogs[i].ReturnToWander();
        }
        requestingDogs.Clear();
    }
}
