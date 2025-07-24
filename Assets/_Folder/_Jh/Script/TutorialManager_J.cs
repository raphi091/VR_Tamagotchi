using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager_J : MonoBehaviour
{
    public static TutorialManager_J instance = null;

    private TutorialPageController page;

    public TutorialPageController Page => page;


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

        if (DataManager_J.instance.gameData.tutorial && scene.name == "H_Indoor")
        {
            page.gameObject.SetActive(false);  
        }
        else if (DataManager_J.instance.gameData.tutorial && scene.name == "H_Lunch")
        {
            page.gameObject.SetActive(false);
        }
        else if (DataManager_J.instance.gameData.tutorial && scene.name == "H_Outdoor")
        {
            page.gameObject.SetActive(false);
        }
        else
        {
            return;
        }
    }
}
