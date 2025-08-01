using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class PetController_J : MonoBehaviour
{
    public PetStatusData_J petData;
    public NamePlate_K namePlate;

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

    private Animator ani;
    private BoxCollider col;

    private void Awake()
    {
        if (!TryGetComponent(out ani))
            Debug.LogWarning("PetController_J ] Animator 없음");

        if (!TryGetComponent(out col))
            Debug.LogWarning("PetController_J ] SphereCollider 없음");
    }

    // GameManager가 호출할 데이터 적용 함수
    public void ApplyData(PetStatusData_J data)
    {
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
        this.currentIntimacy = data.intimacyper;

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "H_Indoor")
        {
            this.currentHunger = Random.Range(80f, 90f);
            this.currentBowel = Random.Range(0f, 20f);
        }
        else
        {
            this.currentHunger = 100f;
            this.currentBowel = data.bowelper;
        }

        DogFSM_K fsm = GetComponent<DogFSM_K>();
        if (fsm != null)
        {
            fsm.mouthpoint = transform.FindSlot("PICKUP_POINT");
            fsm.particlepoint = transform.FindSlot("PARTICLE_POINT");
            fsm.data = DatabaseManager_J.instance.personalities[data.personalityIndex];
            fsm.player = GameObject.FindGameObjectWithTag("Player").transform;

            switch (DatabaseManager_J.instance.petProfiles[data.modelIndex].petSize)
            {
                case PetSize.Big:
                    col.center = new Vector3(0f, 0.6f, 0.1f);
                    col.size = new Vector3(0.2f, 0.55f, 1f);
                    break;
                case PetSize.medium:
                    col.center = new Vector3(0f, 0.47f, 0.07f);
                    col.size = new Vector3(0.2f, 0.45f, 0.85f);
                    break;
                case PetSize.small:
                    col.center = new Vector3(0f, 0.25f, 0.035f);
                    col.size = new Vector3(0.2f, 0.35f, 0.5f);
                    break;
            }

            fsm.snack = FindObjectOfType<Snack>();
            if (fsm.snack != null)
                fsm.snack.SetUp(false);
        }

        LunchDog lunchDog = GetComponent<LunchDog>();
        if (lunchDog != null)
        {
            lunchDog.particlepoint = transform.FindSlot("PARTICLE_POINT");
        }

        if (ani != null)
        {
            ani.runtimeAnimatorController = DatabaseManager_J.instance.petProfiles[data.modelIndex].petAnimator;
            ani.avatar = DatabaseManager_J.instance.petProfiles[data.modelIndex].petAvater;
        }

        // 5. 모든 설정이 끝났으니 오브젝트를 활성화
        this.gameObject.SetActive(true);
    }
}
