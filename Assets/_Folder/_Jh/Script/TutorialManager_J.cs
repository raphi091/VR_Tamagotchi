using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager_J : MonoBehaviour
{
    public static TutorialManager_J instance = null;
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

    public GameObject TutorialUI_1, TutorialUI_2, TutorialUI_3, TutorialUI_4;

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
        if (DataManager_J.instance.gameData.tutorial && scene.name == "H_Indoor")
        {
            
        }
        else if (DataManager_J.instance.gameData.tutorial && scene.name == "H_Lunch")
        {
            
        }
        else if (DataManager_J.instance.gameData.tutorial && scene.name == "H_Outdoor")
        {

        }
        else
        {
            return;
        }
    }

    public void InteractionTutorial()
    {
        
    }
    
}
