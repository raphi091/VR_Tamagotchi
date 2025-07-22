using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Ch_WetFood : MonoBehaviour
{
    [SerializeField] Ch_CannedFood cannedFood;

    [Range(0f, -1f), SerializeField] private float upsideDownRange = -0.7f;
    [SerializeField] private Transform startPoint;
    
    [SerializeField] private float duration;
    private bool isOut=false;
    private Ch_VelocityInteractable interactable;
    private bool isTweening = false;
    private Tween MyTween;

    void Awake()
    {
        cannedFood = GetComponentInChildren<Ch_CannedFood>();
        interactable = GetComponent<Ch_VelocityInteractable>();
    }

    void Start()
    {
        MyTween=cannedFood.transform.DOLocalMoveZ(0.01f, duration);
        MyTween.SetEase(Ease.OutQuad);
        MyTween.Pause();
    }


    private void Update()
    {
            if (Vector3.Distance(startPoint.position, cannedFood.endPoint.position) < 0.001f && !isOut)
            {
                isOut=true;
                cannedFood.OnOut();
            }
            if (transform.forward.y < upsideDownRange && !isOut)
            {
                isTweening = MyTween.IsPlaying();
                if (interactable.velocity.magnitude >= 0.5f &&!isTweening)
                {
                    Debug.Log("Start Drop");
                    MyTween.Restart();
                }
                else if(interactable.velocity.magnitude < 0.1f && isTweening)
                {
                    Debug.Log("Stop Drop");
                    MyTween.Pause();
                }
            }
        
    }
}
