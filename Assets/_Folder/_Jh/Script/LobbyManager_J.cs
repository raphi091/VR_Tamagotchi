using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;

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
    public ClassDisplayCard_J[] displayCards;

    private Dictionary<string, List<PetStatusData_J>> temporaryClassPets;
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
        CreateTemporaryPetData();

        newGameButton.SetActive(false);
        continueButton.SetActive(false);

        popUp.SetActive(true);
        SetupPreviewCards();
    }

    void CreateTemporaryPetData()
    {
        temporaryClassPets = new Dictionary<string, List<PetStatusData_J>>();
        string[] classNames = { "�޴Թ�", "�޴Թ�", "���Թ�" };

        foreach (string className in classNames)
        {
            List<PetStatusData_J> petsForThisClass = new List<PetStatusData_J>();
            for (int i = 0; i < 3; i++) // �� �ݸ��� 3������ ����
            {
                int profileIdx = Random.Range(0, DatabaseManager_J.instance.petProfiles.Count);
                int personalityIdx = Random.Range(0, DatabaseManager_J.instance.personalities.Count);
                int nameIdx = Random.Range(0, DatabaseManager_J.instance.PetNames.Count);

                PetStatusData_J tempPet = new PetStatusData_J(profileIdx, personalityIdx);
                tempPet.petName = DatabaseManager_J.instance.PetNames[nameIdx];
                petsForThisClass.Add(tempPet);
            }
            temporaryClassPets.Add(className, petsForThisClass);
        }
    }

        void SetupPreviewCards()
        {
        // 1. �������� ��õ�� temporaryClassPets(Dictionary)�� �������� �ݺ��մϴ�.
        int index = 0;
        foreach (var entry in temporaryClassPets)
        {
            // 2. ī�尡 �迭�� ������ ����� �ʴ��� Ȯ���մϴ�.
            if (index < displayCards.Length)
            {
                // 3. Dictionary���� key(�� �̸�)�� value(�� ����Ʈ)�� ���� �����ϴ�.
                string className = entry.Key;
                List<PetStatusData_J> petsToShow = entry.Value;

                // 4. �̷��� ������ ��ȿ�� ������ ī�忡 �����Ͽ� ������ �Ϸ��մϴ�.
                displayCards[index].SetupCard(className, petsToShow);

                // 5. ���� ī��� �Ѿ�� ���� �ε����� ������ŵ�ϴ�.
                index++;
            }
        }
    }


        public void OnClickContinue()
    {
        // �̾��ϱ⵵ ������ 'IndoorScene' (1���� ��)���� �̵�
        GameManager.instance.GoToScene("H_Indoor");
    }
    public void OnSelectClass(string className)
    {
        List<PetStatusData_J> selectedPets = temporaryClassPets[className];

        // 2. GameManager���� ���õ� 3������ �����ͷ� �� ������ ������ ���
        GameManager.instance.CreateNewGameData(className, selectedPets);

        // 3. ���� ������ �̵�
        GameManager.instance.GoToScene("H_Indoor");
        //GameManager
    }
}