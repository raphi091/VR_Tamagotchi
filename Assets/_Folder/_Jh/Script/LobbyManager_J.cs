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
        string[] classNames = { "햇님반", "달님반", "별님반" };

        foreach (string className in classNames)
        {
            List<PetStatusData_J> petsForThisClass = new List<PetStatusData_J>();
            for (int i = 0; i < 3; i++) // 각 반마다 3마리씩 생성
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
        // 1. 데이터의 원천인 temporaryClassPets(Dictionary)를 기준으로 반복합니다.
        int index = 0;
        foreach (var entry in temporaryClassPets)
        {
            // 2. 카드가 배열의 범위를 벗어나지 않는지 확인합니다.
            if (index < displayCards.Length)
            {
                // 3. Dictionary에서 key(반 이름)와 value(펫 리스트)를 먼저 꺼냅니다.
                string className = entry.Key;
                List<PetStatusData_J> petsToShow = entry.Value;

                // 4. 이렇게 꺼내온 유효한 정보를 카드에 전달하여 설정을 완료합니다.
                displayCards[index].SetupCard(className, petsToShow);

                // 5. 다음 카드로 넘어가기 위해 인덱스를 증가시킵니다.
                index++;
            }
        }
    }


        public void OnClickContinue()
    {
        // 이어하기도 무조건 'IndoorScene' (1교시 씬)으로 이동
        GameManager.instance.GoToScene("H_Indoor");
    }
    public void OnSelectClass(string className)
    {
        List<PetStatusData_J> selectedPets = temporaryClassPets[className];

        // 2. GameManager에게 선택된 3마리의 데이터로 새 게임을 만들라고 명령
        GameManager.instance.CreateNewGameData(className, selectedPets);

        // 3. 게임 씬으로 이동
        GameManager.instance.GoToScene("H_Indoor");
        //GameManager
    }
}