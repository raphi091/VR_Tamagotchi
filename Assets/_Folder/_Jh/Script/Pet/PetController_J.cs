using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PetController_J : MonoBehaviour
{
    public PetStatusData_J petData;

    [Header("시각적 요소")]
    // 모델링 프리팹이 실제로 생성될 위치 (자식 오브젝트)
    public GameObject petModelSlot;
    // 이름표 UI (TextMeshProUGUI 또는 UI.Text)
    public TextMeshProUGUI nameText;

    [Header("현재 상태 (실시간 변경값)")]
    // 게임 플레이 중에 계속 변하는 현재 수치들
    public float currentHunger;
    public float currentIntimacy;
    public float currentBowel;

    // GameManager가 호출할 데이터 적용 함수
    public void ApplyData(PetStatusData_J data)
    {
        if (data == null)
        {
            Debug.LogError("💥 ApplyData() 실패: PetStatusData_J 가 null입니다.");
            return;
        }

        // 1. 전달받은 데이터를 이 컨트롤러에 저장
        this.petData = data;

        // 2. 모델링 적용
        // 만약 이전에 생성된 모델이 있다면 먼저 파괴
        if (petModelSlot.transform.childCount > 0)
        {
            Destroy(petModelSlot.transform.GetChild(0).gameObject);
        }

        // DatabaseManager에서 데이터에 맞는 모델 '프리팹'을 가져옴
        GameObject modelPrefab = DatabaseManager_J.instance.petProfiles[data.modelIndex].modelPrefab;
        // 가져온 프리팹을 petModelSlot의 자식으로 '생성(Instantiate)'
        Instantiate(modelPrefab, petModelSlot.transform);

        // 3. 이름 적용
        // 게임오브젝트의 이름을 펫 이름으로 변경하면 구분이 쉬움
        this.name = data.petName;
        if (nameText != null)
        {
            nameText.text = data.petName; // UI 텍스트 변경
        }

        // 4. 저장된 수치를 현재 상태 변수에 적용 (게임 시작 시)
        this.currentHunger = data.hungerper;
        this.currentIntimacy = data.intimacyper;
        this.currentBowel = data.bowelper;

        DogFSM_K fsm = GetComponent<DogFSM_K>();
        fsm.mouthTransform = transform.FindSlot("_MOUSE_", "MOUSE");
        fsm.data = DatabaseManager_J.instance.personalities[data.personalityIndex];
        fsm.player = GameObject.FindGameObjectWithTag("Player").transform;

        // 5. 모든 설정이 끝났으니 오브젝트를 활성화
        this.gameObject.SetActive(true);

        //TEMP
        GetComponent<DogFSM_K>().cubeRenderer = GetComponentInChildren<Renderer>();
        //TEMP
    }
}
