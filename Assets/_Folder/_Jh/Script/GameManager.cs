using UnityEngine;
public class GameManager : MonoBehaviour
{
    // ����: �ݿ� �� ���� ��
    private const int PETS_PER_CLASS = 3;

    void Start()
    {
        // ���� ���� �� ������ �ҷ�����
        DataManager_J.instance.LoadGameData();

        // ���� ����� �����Ͱ� ���ٸ� (ó�� ����)
        if (DataManager_J.instance.gameData.allPetData.Count == 0)
        {
            Debug.Log("����� ������ ����, �� ���� UI Ȱ��ȭ!");
            // ���⼭ �� ���� UI�� �����ִ� ������ �����մϴ�.
        }
        else
        {
            // ����� �����Ͱ� �ִٸ� �ش� �����ͷ� ����� �����մϴ�.
            InstantiateSavedPets();
        }
    }

    // UI ��ư�� ������ �Լ�
    public void OnClassSelected(string className)
    {
        Debug.Log(className + " ���õ�! ���� �������� �����մϴ�.");
        DataManager_J.instance.gameData.selectedClassName = className;

        // �ݿ� �� ����� �������� ����
        for (int i = 0; i < PETS_PER_CLASS; i++)
        {
            // �𵨰� ������ �������� ����
            int randomModelIndex = Random.Range(0, DatabaseManager_J.instance.allModels.Count);
            int randomPersonalityIndex = Random.Range(0, DatabaseManager_J.instance.personalities.Count);

            // ������ ���� �� DataManager�� �߰�
            PetStatusData_J newPet = new PetStatusData_J(randomModelIndex, randomPersonalityIndex);

            // �̸� �������� ����
            int randomNameIndex = Random.Range(0, DatabaseManager_J.instance.PetNames.Count);
            newPet.petName = DatabaseManager_J.instance.PetNames[randomNameIndex];

            DataManager_J.instance.gameData.allPetData.Add(newPet);
        }

        // ����� ���� ������ ����
        InstantiateSavedPets();
    }

    // ����� �����͸� �������� �� ������Ʈ���� ���� �����ϴ� �Լ�
    void InstantiateSavedPets()
    {
        foreach (PetStatusData_J petData in DataManager_J.instance.gameData.allPetData)
        {
            // �����Ϳ� �´� �� ������ ��������
            GameObject petPrefab = DatabaseManager_J.instance.allModels[petData.modelIndex];
            // �� ����
            GameObject petInstance = Instantiate(petPrefab, Vector3.zero, Quaternion.identity);
            // ���⿡ ���� �̸�, ���� ���� ���� petInstance�� �Ѱ��ִ� ���� �߰�
        }
    }


    // �Ϸ簡 ������ �� ȣ��� �Լ�
    public void EndOfDay()
    {
        Debug.Log("�Ϸ簡 ����Ǿ� ������ �ڵ� �����մϴ�.");

        // ���� ����� ��ġ�� gameData�� ������Ʈ�ϴ� ������ �ʿ��մϴ�.
        // ��: for (int i=0; i < allPets.Count; i++) {
        //         DataManager.instance.gameData.allPetData[i].hunger_level = allPets[i].currentHunger;
        //     }

        // ������Ʈ�� �����ͷ� ���� ����
        DataManager_J.instance.SaveGameData();
    }
}