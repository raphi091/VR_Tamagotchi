using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기분 타입 enum 정의
public enum MoodType
{
    Good,
    Bad,
    Happy
}

public class ParticlePoolManager_LES : MonoBehaviour
{
    public static ParticlePoolManager_LES Instance;

    [Header("Particle Settings")]
    [SerializeField] private GameObject goodParticlePrefab;
    [SerializeField] private GameObject badParticlePrefab;
    [SerializeField] private GameObject happyParticlePrefab;
    
    [Header("Pool Settings")]
    [SerializeField] private int poolSize = 10; // 각 타입별 풀 크기
    [SerializeField] private float particleDuration = 3f; // 파티클 재생 시간

    // 각 MoodType별 파티클 풀
    private Dictionary<MoodType, Queue<GameObject>> particlePools;
    private Dictionary<MoodType, GameObject> particlePrefabs;


    private void Awake()
    {
        // 싱글톤 패턴
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePools();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 파티클 풀 초기화    
    private void InitializePools()
    {
        particlePools = new Dictionary<MoodType, Queue<GameObject>>();
        particlePrefabs = new Dictionary<MoodType, GameObject>();

        // 프리팹 딕셔너리 설정
        particlePrefabs[MoodType.Good] = goodParticlePrefab;
        particlePrefabs[MoodType.Bad] = badParticlePrefab;
        particlePrefabs[MoodType.Happy] = happyParticlePrefab;

        // 각 MoodType별로 풀 생성
        foreach (MoodType moodType in System.Enum.GetValues(typeof(MoodType)))
        {
            particlePools[moodType] = new Queue<GameObject>();
            
            // 풀에 파티클 오브젝트들을 미리 생성하여 추가
            for (int i = 0; i < poolSize; i++)
            {
                GameObject particle = CreateParticleObject(moodType);
                particlePools[moodType].Enqueue(particle);
            }
        }
    }

    // 파티클 오브젝트 생성    
    private GameObject CreateParticleObject(MoodType moodType)
    {
        GameObject prefab = particlePrefabs[moodType];
        if (prefab == null)
        {
            Debug.LogError($"Particle prefab for {moodType} is not assigned!");
            return null;
        }

        GameObject particle = Instantiate(prefab, transform);
        particle.SetActive(false);
        return particle;
    }

    // 파티클 재생 (위치 지정)    
    public void PlayParticle(MoodType moodType, Vector3 position)
    {
        GameObject particle = GetFromPool(moodType);
        if (particle != null)
        {
            particle.transform.position = position;
            particle.SetActive(true);

            // 파티클 시스템 재생
            ParticleSystem ps = particle.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }

            // 일정 시간 후 풀로 반환
            StartCoroutine(ReturnToPoolAfterDelay(particle, moodType));
        }
    }

    // 파티클 재생 (Transform 지정)    
    public void PlayParticle(MoodType moodType, Transform target)
    {
        PlayParticle(moodType, target.position);
    }

    // 풀에서 파티클 가져오기    
    private GameObject GetFromPool(MoodType moodType)
    {
        if (particlePools[moodType].Count > 0)
        {
            return particlePools[moodType].Dequeue();
        }
        else
        {
            // 풀이 비어있으면 새로 생성
            Debug.LogWarning($"Pool for {moodType} is empty. Creating new particle.");
            return CreateParticleObject(moodType);
        }
    }

    // 일정 시간 후 풀로 반환    
    private IEnumerator ReturnToPoolAfterDelay(GameObject particle, MoodType moodType)
    {
        yield return new WaitForSeconds(particleDuration);
        ReturnToPool(particle, moodType);
    }

    // 파티클을 풀로 반환    
    private void ReturnToPool(GameObject particle, MoodType moodType)
    {
        if (particle != null)
        {
            particle.SetActive(false);
            particlePools[moodType].Enqueue(particle);
        }
    }

    // 모든 파티클 정지    
    public void StopAllParticles()
    {
        foreach (var pool in particlePools.Values)
        {
            foreach (var particle in pool)
            {
                if (particle.activeInHierarchy)
                {
                    ParticleSystem ps = particle.GetComponent<ParticleSystem>();
                    if (ps != null)
                    {
                        ps.Stop();
                    }
                    particle.SetActive(false);
                }
            }
        }
    }

    // 특정 타입 파티클만 정지
    public void StopParticles(MoodType moodType)
    {
        foreach (var particle in particlePools[moodType])
        {
            if (particle.activeInHierarchy)
            {
                ParticleSystem ps = particle.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Stop();
                }
                particle.SetActive(false);
            }
        }
    }
}