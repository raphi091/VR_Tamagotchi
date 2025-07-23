using System.IO;
using UnityEngine;

public class DataManager_J : MonoBehaviour
{
    public static DataManager_J instance;

    [Header("GameData")]
    public GameData gameData;

    [Header("Settiong")]
    public GameSetting setting;
    private string settingFilePath;

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
        settingFilePath = Path.Combine(Application.persistentDataPath, "setting.json");
        LoadSettings();
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

    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(setting, true);
        File.WriteAllText(settingFilePath, json);
    }

    public void LoadSettings()
    {
        if (File.Exists(settingFilePath))
        {
            string json = File.ReadAllText(settingFilePath);
            setting = JsonUtility.FromJson<GameSetting>(json);
        }
        else
        {
            setting = new GameSetting();
        }
    }
}
