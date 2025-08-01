using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Snack : MonoBehaviour
{
    [SerializeField] private GameObject model;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
        SetUp(false);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "H_Indoor" || scene.name == "H_Outdoor")
            SetUp(true);
        else
            SetUp(false);
    }

    public void SetUp(bool b)
    {
        model.SetActive(b);
    }
}
