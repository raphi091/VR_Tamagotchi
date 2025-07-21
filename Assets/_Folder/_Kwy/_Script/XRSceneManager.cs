using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class XRSceneManager : MonoBehaviour
{
    public static XRSceneManager Instance = null;

    private CustomXRInteractionManager customInteractionManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        customInteractionManager = FindObjectOfType<CustomXRInteractionManager>();
    }

    void Update()
    {
        if (customInteractionManager != null && Time.timeScale < 0.01f)
        {
            customInteractionManager.ManualUpdate();
        }
    }
}
