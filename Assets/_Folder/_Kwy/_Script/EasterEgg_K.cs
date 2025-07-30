using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg_K : MonoBehaviour, H_UI
{
    [SerializeField] private GameObject remote;
    [SerializeField] private Transform spawnPoint;

    private GameObject i;


    private void Awake()
    {
        i = null;
    }

    public void OnPress()
    {
        if (i == null)
        {
            i = Instantiate(remote, spawnPoint);
            i.transform.position = spawnPoint.position;
        }
    }

    public void OnRelease()
    {
    }
}
