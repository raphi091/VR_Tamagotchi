using CustomInspector.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FootstepDetector_LES : MonoBehaviour
{
    [Header("발걸음 설정")]
    [Tooltip("한 걸음으로 인정할 거리 (미터)")]
    public float stepDistance = 0.5f;

    // 내부 변수
    private CharacterController characterController;
    private Vector3 lastPosition;
    private float distanceTraveled = 0f;

    [SerializeField] private AudioClip indoorWalking;
    [SerializeField] private AudioClip outdoorWalking;



    void Start()
    {
        // XR Rig 구조에 따라 GetComponentInParent 또는 GetComponent 사용
        characterController = GetComponentInParent<CharacterController>();
        lastPosition = transform.position;
    }

    void Update()
    {
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
        // 발밑으로 Raycast를 쏴서 바닥 재질 감지
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2f))
        {
            if (gameObject.CompareTag("InDoorGround"))
                SoundManager.Instance.PlaySFX(indoorWalking);
            else if(gameObject.CompareTag("OutDoorGround"))
                SoundManager.Instance.PlaySFX(outdoorWalking);
        }
    }
}
