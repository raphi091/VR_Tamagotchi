using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class LobbyManager_J : MonoBehaviour
{
    public static LobbyManager_J Instance = null;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject newGameButton;
    public GameObject continueButton;
    public GameObject popUp;
    private string saveFilePath;

    void Start()
    {
        popUp.SetActive(false);
        saveFilePath = Path.Combine(Application.persistentDataPath, "MyPetData.json");
        if (!File.Exists(saveFilePath))
        {
            continueButton.SetActive(false);
        }
    }

    public void OnClickNewGame()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }
        // 새 게임은 무조건 'IndoorScene' (1교시 씬)으로 이동
        //SceneManager.LoadScene("H_Indoor");
        newGameButton.SetActive(false);
        continueButton.SetActive(false);

        popUp.SetActive(true);
    }

    public void OnClickContinue()
    {
        // 이어하기도 무조건 'IndoorScene' (1교시 씬)으로 이동
        SceneManager.LoadScene("H_Indoor");
    }
    public void OnSelectClass()
    {
        
        //GameManager
    }
}