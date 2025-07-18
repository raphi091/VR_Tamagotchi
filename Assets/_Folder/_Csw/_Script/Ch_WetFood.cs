using DG.Tweening;
using UnityEngine;

public class Ch_WetFood : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Ch_CannedFood cannedFood;

    [Range(0f, -1f), SerializeField] private float upsideDownRange = -0.7f;
    [SerializeField] private Transform startPoint;
    private float duration;
    private bool isOut=false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cannedFood = GetComponentInChildren<Ch_CannedFood>();
    }

    private void Update()
    {
        if (Vector3.Distance(startPoint.position, cannedFood.endPoint.position) < 0.01f)
        {
            isOut=true;
            cannedFood.OnOut();
        }
        Debug.Log(rb.velocity.magnitude);
        if (transform.up.y < upsideDownRange)
        {
            Debug.Log("UpsideDown");
            if (rb.velocity.magnitude > 0f && !isOut)
            {
                duration = 1f / (Mathf.Clamp(rb.velocity.magnitude,1f,5f)*Time.deltaTime);
                cannedFood.transform.DOLocalMoveY(transform.localPosition.y + 0.001f, duration)
                    .SetEase(Ease.OutBack);
            }
        }
        else
        {

        }
    }
}
