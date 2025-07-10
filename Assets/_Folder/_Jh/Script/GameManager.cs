using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public List<PetController_J> petsInScene = new List<PetController_J>();

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
            UpdatePlacedPets();
        }
    }

    public void SaveChangesToDataManager()
    {
        Debug.Log("���� ����� �ֽ� ���¸� DataManager�� ������Ʈ�մϴ�.");
        foreach (PetController_J pet in petsInScene)
        {
            if (pet.gameObject.activeSelf)
            {
                // PetController�� �ǽð� ���� ���� ������(petData)�� �����
                pet.petData.hungerper = pet.currentHunger;
                pet.petData.intimacyper = pet.currentIntimacy;
                pet.petData.bowelper = pet.currentBowel;
            }
        }
    }

    public void GoToScene(string sceneName)
    {
        // 1. ���� ������ ���� ���� ���¸� ���� DataManager�� �����Ѵ�.
        SaveChangesToDataManager();

        // 2. ���ϴ� ������ �̵��Ѵ�.
        Debug.Log(sceneName + "���� �̵��մϴ�...");
        SceneManager.LoadScene(sceneName);
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


    // �Ϸ簡 ������ �� ȣ��� �Լ�
    public void EndOfDay()
    {
        Debug.Log("�Ϸ簡 ����Ǿ� ������ �ڵ� �����մϴ�.");

        SaveChangesToDataManager();

        // ������Ʈ�� �����ͷ� ���� ����
        DataManager_J.instance.SaveGameData();
    }
}