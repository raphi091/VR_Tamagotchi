using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public List<PetController_J> petsInScene = new List<PetController_J>();
    // ����: �ݿ� �� ���� ��
    private const int PETS_PER_CLASS = 3;

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
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // �� �ε尡 �Ϸ�Ǹ� �� �Լ��� ����˴ϴ�.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���� �÷��� ��(�ǳ� �Ǵ� �ǿ�)�� ��쿡�� �ʱ�ȭ ���� ����
        if (scene.name == "H_Indoor" || scene.name == "H_0utdoor")
        {
            InitializeGame();
        }
    }

    // ���� �ʱ�ȭ �Լ�
    void InitializeGame()
    {
        // �̾��ϱ��� ���, DataManager�� �̹� �κ񿡼� �ε�Ǿ��ų�,
        // �� ������ ��� LobbyManager�� �����͸� �̹� ����� �� ������.
        // ���� ���⼭�� �� ��ġ���� ����.
        if (DataManager_J.instance.gameData != null && DataManager_J.instance.gameData.allPetData.Count > 0)
        {
            UpdatePlacedPets();
        }
    }

    // �κ񿡼� ȣ���� �� ���� ������ ���� ���� �Լ�
    public void CreateNewGameData(string className, List<PetStatusData_J> petsToCreate)
    {
        Debug.Log(className + "���� �� ������ �����մϴ�.");

        // 1. ������ ��ü �ʱ�ȭ
        DataManager_J.instance.gameData = new GameData();

        // 2. �� �̸��� ���޹��� �� ����Ʈ�� �״�� ����
        DataManager_J.instance.gameData.selectedClassName = className;
        DataManager_J.instance.gameData.allPetData = petsToCreate;
    }

    // ��ġ�� ��鿡�� �����͸� �����ϴ� �Լ�
    void UpdatePlacedPets()
    {
        petsInScene.Clear();
        petsInScene.AddRange(FindObjectsOfType<PetController_J>());

        List<PetStatusData_J> loadedPetData = DataManager_J.instance.gameData.allPetData;

        for (int i = 0; i < petsInScene.Count; i++)
        {
            if (i < loadedPetData.Count)
            {
                petsInScene[i].ApplyData(loadedPetData[i]);
            }
            else
            {
                petsInScene[i].gameObject.SetActive(false);
            }
        }
    }


    // �Ϸ簡 ������ �� ȣ��� �Լ�
    public void EndOfDay()
    {
        Debug.Log("�Ϸ簡 ����Ǿ� ������ �ڵ� �����մϴ�.");

        foreach (PetController_J pet in petsInScene)
        {
            if (pet.gameObject.activeSelf)
            {
                pet.petData.hungerper = pet.currentHunger;
                pet.petData.intimacyper = pet.currentIntimacy;
                pet.petData.bowelper = pet.currentBowel;
            }
        }

        // ������Ʈ�� �����ͷ� ���� ����
        DataManager_J.instance.SaveGameData();
    }
}