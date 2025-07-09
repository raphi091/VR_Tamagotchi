using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager_LES : MonoBehaviour
{
    public static GameManager_LES Instance; // 싱글톤으로 키세팅매니저 저장

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

    public void Main_To_InSide()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void InSide_To_Main()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void InSide_To_OutSide()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    public void OutSide_To_InSide()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void InSide_To_DogCareRoom()
    {
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }

    public void DogCareRoom_To_Inside()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
