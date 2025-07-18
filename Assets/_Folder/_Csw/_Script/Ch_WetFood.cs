using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Ch_WetFood : MonoBehaviour
{
    [SerializeField] Ch_CannedFood cannedFood;

    [Range(0f, -1f), SerializeField] private float upsideDownRange = -0.7f;
    [SerializeField] private Transform startPoint;
    
    private float duration;
    private bool isOut=false;
    private Ch_VelocityInteractable interactable;

    void Awake()
    {
        cannedFood = GetComponentInChildren<Ch_CannedFood>();
        interactable = GetComponent<Ch_VelocityInteractable>();
    }

    void Start()
    {
        StartCoroutine(Update_Co());
    }

    private IEnumerator Update_Co()
    {
        while (true)
        {
            if (Vector3.Distance(startPoint.position, cannedFood.endPoint.position) < 0.01f)
            {
                isOut=true;
                cannedFood.OnOut();
                yield break;
            }
            if (transform.up.y < upsideDownRange)
            {
                if (interactable.velocity.magnitude >= 1f && !isOut)
                {
                    duration = 1f / (Mathf.Clamp(interactable.velocity.magnitude,1f,5f)*Time.deltaTime);
                    yield return cannedFood.transform.DOLocalMoveY(transform.localPosition.y + 0.05f, duration)
                        .SetEase(Ease.OutBack);

                }
            }
            else
            {
                cannedFood.transform.DOKill();
            }
        }
    }
}
