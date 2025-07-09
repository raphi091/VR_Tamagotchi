using CustomInspector;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class bulletinboardUI : MonoBehaviour
{
    [ReadOnly] public bool isInside;
    [ReadOnly] public bool isPlayer;

    public GameObject select;
    public GameObject Panel;

    private void Awake()
    {
        isInside = false;
        isPlayer = false;
        select.SetActive(false);
        Panel.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInside = true;
            isPlayer = true;
            select.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInside = false;
            isPlayer = false;
            select.SetActive(false);
        }
    }

    public void Open(GameObject g)
    {
        g.SetActive(true);
    }

    public void Close(GameObject g)
    {
        g.SetActive(false);
    }
}