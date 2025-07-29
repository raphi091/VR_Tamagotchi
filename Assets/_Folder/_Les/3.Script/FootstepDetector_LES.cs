using UnityEngine;
using UnityEngine.SceneManagement;

// 인스펙터 창에서 씬별 설정을 보기 좋게 만들기 위한 클래스
[System.Serializable]
public class SceneFootstepSet
{
    [Tooltip("적용할 씬의 이름")]
    public string sceneName;
    [Tooltip("해당 씬에서 재생될 발소리 오디오 클립들")]
    public AudioClip[] footstepClips;
}

[RequireComponent(typeof(AudioSource))]
public class FootstepDetector_LES : MonoBehaviour
{
    [Header("핵심 설정"), Tooltip("위치를 추적할 오브젝트 (XR Origin > Camera Offset)")]
    public Transform bodyTransform;
    [Tooltip("발소리 사이의 고정된 시간 간격 (초)")]
    public float stepInterval = 0.5f;
    [Tooltip("움직임으로 감지할 최소 이동 거리. 매우 작은 값이어야 합니다.")]
    public float minMoveThreshold = 0.001f;

    [Header("씬별 오디오 설정"), Tooltip("각 씬에 맞는 발소리 세트를 설정합니다.")]
    public SceneFootstepSet[] sceneSets; // 씬별 오디오 세트 배열

    // 내부 변수
    private AudioSource audioSource;
    private Vector3 lastPosition;
    private float footstepTimer = 0f;
    private AudioClip[] currentSceneClips; // 현재 씬에서 사용할 클립 배열

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (bodyTransform == null)
        {
            Debug.LogError("Body Transform이 연결되지 않았습니다! XR Origin의 Camera Offset을 연결해주세요.");
            this.enabled = false;
            return;
        }

        lastPosition = bodyTransform.position;

        // 현재 씬에 맞는 오디오 클립을 찾아서 설정
        InitializeFootstepsForCurrentScene();
    }

    // 현재 씬을 확인하고, 그에 맞는 발소리 클립 배열을 준비하는 메서드
    void InitializeFootstepsForCurrentScene()
    {
        // 현재 활성화된 씬의 정보를 가져옴
        Scene currentScene = SceneManager.GetActiveScene();

        // 설정된 씬 세트들을 순회하며 현재 씬 이름과 일치하는 것을 찾음
        foreach (var set in sceneSets)
        {
            if (set.sceneName == currentScene.name)
            {
                currentSceneClips = set.footstepClips; // 일치하는 세트의 클립을 사용
                Debug.Log($"'{currentScene.name}' 씬을 위한 발소리가 로드되었습니다.");
                return; // 찾았으면 함수 종료
            }
        }

        // 루프가 끝날 때까지 맞는 세트를 못 찾았다면 경고 메시지 출력
        Debug.LogWarning($"'{currentScene.name}' 씬에 맞는 발소리 설정이 없습니다!");
    }

    void Update()
    {
        Vector3 currentXZPosition = new Vector3(bodyTransform.position.x, 0, bodyTransform.position.z);
        Vector3 lastXZPosition = new Vector3(lastPosition.x, 0, lastPosition.z);
        float distanceMoved = Vector3.Distance(currentXZPosition, lastXZPosition);
        
        lastPosition = bodyTransform.position;

        if (distanceMoved > minMoveThreshold)
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= stepInterval)
            {
                PlayFootstepSound();
                footstepTimer = 0f;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }

    private void PlayFootstepSound()
    {
        // 현재 씬의 클립 배열을 사용하도록 수정
        if (currentSceneClips == null || currentSceneClips.Length == 0) return;
        
        int index = Random.Range(0, currentSceneClips.Length);
        audioSource.PlayOneShot(currentSceneClips[index]);
    }
}