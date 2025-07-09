using CustomInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideChecker : MonoBehaviour
{
    private void Awake()
    {
        UIManager.i.isInside = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            UIManager.i.isInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            UIManager.i.isInside = false;
    }
}