using System.IO; // ������ �ٷ�� ���� �� �ʿ��մϴ�.
using UnityEngine;
public class DataManager_J : MonoBehaviour
{
    // �ٸ� ��ũ��Ʈ���� ���� ������ �� �ֵ��� static ������ �ڽ��� ���� (�̱���)
    public static DataManager_J instance;

    // ���� ������ ��� �����͸� ��� �ִ� ��ü
    public GameData gameData;

    // ����� ������ ��ü ���
    private string saveFilePath;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ���� �ٲ� �� ������Ʈ�� �ı����� ����
        }
        else
        {
            Destroy(gameObject);
        }

        // Application.persistentDataPath�� PC, ����� �� � ȯ�濡����
        // �����ϰ� ������ ������ �� �ִ� ��θ� �˷��ݴϴ�.
        saveFilePath = Path.Combine(Application.persistentDataPath, "MyPetData.json");
    }

    // JSON ���Ͽ��� ���� �����͸� �ҷ����� �Լ�
    public void LoadGameData()
    {
        // ����� ������ ������ �����ϴ��� Ȯ��
        if (File.Exists(saveFilePath))
        {
            // ������ �ִٸ�, ������ ��� �ؽ�Ʈ(JSON)�� �о��
            string jsonData = File.ReadAllText(saveFilePath);

            // JSON ���ڿ��� GameData ��ü�� ��ȯ�Ͽ� gameData ������ ���
            gameData = JsonUtility.FromJson<GameData>(jsonData);

            Debug.Log("������ �ҷ����� �Ϸ�.");
        }
        else
        {
            // ����� ������ ������, ���ο� ���� ������ ��ü�� ����
            Debug.Log("����� ���� ����. �� ������ �����մϴ�.");
            gameData = new GameData();
        }
    }

    // ���� ���� �����͸� JSON ���Ϸ� �����ϴ� �Լ�
    public void SaveGameData()
    {
        // gameData ��ü�� JSON ������ ���ڿ��� ��ȯ (true �ɼ��� ���� ���� ����)
        string jsonData = JsonUtility.ToJson(gameData, true);

        // ��ȯ�� JSON ���ڿ��� ���� ��ο� �ؽ�Ʈ ���Ϸ� ����
        File.WriteAllText(saveFilePath, jsonData);

        Debug.Log("������ ���� �Ϸ�: " + saveFilePath);
    }
}
