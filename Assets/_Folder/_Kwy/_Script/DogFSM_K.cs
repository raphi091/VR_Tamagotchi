using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class DogFSM_K : MonoBehaviour
{
    /*
    초창기에는 내부 공간 탐색
    자유놀기 혼자 논다 (친밀도에 따라 확률적으로 같이 논다)
    가까이 다가가면 상호작용 할지 안할지
    배고프면 간식
    */

    public enum State { Setup, Wander, Playing, Interaction, Hunger, Toilet}
    [Header("AI 상태")]
    [SerializeField] private State currentState;
    [SerializeField] private PersonalityData_LES data;

    private Transform player;
    private NavMeshAgent agent;
    private CharacterController controller;
    private Animator animator;
    private float lastStateChangeTime;
    private float hungerpercent;
    private float bowelpercent;
    private bool isWandering;

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
    }

    private void Update()
    {
        hungerpercent -= Time.deltaTime * 0.6f;
        bowelpercent -= Time.deltaTime * 0.6f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnterState(State.Interaction);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
        animator.SetFloat("MoveSpeed", 0);

        while (true)
        {
            yield return null;

            if (bowelpercent <= 10f)
            {
                EnterState(State.Toilet);
            }
            if (hungerpercent <= 10f)
            {
                EnterState(State.Hunger);
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
                if (Time.time > lastStateChangeTime + 5f)
                {
                    int randAction = Random.Range(0, 10);
                    float rang;
                    float movespeed;

                    if (randAction < 5)
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
                    else if (randAction < 9)
                    {
                        // 애니메이션 작업
                        lastStateChangeTime = Time.time;
                    }
                    else
                    {
                        EnterState(State.Playing);
                    }
                }
            }

            yield return null;
        }
    }

    private IEnumerator Play_co()
    {
        // 애니메이션 확인 후 작업

        yield return null;

        EnterState(State.Wander);
    }

    private IEnumerator Interaction_co()
    {
        yield return null;

        EnterState(State.Wander);
    }

    private IEnumerator Hunger_co()
    {
        yield return null;

        EnterState(State.Wander);
    }

    private IEnumerator Toilet_co()
    {
        yield return null;

        EnterState(State.Wander);
    }
}
