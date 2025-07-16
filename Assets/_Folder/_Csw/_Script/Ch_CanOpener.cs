using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Ch_CanOpener : XRSimpleInteractable
{
    private Transform interactorTransform;
    private bool isGrabbed = false;
    private Vector3 interRotation;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        this.selectEntered.AddListener(OnLidGrabbed);
        this.selectExited.AddListener(OnLidLoose);
    }

    protected override void OnDisable()
    {
        this.selectEntered.RemoveListener(OnLidGrabbed);
        this.selectExited.RemoveListener(OnLidLoose);
        interactorTransform = null;
        isGrabbed=false;
        base.OnDisable();
    }

    void Update()
    {
        if (isGrabbed&&interactorTransform!=null)
        {
            Debug.Log("잡힘");
            
            this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles + interactorTransform.rotation.eulerAngles - interRotation);
        }
    }

    void OnLidGrabbed(SelectEnterEventArgs args)
    {
        interactorTransform = args.interactorObject.transform;
        interRotation=interactorTransform.rotation.eulerAngles;
        isGrabbed=true;
    }

    void OnLidLoose(SelectExitEventArgs args)
    {
        interactorTransform = null;
        isGrabbed = false;
    }
}
