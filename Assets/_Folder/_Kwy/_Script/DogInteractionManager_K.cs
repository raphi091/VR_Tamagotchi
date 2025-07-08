using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogInteractionManager_K : MonoBehaviour
{
    public static DogInteractionManager_K instance;

    private List<DogFSM_K> requestingDogs = new List<DogFSM_K>();

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

    // 강아지가 상호작용 요청을 취소할 때 (플레이어 영역을 벗어날 때)
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
        if (!requestingDogs.Contains(selectedDog))
        {
            Debug.Log("이 강아지는 상호작용을 요청한 상태가 아닙니다.");
            return;
        }

        Debug.Log($"{selectedDog.name} 선택됨! 상호작용을 시작합니다.");

        // 선택된 강아지에게는 상호작용 시작 신호를 보냄
        selectedDog.StartInteraction();

        // 선택받지 못한 나머지 강아지들을 순회하며 Wander 상태로 돌려보냄
        for (int i = requestingDogs.Count - 1; i >= 0; i--)
        {
            DogFSM_K dog = requestingDogs[i];
            if (dog != selectedDog)
            {
                Debug.Log($"{dog.name}은(는) 선택받지 못해 Wander 상태로 돌아갑니다.");
                dog.ReturnToWander();
            }
        }

        // 모든 결정이 끝났으므로 요청 목록을 비움
        requestingDogs.Clear();
    }
}
