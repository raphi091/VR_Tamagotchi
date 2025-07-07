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
    // [SerializeField] private PetData data;

    private Transform player;
    private NavMeshAgent agent;
    private CharacterController controller;
    private Animator animator;
    private float lastStateChangeTime;
    private bool isWandering;

    private Coroutine currentStateCoroutine;


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

            // 배고플때
            // 배변활동

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

                    if (randAction < 5)
                    {

                        Vector3 randomDirection = Random.insideUnitSphere * 2 /*data.wanderRadius*/;
                        randomDirection += transform.position;

                        NavMeshHit hit;
                        NavMesh.SamplePosition(randomDirection, out hit, 2 /*data.wanderRadius*/, 1);
                        Vector3 finalPosition = hit.position;

                        agent.speed = 2 /*data.walkspeed*/;
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
