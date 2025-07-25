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
        Debug.DrawRay(transform.position, Vector3.down * 2f, Color.green);

        // 발밑으로 Raycast를 쏴서 바닥 재질 감지
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2f))
        {
            // 1. Raycast가 무언가에 닿기는 했다는 의미의 로그
            Debug.Log("Raycast Hit! 닿은 오브젝트: " + hit.collider.name + ", 태그: " + hit.collider.tag);

            if (hit.collider.CompareTag("InDoorGround"))
            {
                Debug.Log("내부 걷는 중");
                if(indoorWalking != null) PlayerAudio.PlayOneShot(indoorWalking);
            }
            else if (hit.collider.CompareTag("OutDoorGround"))
            {
                Debug.Log("외부 걷는 중");
                if (outdoorWalking != null) PlayerAudio.PlayOneShot(outdoorWalking);
            }
            else
            {
                // 2. 태그가 일치하지 않을 경우의 로그
                Debug.Log("닿았지만, 태그가 InDoorGround 또는 OutDoorGround가 아님.");
            }
        }
        else
        Debug.LogWarning("Raycast가 아무것도 감지하지 못함!");
    }
}
