using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomXRInteractionManager : XRInteractionManager
{
    public void ManualUpdate()
    {
        ProcessInteractors(XRInteractionUpdateOrder.UpdatePhase.Dynamic);
        ProcessInteractables(XRInteractionUpdateOrder.UpdatePhase.Dynamic);
    }
}
