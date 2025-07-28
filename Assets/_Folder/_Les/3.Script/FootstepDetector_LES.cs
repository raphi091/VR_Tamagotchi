using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(AudioSource))]
public class FootstepDetector_LES : MonoBehaviour
{
    [Header("오디오 설정")]
    [Tooltip("발소리 오디오 클립들. 여러 개를 넣으면 랜덤으로 재생됩니다.")]
    public AudioClip[] footstepClips;

    [Header("발소리 조건")]
    [Tooltip("이 속도(m/s) 이상으로 움직일 때만 발소리가 납니다.")]
    public float minSpeed = 0.1f;
    [Tooltip("기본 발소리 사이의 간격(초)")]
    public float stepInterval = 0.5f;

    [Header("동적 발소리 설정")]
    [Tooltip("이 속도를 기준으로 발소리 간격이 조절됩니다. 캐릭터의 평균 걷는 속도를 입력하세요.")]
    public float characterBaseSpeed = 2.0f; // 캐릭터의 '평균' 걷기 속도

    // 내부 변수
    private AudioSource audioSource;
    private CharacterController characterController;
    private float footstepTimer = 0f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();

        if (characterController == null)
        {
            Debug.LogError("이 스크립트는 CharacterController가 필요합니다. XR Origin에 추가해주세요.");
        }
    }

    void Update()
    {
        Vector3 horizontalVelocity = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);
        float currentSpeed = horizontalVelocity.magnitude;

        if (currentSpeed > minSpeed)
        {
            // 타이머 증가
            footstepTimer += Time.deltaTime;

            // '현재 속도'를 기반으로 동적인 발소리 간격을 계산합니다.
            float dynamicInterval = stepInterval / (currentSpeed / characterBaseSpeed);

            // 타이머가 동적으로 계산된 간격보다 커지면 소리 재생
            if (footstepTimer >= dynamicInterval)
            {
                PlayFootstepSound();
                footstepTimer = 0f; // 타이머 리셋
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }

    private void PlayFootstepSound()
    {
        if (footstepClips == null || footstepClips.Length == 0) return;
        
        int index = Random.Range(0, footstepClips.Length);
        AudioClip clip = footstepClips[index];
        audioSource.PlayOneShot(clip);
    }
}