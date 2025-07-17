using DG.Tweening;
using UnityEngine;

public class Ch_WetFood : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Ch_CannedFood cannedFood;

    [Range(0f, -1f), SerializeField] private float upsideDownRange = -0.8f;
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
        if (transform.up.y < upsideDownRange&&rb.velocity.magnitude>0&&!isOut)
        {
            duration = Mathf.Clamp(rb.velocity.magnitude,0f,100f);
            cannedFood.transform.DOLocalMoveY(transform.localPosition.y + 0.01f, duration)
                .SetEase(Ease.OutBack)
                .WaitForCompletion();
        }
        else
        {

        }
    }
}
