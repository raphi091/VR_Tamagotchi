using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class H_loadGame : MonoBehaviour, H_UI
{
    [SerializeField] private GameObject image;

    private void Awake()
    {
        image.SetActive(false);
    }

    public void OnPress()
    {
        StartCoroutine(OnPress_co());
    }

    private IEnumerator OnPress_co()
    {
        image.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        LobbyManager_J.Instance.OnClickContinue();
    }

    public void OnRelease()
    {
        
    }
}
