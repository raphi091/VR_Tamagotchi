using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using TMPro;
using CustomInspector;

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
    [SerializeField] private GameObject nametext;

    public Transform mouthpoint;
    public Transform particlepoint;
    public Transform player;
    [AsRange(0f, 1f)] public Vector2 Hunger = new Vector2(0.2f, 0.5f);
    [AsRange(0f, 1f)] public Vector2 Toilet = new Vector2(0.1f, 0.4f);
    public float sightRange = 1f;
    public float loseSightRange = 2f;

    private PetController_J control;
    private NavMeshAgent agent;
    private CharacterController controller;
    private Animator animator;
    private AudioSource dogAudio;

    private float rotationSpeed = 10f;
    private float lastStateChangeTime;
    private bool isSelected = false;
    private bool isCalled = false;
    private bool isWandering;
    private bool isHunger = false;
    private bool isBowel = false;

    private Coroutine currentStateCoroutine;

    public PersonalityData_LES Data => data;

    [Header("Debug")]
    public bool debug = false;
    public TextMeshProUGUI text;


    private void Awake()
    {
        if (!TryGetComponent(out control))
            Debug.LogWarning("DogFSM ] PetController_J 없음");

        if (!TryGetComponent(out agent))
            Debug.LogWarning("DogFSM ] NavMeshAgent 없음");

        if (!TryGetComponent(out controller))
            Debug.LogWarning("DogFSM ] CharacterController 없음");

        if (!TryGetComponent(out animator))
            Debug.LogWarning("DogFSM ] Animator 없음");

        if (!TryGetComponent(out dogAudio))
            Debug.LogWarning("DogFSM ] AudioSource 없음");

        nametext.SetActive(false);

        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    private void Start()
    {
        EnterState(State.Setup);
    }

    private void Update()
    {
        control.currentHunger -= Time.deltaTime * Random.Range(Hunger.x, Hunger.y);
        control.currentBowel += Time.deltaTime * Random.Range(Toilet.x, Toilet.y);

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

        if (agent.velocity.magnitude > 0)
        {
            if (!dogAudio.isPlaying)
            {
                PlayWalk();
            }
            else
            {
                return;
            }
        }
        else if (!currentState.Equals(State.Stroking))
        {
            PlayStop();
        }

        if (nametext.activeSelf)
        {
            nametext.transform.LookAt(Camera.main.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand") && isSelected && currentState == State.Interaction)
        {
            EnterState(State.Stroking);
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHand") && currentState == State.Stroking)
        {
            PlayStop();
            ParticlePoolManager_LES.Instance.StopParticles(MoodType.Happy);

            EnterState(State.Interaction);            
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
        agent.Warp(transform.position);

        if (debug)
        {
            text.gameObject.SetActive(true);
            text.text = currentState.ToString();
        }
        else
        {
            text.gameObject.SetActive(false);
        }

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
                currentStateCoroutine = StartCoroutine(Called_co());
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
        yield return new WaitUntil(() => TutorialManager_J.instance.isTutorial == false);

        EnterState(State.Wander);
    }

    private IEnumerator Wander_co()
    {
        agent.isStopped = true;
        agent.ResetPath();
        isWandering = false;
        isCalled = false;
        animator.SetBool("INTERACT", false);

        while (true)
        {
            yield return null;

            if (IsPlayerInSight())
            {
                EnterState(State.InteractionRequest);
                yield break;
            }

            if (control.currentHunger <= 10f || isHunger)
            {
                EnterState(State.Hunger);
                yield break;
            }

            if (control.currentBowel >= 90f && !isHunger && ToiletPoint != null)
            {
                EnterState(State.Toilet);
                yield break;
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

                    if (randAction <= 5)
                    {
                        if (control.currentIntimacy >= 80)
                        {
                            rang = data.Active_MovingRang;
                            movespeed = data.Active_walkSpeed;
                        }
                        else if (control.currentIntimacy >= 20)
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
                    else if (randAction <= 8)
                    {
                        EnterState(State.Playing);
                    }
                    else
                    {
                        Debug.Log("월!");
                        animator.SetTrigger("BARK");

                        yield return new WaitForSeconds(4f);
                    }
                }
            }

            yield return null;
        }
    }

    private IEnumerator Play_co()
    {
        Scene scene = SceneManager.GetActiveScene();

        agent.isStopped = true;
        agent.ResetPath();

        int randAction = Random.Range(0, 10);
        float playtime;

        if (IsPlayerInSight())
        {
            EnterState(State.InteractionRequest);
            yield break;
        }

        if (scene.name.Equals("H_Outdoor"))
        {
            if (randAction <= 3)
            {
                animator.SetBool("PLAY", true);
                animator.SetInteger("PLAYNUM", 1);
                playtime = 3f;
            }
            else if (randAction <= 5)
            {
                animator.SetBool("PLAY", true);
                animator.SetInteger("PLAYNUM", 2);
                playtime = 4f;
            }
            else if (randAction <= 7)
            {
                animator.SetBool("PLAY", true);
                animator.SetInteger("PLAYNUM", 3);
                playtime = 8f;
            }
            else
            {
                animator.SetBool("PLAY", true);
                animator.SetInteger("PLAYNUM", 4);
                playtime = 10f;
            }
        }
        else
        {
            if (randAction <= 3)
            {
                animator.SetBool("PLAY", true);
                animator.SetInteger("PLAYNUM", 1);
                playtime = 3f;
            }
            else if (randAction <= 5)
            {
                animator.SetBool("PLAY", true);
                animator.SetInteger("PLAYNUM", 2);
                playtime = 4f;
            }
            else
            {
                animator.SetBool("PLAY", true);
                animator.SetInteger("PLAYNUM", 3);
                playtime = 8f;
            }
        }
        
        yield return new WaitForSeconds(playtime);

        animator.SetBool("PLAY", false);

        yield return new WaitForSeconds(0.5f);

        EnterState(State.Wander);
    }

    private IEnumerator InteractionRequest_co()
    {
        DogInteractionManager_K.instance.RequestInteraction(this);
        agent.isStopped = true;
        agent.ResetPath();
        ShowText(particlepoint.position);

        while (true)
        {
            if (control.currentHunger <= 10f || isHunger)
            {
                EnterState(State.Hunger);
                yield break;
            }

            if (control.currentBowel >= 90f && !isHunger && ToiletPoint != null)
            {
                EnterState(State.Toilet);
                yield break;
            }

            if (IsPlayerOutOfRange())
            {
                DogInteractionManager_K.instance.CancelRequest(this);
                ReturnToWander();
                yield break;
            }

            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            yield return null;
        }
    }

    private IEnumerator Interaction_co()
    {
        animator.SetBool("STROK", false);
        animator.SetBool("INTERACT", true);

        while (isSelected)
        {
            if (control.currentHunger <= 10f || isHunger)
            {
                EnterState(State.Hunger);
                yield break;
            }

            if (control.currentBowel <= 10f && !isHunger && ToiletPoint != null)
            {
                EnterState(State.Toilet);
                yield break;
            }

            if (IsPlayerOutOfRange())
            {
                DogInteractionManager_K.instance.CancelRequest(this);
                ReturnToWander();
                yield break;
            }

            yield return null;
        }

        EnterState(State.Wander);
    }

    public void ConfirmSelection()
    {
        isSelected = true;
        EnterState(State.Interaction);
    }

    public void ReturnToWander()
    {
        animator.SetBool("INTERACT", false);
        nametext.SetActive(false);
        isSelected = false;
        EnterState(State.Wander);
    }

    private IEnumerator Stroking_co()
    {
        Debug.Log("쓰다듬는 중...");
        agent.isStopped = true;

        animator.SetBool("STROK", true);
        PlayStrok();
        ParticlePoolManager_LES.Instance.PlayParticle(MoodType.Happy, particlepoint);

        while (true)
        {
            control.currentIntimacy += 0.5f * Time.deltaTime;
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
        Debug.Log($"{name}: 앉습니다.");
        agent.isStopped = true;
        animator.SetBool("SIT", true);

        yield return new WaitForSeconds(5f);

        animator.SetBool("SIT", false);

        control.currentIntimacy += 2f;
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
        Debug.Log($"{name}: 엎드립니다.");
        agent.isStopped = true;
        animator.SetBool("LIE", true);

        yield return new WaitForSeconds(5f);

        animator.SetBool("LIE", false);

        control.currentIntimacy += 3f;
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

        Debug.Log($"{name}: {target.name} 물어올게요!");
        agent.isStopped = false;

        animator.SetBool("INTERACT", false);

        agent.SetDestination(target.position);

        // 목표물에 도착할 때까지 대기
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        // 2. 목표물 줍기
        Debug.Log("잡았다!");
        animator.SetTrigger("PICKUP");
        // 목표물에 붙어있는 스크립트를 가져와서 '줍기' 함수 호출
        Ch_Throwable item = target.GetComponent<Ch_Throwable>();
        if (item != null)
        {
            item.GetPickedUpBy(mouthpoint); // 입 위치로 아이템을 옮김
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
        animator.SetTrigger("PUTDOWN");
        if (item != null)
        {
            item.Drop(); // 아이템 내려놓기 함수 호출
        }

        // 5. 임무 완수 후 다시 상호작용 대기 상태로 복귀
        control.currentIntimacy += 5f;
        EnterState(State.Interaction);
    }

    private IEnumerator Hunger_co()
    {
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
            control.currentHunger = 100f;
            isHunger = false;

            animator.SetTrigger("EAT");
            control.currentIntimacy += 7f;
        }
    }

    private IEnumerator Toilet_co()
    {
        isBowel = true;
        agent.SetDestination(ToiletPoint.position);

        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        PetController_J petcon = GetComponent<PetController_J>();

        petcon.petModelSlot.gameObject.SetActive(false);

        yield return new WaitForSeconds(5f);

        petcon.petModelSlot.gameObject.SetActive(true);

        isBowel = false;
        control.currentBowel = 0f;
        EnterState(State.Wander);
    }

    public void BeCalled()
    {
        // 다른 중요한 상태(상호작용 중 등)가 아닐 때만 호출에 응답
        if (currentState == State.Wander || currentState == State.Playing)
        {
            EnterState(State.Called);
        }
    }

    private IEnumerator Called_co()
    {
        animator.SetBool("PLAY", false);

        Debug.Log($"{name}: 부르셨나요? 지금 갑니다!");
        PlayBark();
        isCalled = true;
        agent.isStopped = false;

        // 목표 지점에 도착할 때까지 대기
        while (isCalled)
        {
            if (IsPlayerInSight())
            {
                EnterState(State.InteractionRequest);
                yield break;
            }

            agent.SetDestination(player.position);

            yield return null;
        }

        EnterState(State.InteractionRequest);
    }

    public State GetCurrentState()
    {
        return currentState;
    }

    private bool IsPlayerInSight()
    {
        return Vector3.Distance(transform.position, player.position) < sightRange;
    }

    private bool IsPlayerOutOfRange()
    {
        return Vector3.Distance(transform.position, player.position) > loseSightRange;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, loseSightRange);
    }

    public void PlayBark()
    {
        if (DatabaseManager_J.instance.petProfiles[control.petData.modelIndex].barkSound != null)
            dogAudio.PlayOneShot(DatabaseManager_J.instance.petProfiles[control.petData.modelIndex].barkSound);
    }

    private void PlayStrok()
    {
        if (DatabaseManager_J.instance.petProfiles[control.petData.modelIndex].strokSound != null)
        {
            Debug.Log(1);
            dogAudio.loop = true;
            dogAudio.clip = DatabaseManager_J.instance.petProfiles[control.petData.modelIndex].strokSound;
            dogAudio.Play();
        }
    }

    private void PlayStop()
    {
        if (DatabaseManager_J.instance.petProfiles[control.petData.modelIndex].strokSound != null && 
            DatabaseManager_J.instance.petProfiles[control.petData.modelIndex].indoorWalkSound != null &&
            DatabaseManager_J.instance.petProfiles[control.petData.modelIndex].outdoorWalkSound != null)
        {
            dogAudio.Stop();
            dogAudio.clip = null;
            dogAudio.loop = false;
        }
    }

    private void PlayWalk()
    {
        var scene = SceneManager.GetActiveScene();

        if (DatabaseManager_J.instance.petProfiles[control.petData.modelIndex].indoorWalkSound != null &&
            DatabaseManager_J.instance.petProfiles[control.petData.modelIndex].outdoorWalkSound != null)
        {
            if (scene.name == "H_Indoor")
            {
                dogAudio.loop = true;
                dogAudio.clip = DatabaseManager_J.instance.petProfiles[control.petData.modelIndex].indoorWalkSound;
                dogAudio.Play();
            }
            else
            {
                dogAudio.loop = true;
                dogAudio.clip = DatabaseManager_J.instance.petProfiles[control.petData.modelIndex].outdoorWalkSound;
                dogAudio.Play();
            }
        }
    }

    private void ShowText(Vector3 position)
    {
        position.y += 0.2f;
        nametext.transform.position = position;
        nametext.transform.LookAt(Camera.main.transform);

        nametext.SetActive(true);
    }
}
