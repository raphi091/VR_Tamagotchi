using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class LunchDog : MonoBehaviour
{
    [Header("위치 지정")]
    public Transform waitPosition;
    public Transform bowlPosition;

    [Header("파티클 프리팹")]
    public Transform particlepoint;

    [Header("식사 설정")]
    public float eatingTime = 3f;
    public float rotationSpeed = 10f;
    private PetController_J petcontroller;
    private CharacterController controller;
    private Animator animator;
    private NavMeshAgent agent;
    private FoodBowl foodBowl;

    public PetController_J PetController => petcontroller;
    public FoodBowl FoodBowl => foodBowl;


    private void Awake()
    {
        if (!TryGetComponent(out petcontroller))
            Debug.LogWarning("LunchDog ] PetController_J 없음");

        if (!TryGetComponent(out animator))
            Debug.LogWarning("LunchDog ] Animator 없음");

        if (!TryGetComponent(out agent))
            Debug.LogWarning("LunchDog ] NavMeshAgent 없음");

        if (!TryGetComponent(out controller))
            Debug.LogWarning("LunchDog ] CharacterController 없음");
    }

    private void Update()
    {
        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        Vector3 moveDelta = agent.velocity * Time.deltaTime;

        if (!controller.isGrounded)
        {
            moveDelta.y -= 20f * Time.deltaTime;
        }

        controller.Move(moveDelta);
        animator.SetFloat("MOVESPEED", agent.velocity.magnitude);
    }

    public void SetLunchFood(FoodBowl bowl)
    {
        foodBowl = bowl;
        bowlPosition = bowl.transform;
        waitPosition = bowl.WaitPoint;
    }

    public void StartLunch()
    {
        StartCoroutine(LunchRoutine());
    }

    private IEnumerator LunchRoutine()
    {
        if (bowlPosition == null)
        {
            Debug.LogError($"[LunchDog] bowlPosition이 null입니다!");
            yield break;
        }

        agent.SetDestination(waitPosition.position);

        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        Vector3 directionToBowl = bowlPosition.position - transform.position;
        directionToBowl.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToBowl);
        while (Quaternion.Angle(transform.rotation, targetRotation) > 1.0f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        transform.rotation = targetRotation;

        yield return null;

        animator.SetBool("LUNCHEAT", true);

        yield return new WaitForSeconds(eatingTime);

        animator.SetBool("LUNCHEAT", false);
        foodBowl.ClearBowl();

        // 음식 비교 후 파티클 생성
        MoodType currentMood;
        if (petcontroller.petData.foodType.Equals(foodBowl.containedFood))
        {
            ParticlePoolManager_LES.Instance.PlayParticle(MoodType.Good, particlepoint);
            currentMood = MoodType.Good;
        }
        else
        {
            ParticlePoolManager_LES.Instance.PlayParticle(MoodType.Bad, particlepoint);
            currentMood = MoodType.Bad;
        }

        // 식사 완료 → 허기 회복
        petcontroller.petData.hungerper = 100f;
        
        // 파티클이 충분히 표시될 시간 대기 (예: 2.5초)
        yield return new WaitForSeconds(4f);

        // 파티클 종료
        ParticlePoolManager_LES.Instance.StopParticles(currentMood);
        petcontroller.currentIntimacy += 5f;

        // 식사 완료 알림
        LunchSceneManager.instance.OnDogFinished();
    }
}