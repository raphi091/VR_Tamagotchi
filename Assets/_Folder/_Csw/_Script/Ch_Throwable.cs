using UnityEngine;

public class Ch_Throwable : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;

    private bool isPicked = false;
    private bool isGrounded = false;


    private void Awake()
    {
        if (!TryGetComponent(out rb))
            Debug.Log("Ch_Throwable ] Rigidbody 없음");

        if (!TryGetComponent(out col))
            Debug.Log("Ch_Throwable ] Collider 없음");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if (isPicked == true)
            {
                Debug.Log("막대기가 땅에 떨어졌습니다.");
                isGrounded = true;
                isPicked=false;

                Debug.Log(this.name + "가 땅에 닿았습니다. 모든 강아지에게 알립니다.");

                DogFSM_K[] allDogs = FindObjectsOfType<DogFSM_K>();

                foreach (DogFSM_K dog in allDogs)
                {
                    dog.CommandCatch(this.transform);
                }
            }
            else
            {
                isGrounded = true;
                isPicked=false;
            }
        }
    }

    public void GetReady()
    {
        isPicked = true;
    }

    public void GetPickedUpBy(Transform newParent)
    {
        rb.isKinematic = true; // 물리 효과 정지
        col.enabled = false;   // 충돌 정지
        transform.SetParent(newParent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    // 강아지가 내려놓았을 때 호출될 함수
    public void Drop()
    {
        transform.SetParent(null); // 부모-자식 관계 해제
        rb.isKinematic = false;  // 물리 효과 다시 활성화
        col.enabled = true;    // 충돌 다시 활성화
    }
}
