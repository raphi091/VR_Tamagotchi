﻿using System.Collections;
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

    옮겨야하는 값
    친밀도, 배고픔지수, 배변활동지수, 모델링, 데이터
    */

    public PersonalityData_LES data;
    public enum State { Setup, Wander, Playing, InteractionRequest, Interaction, Stroking, Sit, Liedown, Catch, Called, Hunger, Toilet}
    [Header("AI 상태")]
    [SerializeField] private State currentState;

    [SerializeField] private Transform ToiletPoint;
    [SerializeField] private Transform EatPoint;

    public Transform mouthTransform;
    public Transform player;

    private NavMeshAgent agent;
    private CharacterController controller;
    private Animator animator;

    private float currentintimacy;
    private float rotationSpeed = 10f;
    private float lastStateChangeTime;
    private float hungerpercent;
    private float bowelpercent;
    private bool isSelected = false;
    private bool isWandering;
    private bool isHunger = false;
    private bool isBowel = false;

    private Coroutine currentStateCoroutine;

    public PersonalityData_LES Data => data;
    public float Currentintimacy => currentintimacy;
    public float Hungerpercent => hungerpercent;
    public float Bowelpercent => bowelpercent;


    //TEMP
    public Renderer cubeRenderer;
    //TEMP


    private void Awake()
    {
        if (!TryGetComponent(out agent))
            Debug.LogWarning("DogFSM ] NavMeshAgent 없음");

        if (!TryGetComponent(out controller))
            Debug.LogWarning("DogFSM ] CharacterController 없음");

        if (!TryGetComponent(out animator))
            Debug.LogWarning("DogFSM ] Animator 없음");

        agent.updatePosition = false;
        agent.updateRotation = false;

        hungerpercent = 80f;
        bowelpercent = 100f;

    }

    private void Start()
    {
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

        Vector3 moveDelta = agent.velocity * Time.deltaTime;

        if (!controller.isGrounded)
        {
            moveDelta.y -= 20f * Time.deltaTime;
        }

        controller.Move(moveDelta);
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
        if (other.CompareTag("PlayerHand") && isSelected && currentState == State.Interaction)
        {
            EnterState(State.Stroking);
            return;
        }

        if (other.CompareTag("Player"))
        {
            if (currentState == State.Wander || currentState == State.Playing || currentState == State.Called)
            {
                EnterState(State.InteractionRequest);
                DogInteractionManager_K.instance.RequestInteraction(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHand") && currentState == State.Stroking)
        {
            EnterState(State.Interaction);
        }

        if (other.CompareTag("Player"))
        {
            // "나 이제 갈게" 라고 매니저에게 알림
            DogInteractionManager_K.instance.CancelRequest(this);

            if (currentState == State.InteractionRequest || currentState == State.Interaction)
            {
                ReturnToWander();
            }

        }
    }

    private void EnterState(State newState, object data = null)
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
            case State.InteractionRequest:
                currentStateCoroutine = StartCoroutine(InteractionRequest_co());
                break;
            case State.Interaction:
                currentStateCoroutine = StartCoroutine(Interaction_co());
                break;
            case State.Stroking:
                currentStateCoroutine = StartCoroutine(Stroking_co());
                break;
            case State.Sit:
                currentStateCoroutine = StartCoroutine(Sit_co());
                break;
            case State.Liedown:
                currentStateCoroutine = StartCoroutine(Liedown_co());
                break;
            case State.Catch:
                currentStateCoroutine = StartCoroutine(Catch_co(data as Transform));
                break;
            case State.Called:
                currentStateCoroutine = StartCoroutine(Called_co(data as Transform));
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
        //TEMP
        if (cubeRenderer != null)
            cubeRenderer.material.color = Color.white;
        //TEPM

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
                        if (currentintimacy >= 80)
                        {
                            rang = data.Active_MovingRang;
                            movespeed = data.Active_walkSpeed;
                        }
                        else if (currentintimacy >= 20)
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

    private IEnumerator InteractionRequest_co()
    {
        //TEMP
        if (cubeRenderer != null)
            cubeRenderer.material.color = Color.gray;
        //TEMP

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

    private IEnumerator Interaction_co()
    {
        //TEMP
        if (cubeRenderer != null)
            cubeRenderer.material.color = Color.green;
        //TEMP

        // 이 상태에 들어왔다는 것은 이미 선택이 완료되었다는 의미
        while (isSelected) // isSelected가 false가 될 때까지 대기
        {
            // 여기서 쓰다듬기, 손 주기 등 구체적인 상호작용을 기다리거나 처리
            yield return null;
        }
        EnterState(State.Wander); // 선택이 해제되면 Wander로 복귀
    }

    public void ConfirmSelection()
    {
        isSelected = true;
        EnterState(State.Interaction);
    }

    public void ReturnToWander()
    {
        isSelected = false;
        EnterState(State.Wander);
    }

    private IEnumerator Stroking_co()
    {
        //TEMP
        if (cubeRenderer != null)
            cubeRenderer.material.color = Color.blue;
        //TEMP

        Debug.Log("쓰다듬는 중...");
        agent.isStopped = true;

        // "Stroking_Loop" 같은 반복 애니메이션을 여기서 재생할 수 있습니다.
        // animator.SetBool("isStroking", true);

        // OnTriggerExit에 의해 상태가 변경될 때까지 이 상태를 무한히 유지합니다.
        while (true)
        {
            yield return null;
        }
    }

    public void CommandSit()
    {
        // 상호작용 대기 상태일 때만 '앉아' 명령을 수행
        if (currentState == State.Interaction)
        {
            EnterState(State.Sit);
        }
    }

    private IEnumerator Sit_co()
    {
        //TEMP
        if (cubeRenderer != null)
            cubeRenderer.material.color = Color.yellow;
        //TEMP

        Debug.Log($"{name}: 앉습니다.");
        agent.isStopped = true; // 앉는 동안은 움직이지 않음

        // "Sit" 애니메이션을 여기서 재생
        // animator.SetTrigger("Sit");

        // 5초 동안 앉아있음
        yield return new WaitForSeconds(5f);

        // "StandUp" 애니메이션을 여기서 재생
        // animator.SetTrigger("StandUp");

        // 앉기가 끝나면 다시 다른 상호작용을 기다리는 'Interaction' 상태로 복귀
        EnterState(State.Interaction);
    }

    public void CommandLiedown()
    {
        // 상호작용 대기 상태일 때만 '엎드려' 명령을 수행
        if (currentState == State.Interaction)
        {
            EnterState(State.Liedown);
        }
    }

    private IEnumerator Liedown_co()
    {
        //TEMP
        if (cubeRenderer != null)
            cubeRenderer.material.color = Color.magenta;
        //TEMP

        Debug.Log($"{name}: 엎드립니다.");
        agent.isStopped = true; // 앉는 동안은 움직이지 않음

        // "Sit" 애니메이션을 여기서 재생
        // animator.SetTrigger("Liedown");

        // 5초 동안 앉아있음
        yield return new WaitForSeconds(5f);

        // "StandUp" 애니메이션을 여기서 재생
        // animator.SetTrigger("StandUp");

        // 앉기가 끝나면 다시 다른 상호작용을 기다리는 'Interaction' 상태로 복귀
        EnterState(State.Interaction);
    }

    public void CommandCatch(Transform target)
    {
        if (currentState == State.Interaction)
        {
            EnterState(State.Catch, target);
        }
    }

    private IEnumerator Catch_co(Transform target)
    {
        // 0. 목표물이 유효한지 확인
        if (target == null)
        {
            Debug.LogWarning("물어올 대상이 없습니다!");
            EnterState(State.Interaction); // 목표가 없으면 바로 상호작용 상태로 복귀
            yield break; // 코루틴 즉시 종료
        }

        // TEMP: 상태 표시용 색상 변경
        if (cubeRenderer != null)
            cubeRenderer.material.color = Color.cyan;
        // TEMP

        Debug.Log($"{name}: {target.name} 물어올게요!");
        agent.isStopped = false;

        // 1. 목표물(막대기)을 향해 이동
        agent.SetDestination(target.position);

        // 목표물에 도착할 때까지 대기
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        // 2. 목표물 줍기
        Debug.Log("잡았다!");
        // 목표물에 붙어있는 스크립트를 가져와서 '줍기' 함수 호출
        Ch_Throwable item = target.GetComponent<Ch_Throwable>();
        if (item != null)
        {
            item.GetPickedUpBy(mouthTransform); // 입 위치로 아이템을 옮김
        }

        // 3. 플레이어에게 돌아가기
        Debug.Log("주인님에게 돌아갑니다!");
        agent.SetDestination(player.position);

        // 플레이어에게 도착할 때까지 대기
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        // 4. 목표물 내려놓기
        Debug.Log("가져왔어요!");
        if (item != null)
        {
            item.Drop(); // 아이템 내려놓기 함수 호출
        }

        // 5. 임무 완수 후 다시 상호작용 대기 상태로 복귀
        EnterState(State.Interaction);
    }

    private IEnumerator Hunger_co()
    {
        //TEMP
        if (cubeRenderer != null)
            cubeRenderer.material.color = Color.red;
        //TEMP
        
        Debug.Log($"{name}: 배고파요! 주인을 따라다닙니다.");
        isHunger = true;
        agent.isStopped = false;

        while (isHunger)
        {
            // 0.25초마다 한 번씩 플레이어의 위치로 목적지를 갱신
            agent.SetDestination(player.position);
            yield return new WaitForSeconds(0.25f);
        }

        EnterState(State.Wander);
    }

    public void ReceiveSnack()
    {
        if (currentState == State.Hunger)
        {
            Debug.Log($"{name}: 간식 고맙습니다!");
            hungerpercent = 100f;
            isHunger = false;

            // 친밀도를 올리거나 행복해하는 애니메이션을 잠시 보여줄 수 있음
            // animator.SetTrigger("Happy");
            currentintimacy += 5;
        }
    }

    private IEnumerator Toilet_co()
    {
        //TEMP
        if (cubeRenderer != null)
            cubeRenderer.material.color = Color.black;
        //TEMP
        
        isBowel = true;
        agent.SetDestination(ToiletPoint.position);

        yield return new WaitForSeconds(2f);

        isBowel = false;
        bowelpercent = 100f;
        EnterState(State.Wander);
    }

    public void BeCalled(Transform target)
    {
        // 다른 중요한 상태(상호작용 중 등)가 아닐 때만 호출에 응답
        if (currentState == State.Wander || currentState == State.Playing)
        {
            EnterState(State.Called, target);
        }
    }

    private IEnumerator Called_co(Transform target)
    {
        Debug.Log($"{name}: 부르셨나요? 지금 갑니다!");
        agent.isStopped = false;
        agent.SetDestination(target.position);

        // 목표 지점에 도착할 때까지 대기
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        Debug.Log($"{name}: 저 왔어요!");

        EnterState(State.InteractionRequest);
    }

    public State GetCurrentState()
    {
        return currentState;
    }
}
