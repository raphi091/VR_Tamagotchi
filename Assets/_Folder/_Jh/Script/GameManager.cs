using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public List<PetController_J> petsInScene = new List<PetController_J>();
    // 예시: 반에 들어갈 펫의 수
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

    // 씬 로드가 완료되면 이 함수가 실행됩니다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 게임 플레이 씬(실내 또는 실외)일 경우에만 초기화 로직 실행
        if (scene.name == "H_Indoor" || scene.name == "H_0utdoor")
        {
            InitializeGame();
        }
    }

    // 게임 초기화 함수
    void InitializeGame()
    {
        // 이어하기의 경우, DataManager는 이미 로비에서 로드되었거나,
        // 새 게임의 경우 LobbyManager가 데이터를 이미 만들어 둔 상태임.
        // 따라서 여기서는 펫 배치에만 집중.
        if (DataManager_J.instance.gameData != null && DataManager_J.instance.gameData.allPetData.Count > 0)
        {
            UpdatePlacedPets();
        }
    }

    // 로비에서 호출할 새 게임 데이터 생성 전용 함수
    public void CreateNewGameData(string className, List<PetStatusData_J> petsToCreate)
    {
        Debug.Log(className + "으로 새 게임을 시작합니다.");

        // 1. 데이터 객체 초기화
        DataManager_J.instance.gameData = new GameData();

        // 2. 반 이름과 전달받은 펫 리스트를 그대로 저장
        DataManager_J.instance.gameData.selectedClassName = className;
        DataManager_J.instance.gameData.allPetData = petsToCreate;
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
    }


    // 하루가 지났을 때 호출될 함수
    public void EndOfDay()
    {
        Debug.Log("하루가 종료되어 게임을 자동 저장합니다.");

        foreach (PetController_J pet in petsInScene)
        {
            if (pet.gameObject.activeSelf)
            {
                pet.petData.hungerper = pet.currentHunger;
                pet.petData.intimacyper = pet.currentIntimacy;
                pet.petData.bowelper = pet.currentBowel;
            }
        }

        // 업데이트된 데이터로 저장 실행
        DataManager_J.instance.SaveGameData();
    }
}