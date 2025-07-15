using System.IO;
using UnityEngine;

public class DataManager_J : MonoBehaviour
{
    public static DataManager_J instance;

    // 디버깅용 프로퍼티 대신, 원래의 간단한 public 변수로 되돌립니다.
    public GameData gameData;

    private string saveFilePath;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        saveFilePath = Path.Combine(Application.persistentDataPath, "MyPetData.json");
    }

    public bool LoadGameData()
    {
        if (File.Exists(saveFilePath))
        {
            string jsonData = File.ReadAllText(saveFilePath);
            gameData = JsonUtility.FromJson<GameData>(jsonData);
            if (gameData == null)
            {
                gameData = new GameData();
                return false;
            }
            return true;
        }
        return false;
    }

    public void SaveGameData()
    {
        if (gameData == null) return;
        string jsonData = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(saveFilePath, jsonData);
    }
}
