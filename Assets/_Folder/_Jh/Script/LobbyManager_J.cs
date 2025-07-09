using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class LobbyManager : MonoBehaviour
{
    public Button continueButton;
    private string saveFilePath;

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "MyPetData.json");
        if (!File.Exists(saveFilePath))
        {
            continueButton.interactable = false;
        }
    }

    public void OnClickNewGame()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }
        // 새 게임은 무조건 'IndoorScene' (1교시 씬)으로 이동
        SceneManager.LoadScene("IndoorScene_J");
    }

    public void OnClickContinue()
    {
        // 이어하기도 무조건 'IndoorScene' (1교시 씬)으로 이동
        SceneManager.LoadScene("IndoorScene_J");
    }
}