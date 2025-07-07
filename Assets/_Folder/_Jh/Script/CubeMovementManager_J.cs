using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CubeMovementManager_J : MonoBehaviour
{
    public Transform cube;
    public float moveRange = 5f;
    public float moveInterval = 2f;
    public float moveSpeed = 1.5f;

    private Vector3 basePosition;
    private Vector3 targetPosition;
    private bool isHeld = false;
    private bool isMoving = false;
    private float timer = 0f;

    private XRGrabInteractable grabInteractable;

    void Start()
    {
        if (cube == null) cube = GameObject.Find("Cube").transform;
        basePosition = cube.position;
        targetPosition = cube.position;

        grabInteractable = cube.GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    void Update()
    {
        if (isHeld) return;

        timer += Time.deltaTime;
        if (timer >= moveInterval && !isMoving)
        {
            timer = 0f;
            GenerateNewTarget();
        }

        if (isMoving)
        {
            cube.position = Vector3.MoveTowards(cube.position, targetPosition, moveSpeed * Time.deltaTime);
            cube.LookAt(new Vector3(targetPosition.x, cube.position.y, targetPosition.z));

            if (Vector3.Distance(cube.position, targetPosition) < 0.05f)
            {
                isMoving = false;
            }
        }
    }

    void GenerateNewTarget()
    {
        Vector3 randomPos = basePosition + new Vector3(
            Random.Range(-moveRange, moveRange),
            0f,
            Random.Range(-moveRange, moveRange)
        );

        randomPos = new Vector3(
            Mathf.Clamp(randomPos.x, -2.5f, 2.5f),
            cube.position.y,
            Mathf.Clamp(randomPos.z, -2.5f, 2.5f)
        );

        targetPosition = randomPos;
        isMoving = true;
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        isHeld = true;
        isMoving = false;
    }

    void OnReleased(SelectExitEventArgs args)
    {
        StartCoroutine(CheckGroundAfterRelease());
    }

    System.Collections.IEnumerator CheckGroundAfterRelease()
    {
        yield return new WaitForSeconds(0.5f);
        if (Physics.Raycast(cube.position, Vector3.down, out RaycastHit hit, 1f))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                isHeld = false;
            }
        }
    }
}
