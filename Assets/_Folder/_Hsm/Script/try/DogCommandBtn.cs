using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DogCommandBtn : MonoBehaviour
{
    public string command;

    private void Start()
    {
        GetComponent<XRSimpleInteractable>().selectEntered.AddListener(OnSelect);
    }
    private void OnSelect(SelectEnterEventArgs args)
    {
        Debug.Log($"���: {command}");
        //Dog FSM manager Instance �����ڵ� 
    }
}
