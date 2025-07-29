using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public List<PetController_J> petsInScene = new List<PetController_J>();

    public List<PetStatusData_J> getPetsInSceneData()
    {
        List<PetStatusData_J> petDataList = new List<PetStatusData_J>();

        foreach (PetController_J pet in petsInScene)
        {
            if(pet != null && pet.gameObject.activeSelf)
            {
                petDataList.Add(pet.petData);
            }
        }
        return petDataList;
    }
    

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

    // 씬 로드가 완료되면 이 함수가 실행됩니다.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[GameManager] 씬 로드됨: {scene.name}");

        // 게임 플레이 씬(실내 또는 실외)일 경우에만 초기화 로직 실행
        if (scene.name != "H_Lobby")
        {
            Debug.Log($"[GameManager] {scene.name} 씬 - UpdatePlacedPets 실행");
            UpdatePlacedPets();
        }
    }

    public void SaveChangesToDataManager()
    {
        Debug.Log("현재 펫들의 최신 상태를 DataManager에 업데이트합니다.");
        foreach (PetController_J pet in petsInScene)
        {
            if (pet.gameObject.activeSelf)
            {
                // PetController의 실시간 값을 영구 데이터(petData)에 덮어쓰기
                pet.petData.intimacyper = pet.currentIntimacy;
                pet.petData.hungerper = pet.currentHunger;
                pet.petData.bowelper = pet.currentBowel;
            }
        }
    }

    public void GoToScene(string sceneName)
    {
        Debug.Log($"[GameManager] 씬 전환 시작: {sceneName}");

        // 현재 씬이 H_Lunch가 아닐 때만 데이터 저장 시도
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != "H_Lunch")
        {
            try
            {
                SaveChangesToDataManager();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[GameManager] 씬 전환 중 데이터 저장 실패: {e.Message}");
            }
        }
        else
        {
            Debug.Log("[GameManager] 현재 씬이 H_Lunch → 데이터 저장 생략");
        }

        SceneManager.LoadScene(sceneName);
    }



    // 배치된 펫들에게 데이터를 적용하는 함수
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

        FindObjectOfType<Report_K>().SetupPetCard();
    }

    // 로비에서 호출할 새 게임 데이터 생성 전용 함수
    public void CreateNewGameData(string className, List<PetStatusData_J> petsToCreate)
    {
        // 1. 모든 정보가 담긴 '완성된' GameData 객체를 지역 변수로 먼저 만듭니다.
        GameData newGameData = new GameData();
        newGameData.selectedClassName = className;
        newGameData.allPetData = petsToCreate;

        // 2. 완성된 객체를 DataManager에 '단 한 번에' 할당합니다.
        // 이렇게 하면 중간에 데이터가 비어있는 상태가 발생하지 않습니다.
        DataManager_J.instance.gameData = newGameData;

        Debug.Log($"[GameManager] {className} 데이터 생성 완료. 펫: {petsToCreate.Count}마리");
    }


    // 하루가 지났을 때 호출될 함수
    public void EndOfDay()
    {
        Debug.Log("하루가 종료되어 게임을 자동 저장합니다.");

        SaveChangesToDataManager();
        if (DataManager_J.instance.gameData.Day.Equals(1))
        {
            DataManager_J.instance.gameData.tutorial = true;
        }

        DataManager_J.instance.gameData.Day++;

        // 업데이트된 데이터로 저장 실행
        DataManager_J.instance.SaveGameData();
    }
}