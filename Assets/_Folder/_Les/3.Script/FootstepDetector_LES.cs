using UnityEngine;

public class FootstepDetector_LES : MonoBehaviour
{
    [Header("발걸음 설정")]
    [Tooltip("한 걸음으로 인정할 거리 (미터)")]
    public float stepDistance = 0.5f;

    // 내부 변수
    [SerializeField] private CharacterController characterController;
    [SerializeField] private AudioSource PlayerAudio;
    private Vector3 lastPosition;
    private float distanceTraveled = 0f;

    [SerializeField] private AudioClip indoorWalking;
    [SerializeField] private AudioClip outdoorWalking;



    void Start()
    {
        // XR Rig 구조에 따라 GetComponentInParent 또는 GetComponent 사용
        characterController = GetComponent<CharacterController>();
        PlayerAudio = GetComponent<AudioSource>();
        lastPosition = transform.position;
    }

    void Update()
    {
        Debug.Log($"isGrounded: {characterController.isGrounded}, Velocity: {characterController.velocity.magnitude}");
        // 캐릭터가 땅에 닿아 있고, 움직일 때만 감지
        if (characterController.isGrounded && characterController.velocity.magnitude > 0.1f)
        {
            // 이동 거리 계산
            distanceTraveled += Vector3.Distance(transform.position, lastPosition);
            lastPosition = transform.position;

            // 누적 이동 거리가 설정된 값을 넘어서면
            if (distanceTraveled >= stepDistance)
            {
                TriggerFootstep();
                distanceTraveled = 0f; // 거리 초기화
            }
        }
    }

        private void TriggerFootstep()
    {
        Debug.Log("TriggerFootstep() 함수 호출 성공!"); // 1. 함수 호출 확인

        // Raycast의 길이를 넉넉하게 1m로 늘려서 테스트
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.0f))
        {
            Debug.Log("Raycast가 " + hit.collider.name + " 오브젝트에 닿았습니다. 태그는 '" + hit.collider.tag + "' 입니다."); // 2. Raycast 성공 및 태그 확인

            if (hit.collider.CompareTag("InDoorGround"))
            {
                Debug.Log("InDoorGround 태그 확인! 실내 발소리를 재생합니다."); // 3a. 실내 태그 확인
                if (indoorWalking != null)
                    PlayerAudio.PlayOneShot(indoorWalking);
                else
                    Debug.LogWarning("indoorWalking 오디오 클립이 비어있습니다!");
            }
            else if (hit.collider.CompareTag("OutDoorGround"))
            {
                Debug.Log("OutDoorGround 태그 확인! 실외 발소리를 재생합니다."); // 3b. 실외 태그 확인
                if (outdoorWalking != null)
                    PlayerAudio.PlayOneShot(outdoorWalking);
                else
                    Debug.LogWarning("outdoorWalking 오디오 클립이 비어있습니다!");
            }
            else
            {
                Debug.LogWarning("Raycast는 성공했지만, 바닥의 태그가 InDoorGround 또는 OutDoorGround가 아닙니다."); // 4. 태그 불일치 확인
            }
        }
        else
        {
            Debug.LogError("오류: 발밑으로 Raycast를 쐈지만 아무것도 감지되지 않았습니다. 바닥에 Collider가 있는지 다시 확인해주세요."); // 5. Raycast 실패 확인
        }
    }
}
