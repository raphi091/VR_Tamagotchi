using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager_J : MonoBehaviour
{
    public static TutorialManager_J instance = null;

    private TutorialPageController page;

    public TutorialPageController Page => page;
    public bool isTutorial = true;


    private void Awake()
    {
        if( instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneTutorial;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneTutorial;
    }

    private void OnSceneTutorial(Scene scene, LoadSceneMode mode)
    {
        page = FindObjectOfType<TutorialPageController>();

        if (DataManager_J.instance.gameData.tutorial == false)
        {
            isTutorial = true;
        }

        if (DataManager_J.instance.gameData.tutorial && scene.name == "H_Indoor")
        {
            isTutorial = false;
            page.gameObject.SetActive(false);  
        }
        else if (DataManager_J.instance.gameData.tutorial && scene.name == "H_Lunch")
        {
            isTutorial = false;
            page.gameObject.SetActive(false);
        }
        else if (DataManager_J.instance.gameData.tutorial && scene.name == "H_Outdoor")
        {
            isTutorial = false;
            page.gameObject.SetActive(false);
        }
        else
        {
            return;
        }
    }
}
