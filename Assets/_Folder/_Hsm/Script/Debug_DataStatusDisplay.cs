using UnityEngine;
using TMPro;

/// <summary>
/// DataManager�� ���� �� ������ ���� �ǽð����� ȭ�鿡 ǥ���ϴ� ����׿� ��ũ��Ʈ�Դϴ�.
/// </summary>
public class Debug_DataStatusDisplay : MonoBehaviour
{
    public static Debug_DataStatusDisplay instance;
    public TextMeshProUGUI statusText;

    void Awake()
    {
        // �� ����� HUD�� �̱������� ����� ���� �ٲ� �ı����� �ʰ� �մϴ�.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject.transform.root.gameObject); // ĵ���� ��ü�� �ı����� �ʵ��� root�� ����
        }
        else
        {
            Destroy(gameObject.transform.root.gameObject);
        }
    }

    // �� �����Ӹ��� ������ ���¸� Ȯ���ϰ� UI �ؽ�Ʈ�� ������Ʈ�մϴ�.
    void Update()
    {
        // statusText�� �Ҵ���� �ʾҴٸ� �ƹ��͵� ���� �ʽ��ϴ�.
        if (statusText == null) return;

        // DataManager �ν��Ͻ��� �ִ��� Ȯ���մϴ�.
        if (DataManager_J.instance == null)
        {
            statusText.text = "Status: DataManager is NULL";
            statusText.color = Color.red;
            return;
        }

        // GameData ��ü�� �ִ��� Ȯ���մϴ�.
        if (DataManager_J.instance.gameData == null)
        {
            statusText.text = "Status: GameData is NULL";
            statusText.color = Color.yellow;
            return;
        }

        // �� ������ ����Ʈ�� �ִ��� Ȯ���մϴ�.
        if (DataManager_J.instance.gameData.allPetData == null)
        {
            statusText.text = "Status: Pet List is NULL";
            statusText.color = Color.yellow;
            return;
        }

        // ��� �����Ͱ� �����̸�, ���� ���� ǥ���մϴ�.
        int petCount = DataManager_J.instance.gameData.allPetData.Count;
        statusText.text = $"Pet Count: {petCount}";

        // ���� ������ �����, ������ ������� ǥ���մϴ�.
        statusText.color = (petCount == 0) ? Color.yellow : Color.green;
    }
}


















