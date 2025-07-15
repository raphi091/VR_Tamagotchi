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
        Debug.Log($"명령: {command}");
        //Dog FSM manager Instance 연결코드 
    }
}
