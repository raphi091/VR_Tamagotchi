using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterController))]
public class DogFSM_K : MonoBehaviour
{
    /*
    초창기에는 내부 공간 탐색
    자유놀기 혼자 논다 (친밀도에 따라 확률적으로 같이 논다)
    가까이 다가가면 상호작용 할지 안할지
    배고프면 간식
    */

    public enum State { Setup, Wander, Playing, Interaction, Stroking, Sit, Liedown, Hunger, Toilet}
    [Header("AI 상태")]
    [SerializeField] private State currentState;
    [SerializeField] private PersonalityData_LES data;
    [SerializeField] private Transform ToiletPoint;

    private Transform player;
    private NavMeshAgent agent;
    private CharacterController controller;
    private Animator animator;
    private float rotationSpeed = 10f;
    private float lastStateChangeTime;
    private float hungerpercent;
    private float bowelpercent;
    private bool isWandering;
    private bool isHunger = false;
    private bool isBowel = false;

    private Coroutine currentStateCoroutine;


    private void Awake()
    {
        if (!TryGetComponent(out agent))
            Debug.LogWarning("DogFSM ] NavMeshAgent 없음");

        if (!TryGetComponent(out controller))
            Debug.LogWarning("DogFSM ] CharacterController 없음");

        if (!TryGetComponent(out animator))
            Debug.LogWarning("DogFSM ] Animator 없음");

        hungerpercent = 80f;
        bowelpercent = data.bowel_movement;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        EnterState(State.Setup);
    }

    private void Update()
    {
        hungerpercent -= Time.deltaTime * 0.6f;
        bowelpercent -= Time.deltaTime * 0.6f;

        if (/*!useRootMotionLogic &&*/ agent.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        Vector3 finalMoveDelta;

        finalMoveDelta = agent.velocity * Time.deltaTime;

        if (!controller.isGrounded)
        {
            finalMoveDelta.y -= 20f * Time.deltaTime;
        }

        controller.Move(finalMoveDelta);
    }

    //private void OnAnimatorMove()
    //{
    //    if (!controller.enabled || Time.deltaTime <= 0) return;

    //    Vector3 finalMoveDelta;

    //    finalMoveDelta = agent.velocity * Time.deltaTime;

    //    //if (useRootMotionLogic)
    //    //{
    //    //    finalMoveDelta = animator.deltaPosition;
    //    //}
    //    //else
    //    //{
    //    //    finalMoveDelta = agent.velocity * Time.deltaTime;
    //    //}

    //    if (!controller.isGrounded)
    //    {
    //        finalMoveDelta.y -= 20f * Time.deltaTime;
    //    }

    //    controller.Move(finalMoveDelta);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // "나 상호작용하고 싶어!" 라고 매니저에게 요청
            DogInteractionManager_K.instance.RequestInteraction(this);

            // 플레이어 근처에서 기다리는 상태로 전환 (선택적)
            // EnterState(State.WaitingForSelection); 
            // 혹은 기존 Interaction_co 코루틴이 플레이어를 바라보며 기다리므로 그대로 사용 가능
            EnterState(State.Interaction);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // "나 이제 갈게" 라고 매니저에게 알림
            DogInteractionManager_K.instance.CancelRequest(this);
            EnterState(State.Wander);
        }
    }

    private void EnterState(State newState)
    {
        if (currentStateCoroutine != null)
        {
            StopCoroutine(currentStateCoroutine);
        }
        currentState = newState;
        lastStateChangeTime = Time.time;

        //if (newState == State.Attacking || newState == State.Broken)
        //{
        //    useRootMotionLogic = true;
        //    agent.isStopped = true;
        //}
        //else
        //{
        //    useRootMotionLogic = false;
        //    if (agent.enabled)
        //        agent.isStopped = false;
        //}

        switch (currentState)
        {
            case State.Setup:
                currentStateCoroutine = StartCoroutine(Setup_co());
                break;
            case State.Wander:
                currentStateCoroutine = StartCoroutine(Wander_co());
                break;
            case State.Playing:
                currentStateCoroutine = StartCoroutine(Play_co());
                break;
            case State.Interaction:
                currentStateCoroutine = StartCoroutine(Interaction_co());
                break;
            case State.Stroking:
                break;
            case State.Sit:
                break;
            case State.Liedown:
                break;
            case State.Hunger:
                currentStateCoroutine = StartCoroutine(Hunger_co());
                break;
            case State.Toilet:
                currentStateCoroutine = StartCoroutine(Toilet_co());
                break;
        }
    }

    private IEnumerator Setup_co()
    {
        yield return null;

        EnterState(State.Wander);
    }

    private IEnumerator Wander_co()
    {
        agent.isStopped = true;
        agent.ResetPath();
        isWandering = false;
        // animator.SetFloat("MoveSpeed", 0);

        while (true)
        {
            yield return null;

            if (hungerpercent <= 10f || isHunger)
            {
                EnterState(State.Hunger);
            }

            if (bowelpercent <= 10f)
            {
                EnterState(State.Toilet);
            }

            if (isWandering)
            {
                if (agent.remainingDistance < 0.5f)
                {
                    isWandering = false;
                    lastStateChangeTime = Time.time;
                }
            }
            else
            {
                if (Time.time > lastStateChangeTime + 1f)
                {
                    int randAction = Random.Range(0, 10);
                    float rang;
                    float movespeed;

                    if (randAction < 7)
                    {
                        if (data.intimacy >= 80)
                        {
                            rang = data.Active_MovingRang;
                            movespeed = data.Active_walkSpeed;
                        }
                        else if (data.intimacy >= 20)
                        {
                            rang = data.MovingRang;
                            movespeed = data.walkSpeed;
                        }
                        else
                        {
                            rang = data.Sky_MovingRang;
                            movespeed = data.Shy_walkSpeed;
                        }           
                        
                        Vector3 randomDirection = Random.insideUnitSphere * rang;
                        randomDirection += transform.position;

                        NavMeshHit hit;
                        NavMesh.SamplePosition(randomDirection, out hit, rang, 1);
                        Vector3 finalPosition = hit.position;

                        agent.speed = movespeed;
                        agent.SetDestination(finalPosition);
                        agent.isStopped = false;
                        isWandering = true;
                    }
                    else if (randAction > 9)
                    {
                        EnterState(State.Playing);
                    }
                    else
                    {
                        Debug.Log("월!");
                    }
                }
            }

            yield return null;
        }
    }

    private IEnumerator Play_co()
    {
        // 애니메이션 확인 후 작업

        yield return new WaitForSeconds(10f);

        EnterState(State.Wander);
    }

    private IEnumerator Interaction_co()
    {
        agent.isStopped = true;
        agent.ResetPath();

        while (true)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            yield return null;
        }
    }

    public void StartInteraction()
    {
        // 이 함수가 호출되면, 진짜 상호작용(쓰다듬기, 손 주기 등)을 받을 준비를 함
        // 현재 Interaction 상태의 코루틴이 이미 플레이어를 바라보고 있으므로
        // 추가적인 상태 전환 없이 그대로 대기하면 됩니다.
        Debug.Log($"{name}: 제가 선택되었군요! 다음 상호작용을 기다립니다.");
    }

    // 매니저가 "넌 선택되지 않았어"라고 호출해 줄 함수
    public void ReturnToWander()
    {
        // 즉시 Wander 상태로 돌아감
        EnterState(State.Wander);
    }

    private IEnumerator Hunger_co()
    {
        isHunger = true;
        agent.SetDestination(player.position);

        yield return StartCoroutine(Chasing_co());
    }

    private IEnumerator Toilet_co()
    {
        isBowel = true;
        agent.SetDestination(ToiletPoint.position);

        yield return new WaitForSeconds(2f);

        isBowel = false;
        EnterState(State.Wander);
    }

    private IEnumerator Chasing_co()
    {
        if (currentStateCoroutine != null)
        {
            StopCoroutine(currentStateCoroutine);
        }

        agent.isStopped = false;
        agent.speed = data.walkSpeed;

        float repathTimer = 0f;
        float repathInterval = 0.2f;

        while (true)
        {
            yield return null;

            repathTimer -= Time.deltaTime;
            if (repathTimer <= 0f)
            {
                repathTimer = repathInterval;
                agent.SetDestination(player.position);
            }

            agent.speed = data.walkSpeed;
            agent.isStopped = false;
            agent.SetDestination(player.position);

            yield return null;
        }
    }

    public void ReceiveInteraction(string interactionType)
    {
        if (currentState != State.Interaction) return;

        Debug.Log($"플레이어로부터 '{interactionType}' 상호작용을 받았습니다!");

        // 전달된 상호작용 타입에 따라 다른 행동을 시작
        switch (interactionType)
        {
            case "Pat":
                // 쓰다듬기 애니메이션을 재생하는 코루틴 등을 시작
                StartCoroutine(Patting_co());
                break;
            case "ShakeHand":
                // 악수하는 코루틴 시작
                // StartCoroutine(ShakeHand_co());
                break;
        }
    }

    private IEnumerator Patting_co()
    {
        // "쓰다듬기" 애니메이션 재생
        // animator.SetTrigger("Patting");
        Debug.Log("강아지를 쓰다듬습니다...");

        yield return new WaitForSeconds(3f); // 3초 동안 애니메이션 재생

        // 상호작용이 끝났으니 다시 배회 상태로 돌아갈 수 있지만,
        // 여기서는 플레이어가 벗어날 때까지 계속 상호작용을 기다리게 둡니다.
        // 플레이어가 영역을 나가면 OnTriggerExit가 Wander 상태로 바꿔줄 것입니다.
        Debug.Log("쓰다듬기 완료. 다음 상호작용 대기 중...");
    }
}
