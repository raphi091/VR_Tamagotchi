using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Ch_Bell : XRSimpleInteractable
{
    public UnityEvent onBellRing=new UnityEvent();
    
    private Rigidbody rb;

    protected override void Awake()
    {
        
        base.Awake();
    }
}
