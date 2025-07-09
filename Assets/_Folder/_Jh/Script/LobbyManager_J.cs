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
        // �� ������ ������ 'IndoorScene' (1���� ��)���� �̵�
        SceneManager.LoadScene("IndoorScene_J");
    }

    public void OnClickContinue()
    {
        // �̾��ϱ⵵ ������ 'IndoorScene' (1���� ��)���� �̵�
        SceneManager.LoadScene("IndoorScene_J");
    }
}